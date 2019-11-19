using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Nito.AsyncEx;
using BuildingGraph.Client;
using HLApps.Revit.Parameters;

namespace BuildingGraph.Integrations.Revit
{

    public class RevitGraphSyncService
    {


        public void Sync(Document doc)
        {
            var bc = new BuildingGraphClient(@"http://localhost:4001/graphql", null);

            var query = @"{ChangeRequest (IsComplete:false){Changes{ParameterName, NewValue, ChangeSource{BIMRevitElementID}}}}";
            var vars = new Dictionary<string, object>();
            vars.Add("IsComplete", false);

            var task = Task.Run(() => AsyncContext.Run(() => bc.ExecuteQuery(query, vars)));
            //after a lot of issues with thread blocking... this was found to be a working interim solution.
            //see https://msdn.microsoft.com/en-us/magazine/mt238404.aspx for explanation
            //ideally, we wouldn't block Revit's main thread, but in most cases the query will complete very quickly.
            //The majority of time spent is in writing the parameter values with Revit which, due to constraints of the Revit API, can only be synchronous.
            //so not a lot to be gained by making this truly asynchronous

            foreach (dynamic cr in task.Result.ChangeRequest)
            {
                foreach (var cg in cr.Changes)
                {
                    var peramName = cg.ParameterName.Value;
                    var newValue = cg.NewValue.Value;
                    var elmId = cg.ChangeSource.BIMRevitElementID.Value;

                    var elm = doc.GetElement(new ElementId((int)elmId));
                    if (elm == null) continue;
                    var param = elm.GetParameters((string)peramName).FirstOrDefault();
                    if (param == null) continue;
                    var hlParam = new HLRevitParameter(param);
                    hlParam.Value = newValue;
                }
            }

            var docIdent = HLApps.Revit.Utils.DocUtils.GetDocumentIdent(doc);
            var docVars = new Dictionary<string, object>();
            docVars.Add("modelIdent", docIdent);
            var modelElementsQuery = @"query($modelIdent:String){
  Model (Identity:$modelIdent){
    Identity
    ModelElements {
      UniqueId
      AbstractElement {
        __typename
        Name
      }
    }
  }
}";

            var modelElementQResult = bc.ExecuteQuery(modelElementsQuery, docVars);
            var model = modelElementQResult.Result.Model;

        }


    }
}
