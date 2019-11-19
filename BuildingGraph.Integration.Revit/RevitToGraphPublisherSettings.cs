using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Integrations.Revit
{
    public class RevitToGraphPublisherSettings
    {
        public RevitToGraphPublisherSettings()
        {
            IncludeElectrical = true;
            IncludeMechanical = true;
            DBHost = "localhost";
            DBPort = 7687;
            DBUsername = "neo4j";
        }

        public bool IncludeBoundaries { get; set; }
        public bool IncludeMechanical { get; set; }
        public bool IncludeElectrical { get; set; }

        public string DBPassword { get; set; }
        public string DBUsername { get; set; }
        public string DBHost { get; set; }
        public int DBPort { get; set; }
    }
}
