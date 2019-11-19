using System.Collections.Generic;
using HLApps.Revit.Geometry.Octree;
using HLApps.Revit.Geometry;
using BuildingGraph.Integrations.Revit.Parsers;

namespace BuildingGraph.Integrations.Revit
{

    public class MEPPathWriteCache
    {
        public BoundsOctree<GeometrySegment> geoCache;
        public BoundsOctreeElementWriter geoCacheWriter;
        public PointOctree<ConnectorPointGeometrySegment> connectorsCache;
        public PointOctree<FaceIntersectRay> rayhitCache;
        public HashSet<int> ParsedElements;
        public double MaxDepth;
    }
}
    
