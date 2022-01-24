using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using BuildingGraph.Client.Kafka;
using BuildingGraph.Client;
using System.IO;
using BuildingGraph.Integration.Revit.Streams;


namespace BuildingGraph.Integration.Revit.UIAddin
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

            RevitEventDispatcher.Init();

           
  
            var streamConsumer = new RevitStreamConsumer();       
            var bc = new BuildingGraphClient(@"http://localhost:4002/graphql", null);
            var spacesEsh = new ElementStreamHandler(RevitEventDispatcher.Current, bc);
            streamConsumer.StreamHandlers.Add(spacesEsh);
            streamConsumer.Start();

            return Result.Succeeded;
        }

        private void Kc_NewMessageArrived(StreamMessage message)
        {
            RevitEventDispatcher.Current.QueueAction((UIApplication app) => {




            });
        }

        public System.Windows.Window GraphAppWindow { get; set; }
        public RevitToGraphPublisherSettings SessionSettings { get; private set; }




    }

    //public class QucikStreamHandler
}