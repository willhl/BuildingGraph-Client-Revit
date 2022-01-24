using System.Collections.Generic;
using Autodesk.Revit.DB;
using HLApps.Revit.Geometry;
using HLApps.Revit.Geometry.Octree;

namespace BuildingGraph.Integration.Revit.Parsers
{
    public interface IMEPGraphElementScanner
    {
        void Initialise(ICollection<Element> geoSerchElements, BoundsOctree<GeometrySegment> geo);
        void ScanFromDocument(Document doc, MEPRevitGraph graph, int maxDepth);
        void ScanFromElement(Element elm, MEPRevitGraph graph, int maxDepth);
    }
}