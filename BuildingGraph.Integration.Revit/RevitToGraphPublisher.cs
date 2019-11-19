using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using BuildingGraph.Client;

namespace BuildingGraph.Integrations.Revit
{
    public class RevitToGraphPublisher
    {
        Document _rdoc;
        public RevitToGraphPublisher(Document doc)
        {
            _rdoc = doc;
        }

        public void Publish(RevitToGraphPublisherSettings settings, IGraphDBClient client)
        {
            var meGraph = new MEPRevitGraph();
            MEPRevitGraphWriter mps = new MEPRevitGraphWriter(meGraph);

            if (settings.IncludeElectrical) mps.Parsers.Add(new Parsers.MEPGraphParserElectrical());
            if (settings.IncludeMechanical) mps.Parsers.Add(new Parsers.MEPGraphParserConnectors());
            if (settings.IncludeBoundaries) mps.Parsers.Add(new Parsers.MEPGraphParserSpaces());
            //if (settings.IncludeBoundaries) mps.Parsers.Add(new Parsers.ClashDetection());
            mps.Parsers.Add(new Parsers.MEPGraphParserIoT());

            if (mps.Parsers.Count == 0) return;

            var mecFEc = new FilteredElementCollector(_rdoc);
            var mecFilter = mps.GetFilterForAllParsers;
            var mecElements = mecFEc.WherePasses(mecFilter).WhereElementIsNotElementType().ToElements();

            var wfic = new FilteredElementCollector(_rdoc);
            var geoElementsFilter = new ElementMulticategoryFilter(new BuiltInCategory[] { BuiltInCategory.OST_Floors, BuiltInCategory.OST_Roofs, BuiltInCategory.OST_Windows, BuiltInCategory.OST_Doors, BuiltInCategory.OST_MEPSpaces, BuiltInCategory.OST_Rooms });
            var geoElements = wfic.WherePasses(geoElementsFilter).WhereElementIsNotElementType().ToElements();


            using (Transaction tx = new Transaction(_rdoc, "Build graph"))
            {
                tx.Start("Build graph");
                mps.Write(mecElements, geoElements, -1, true, _rdoc);
                tx.Commit();
            }

            MEPGraphWriter wre = new MEPGraphWriter(client);
            wre.Write(meGraph, _rdoc);
            //var bc = new HLApps.Cloud.BuildingGraph.BuildingGraphClient(@"http://localhost:4001/graphql");
            //MEPGraphWriterGraphQL gqlWrtier = new MEPGraphWriterGraphQL(bc);

            //var json = System.IO.File.ReadAllText(@"C:\src\HLApps\GraphData-MEP\HLApps.Revit.Graph\RevitToGraphQLMappings.json");
            //var bgmap = new HLApps.Cloud.BuildingGraph.Introspection.BuildingGraphMapping(json);

            //gqlWrtier.Write(meGraph, bgmap, _rdoc);

        }
    }
}
