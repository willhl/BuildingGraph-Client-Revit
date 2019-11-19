using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using BuildingGraph.Integrations.Revit.UIAddin.ViewModel;


namespace BuildingGraph.Integrations.Revit.UIAddin
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class GraphAppShowCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string message, ElementSet elements)
        {
            Document rDoc = cmdData.Application.ActiveUIDocument.Document;
            var gdApp = GraphApp.Instance;
            var publisher = new RevitToGraphPublisher(rDoc);
            GraphAppViewModel gvm = new GraphAppViewModel(publisher, gdApp);
            gdApp.GraphAppWindow = new GraphAppWindow(gvm);
            gdApp.GraphAppWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}