using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Nito.AsyncEx;
using BuildingGraph.Client;
using HLApps.Revit.Parameters;

namespace BuildingGraph.Integration.Revit
{

    public class RevitGraphSyncService
    {


        public void Sync(Document doc)
        {
 
            var client = new BuildingGraphClient(@"https://hlbuildinggraph.azure-api.net/dev-external/graphql?subscription-key=e24f1d99266045f4b7ff2656b05d7ddb");

            var query = @"query($identity:String) {
  ParameterChange (filter:{IsClosed:false ModelElement_in:{Model_in:{Identity:$identity}}})
  {
      ParameterName
      ModelElement{
        UniqueId
      }
      AbstractElement{
        Id
      }
      NewValue
      OldValue
  }
}";

            Dictionary<string, object> vars = new Dictionary<string, object>();

            var docIdent = HLApps.Revit.Utils.DocUtils.GetDocumentIdent(doc);
            vars.Add("identity", "8764c510-57b7-44c3-bddf-266d86c26380-0000c160:C:\\Users\\reynoldsw\\Desktop\\Ventilation Pressure Drop Model\\Ventilation Pressure Drop Model.rvt");

            var result = client.ExecuteQuery(query, vars);


            foreach (var parameterChange in result.ParameterChange)
            {
                if (parameterChange.NewValue != null)
                {

                    var qlperamName = parameterChange.ParameterName.Value;
                    var newValue = parameterChange.NewValue.Value;
                    var elmId = parameterChange.ModelElement.UniqueId.Value;
                   

                    Element elm = doc.GetElement(elmId);
                    if (elm == null) continue;


                    foreach (var param in elm.Parameters.OfType<Parameter>())
                    {
                        if (param.IsReadOnly) continue;

                        var hp = new HLRevitParameter(param);
                        var paramName = Utils.GetGraphQLCompatibleFieldName(param.Definition.Name);

                        if (qlperamName == paramName)
                        {
                            hp.Value = newValue;
                        }
                    }

                }
            }


        }


    }
}
