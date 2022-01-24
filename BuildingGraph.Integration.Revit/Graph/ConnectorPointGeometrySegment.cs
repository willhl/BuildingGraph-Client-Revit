
using Autodesk.Revit.DB;
using HLApps.Revit.Geometry;

namespace BuildingGraph.Integration.Revit
{

    public class ConnectorPointGeometrySegment : PointGeometrySegment
    {
        public ConnectorPointGeometrySegment(ElementId element, XYZ point, int Connectorindex) : base(element, point)
        {
            ConnectorIndex = Connectorindex;
        }

        public int ConnectorIndex { get; set; }
    }
}
