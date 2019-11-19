using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace BuildingGraph.Integrations.Revit.UIAddin
{

    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class GraphApp : IExternalApplication
    {
        public static GraphApp Instance;

        public GraphApp()
        {
            Instance = this;
        }

        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {

            var appPanel = application.CreateRibbonPanel(Tab.AddIns, "HLApps");
            var showAppBtn = new PushButtonData("hleaPublishToGraph", "Publish to Graph", System.Reflection.Assembly.GetExecutingAssembly().Location, typeof(GraphAppShowCommand).FullName);
            showAppBtn.ToolTip = "Publish the current Revit model to a graph database";
            appPanel.AddItem(showAppBtn);

            var syncBtn = new PushButtonData("hleaSyncWithGraph", "Pull Changes", System.Reflection.Assembly.GetExecutingAssembly().Location, typeof(RevitGraphSyncCommand).FullName);
            appPanel.AddItem(syncBtn);
            //TODO: persist settings with local storage
            SessionSettings = new RevitToGraphPublisherSettings();

            return Result.Succeeded;
        }

        public System.Windows.Window GraphAppWindow { get; set; }
        public RevitToGraphPublisherSettings SessionSettings { get; private set; }
    }
}