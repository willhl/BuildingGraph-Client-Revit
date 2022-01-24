using System.Collections.Generic;
using HLApps.Revit.Geometry.Octree;
using HLApps.Revit.Geometry;
using BuildingGraph.Integration.Revit.Parsers;

namespace BuildingGraph.Integration.Revit
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
    
