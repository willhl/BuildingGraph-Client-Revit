using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BuildingGraph.Integrations.Revit.UIAddin
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class RevitGraphSyncCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string message, ElementSet elements)
        {

            Document rDoc = cmdData.Application.ActiveUIDocument.Document;

            using (Transaction tx = new Transaction(rDoc, "gSync"))
            {
                tx.Start("gSync");
                
                var bc = new RevitGraphSyncService();
                bc.Sync(rDoc);

                tx.Commit();
            }


                return Result.Succeeded;

        }
    }
}
