using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingGraph.Client.Neo4j;

namespace BuildingGraph.Integrations.Revit.UIAddin.ViewModel
{
    class GraphAppViewModel : BaseViewModel
    {
        GraphApp _app;
        RevitToGraphPublisherSettings _settings;
        RevitToGraphPublisher _publisher;
        public GraphAppViewModel(RevitToGraphPublisher pubisher, GraphApp app)
        {
            _publishToGraphCommand = new Command(PublishToGraph);
            _cancelCommand = new Command(Close);
            _publisher = pubisher;
            _settings = app.SessionSettings;
            _app = app;
        }

        public string Username
        {
            get
            {
                return _settings.DBUsername;
            }
            set
            {
                _settings.DBUsername = value;
                NotifyPropertyChanged("Username");
            }
        }


        public string Password
        {
            get
            {
                return _settings.DBPassword;
            }
            set
            {
                _settings.DBPassword = value;
                NotifyPropertyChanged("Password");
            }
        }

        public string Host
        {
            get
            {
                return _settings.DBHost;
            }
            set
            {
                _settings.DBHost = value;
                NotifyPropertyChanged("Host");
            }
        }
        public int Port
        {
            get
            {
                return _settings.DBPort;
            }
            set
            {
                _settings.DBPort = value;
                NotifyPropertyChanged("Port");
            }
        }

        public bool IncludeBoundaries
        {
            get
            {
                return _settings.IncludeBoundaries;
            }
            set
            {
                _settings.IncludeBoundaries = value;
                NotifyPropertyChanged("IncludeBoundaries");
                NotifyPropertyChanged("CanPublish");
            }
        }
        public bool IncludeMechanical
        {
            get
            {
                return _settings.IncludeMechanical;
            }
            set
            {
                _settings.IncludeMechanical = value;
                NotifyPropertyChanged("IncludeMechanical");
                NotifyPropertyChanged("CanPublish");
            }
        }
        public bool IncludeElectrical
        {
            get
            {
                return _settings.IncludeElectrical;
            }
            set
            {
                _settings.IncludeElectrical = value;
                NotifyPropertyChanged("IncludeElectrical");
                NotifyPropertyChanged("CanPublish");
            }
        }

        public bool CanPublish
        {
            get
            {
                return IncludeBoundaries || IncludeMechanical || IncludeElectrical;
            }
        }

        Command _publishToGraphCommand;
        public Command PublishToGraphCommand
        {
            get
            {
                return _publishToGraphCommand;
            }
        }

        Command _cancelCommand;
        public Command CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
        }

        void PublishToGraph()
        {
            var client = new Neo4jClient(new Uri(string.Format("bolt://{0}:{1}", Host, Port)), Username, Password);
            _publisher.Publish(_settings, client);
            Autodesk.Revit.UI.TaskDialog.Show("Publish complete", "The current revit model has been successfully published to the graph database.");
            Close();
        }

        void Close()
        {
            var appWindow = _app.GraphAppWindow;
            if (appWindow != null && appWindow.IsVisible) appWindow.Close();
        }
    }
}
