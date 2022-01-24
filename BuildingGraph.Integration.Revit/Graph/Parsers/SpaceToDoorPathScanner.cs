
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using HLApps.Revit.Geometry;
using HLApps.Revit.Geometry.Octree;
using HLApps.Revit.Utils;
using Model = BuildingGraph.Client.Model;

namespace BuildingGraph.Integration.Revit.Parsers
{
    public class SpaceNetworkPathScanner : IMEPGraphElementScanner
    {
        BoundsOctree<GeometrySegment> _geoTree;

        public void Initialise(ICollection<Element> geoSerchElements, BoundsOctree<GeometrySegment> geo)
        {
            _geoTree = geo;


        }


        public void ScanFromDocument(Document doc, MEPRevitGraph graph, int maxDepth)
        {
            Dictionary<int, HashSet<FamilyInstance>> spaceToDoors = new Dictionary<int, HashSet<FamilyInstance>>();

            scanFromDocument(graph, doc, Transform.Identity, doc);

            //get linked documents
            foreach (RevitLinkInstance linkedInstance in DocUtils.GetLinkInstances(doc))
            {

                //RevitLinkType linkType = (RevitLinkType)doc.GetElement(linkedInstance.GetTypeId()) as RevitLinkType;

                //ExternalFileReference exfRef = linkType.GetExternalFileReference();
                
                //if (exfRef.GetLinkedFileStatus() != LinkedFileStatus.Loaded) continue;


                var ldoc = linkedInstance.GetLinkDocument();
                if (ldoc == null) continue;
                scanFromDocument(graph, ldoc, linkedInstance.GetTotalTransform().Inverse, doc);
            }

        }

        public void ScanFromElement(Element elm, MEPRevitGraph graph, int maxDepth)
        {
            //TODO: actually scan from the supplied element
            ScanFromDocument(elm.Document, graph, maxDepth);

        }

        void scanFromDocument(MEPRevitGraph graph, Document doc, Transform tr, Document spaceSourceDoc)
        {
            var doorCol = new FilteredElementCollector(doc);
            var doors = doorCol.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToElements().OfType<FamilyInstance>();

            foreach (var door in doors)
            {
                if (door.Location == null) continue;

                var ho = door.FacingFlipped ? door.FacingOrientation.Negate() : door.FacingOrientation;
                //var bbox = door.get_BoundingBox(null);
                var midPoint = (door.Location as LocationPoint).Point + new XYZ(0, 0, 1);
                //if (bbox != null)
                //{
                //    midPoint = (bbox.Min + bbox.Max) / 2;
                //}

                //get point in front
                var hfOrt = ho.Normalize();
                var frontPos = midPoint + hfOrt.Multiply(1.64042); //stub is 1.64042 feet (0.5 meters)
                var lpFrontPoint = tr.OfPoint(frontPos);
                var spFront = spaceSourceDoc.GetSpaceAtPoint(lpFrontPoint);//, phase);


                //get point behind
                var hbOrt = ho.Negate().Normalize();
                var behindPos = midPoint + hbOrt.Multiply(1.64042); //stub is 1.64042 feet (0.5 meters)
                var lpBehindPoint = tr.OfPoint(behindPos);
                var spBehind = spaceSourceDoc.GetSpaceAtPoint(lpBehindPoint);//, phase);

                

                if (spFront != null)
                {
                    var edges = graph.AddConnection(spFront, door, midPoint, MEPPathConnectionType.Phyiscal, Model.MEPEdgeTypes.FLOWS_TO);
                    edges.ThisNode.OrginTransform = tr;
                }

                if (spBehind != null)
                {
                    var edges = graph.AddConnection(door, spBehind, midPoint, MEPPathConnectionType.Phyiscal, Model.MEPEdgeTypes.FLOWS_TO);
                    edges.ThisNode.OrginTransform = tr;
                }


            }
        }



    }

}

