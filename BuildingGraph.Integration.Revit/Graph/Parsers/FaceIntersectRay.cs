using Autodesk.Revit.DB;

using HLApps.Revit.Geometry;

namespace BuildingGraph.Integration.Revit.Parsers
{
    public class FaceIntersectRay
    {
        public string IntermediatDocIdent;
        public ElementId IntermeidateElement;
        public SolidGeometrySegment HittingSegment;
        public Element SourceElement;
        public Face SourceFace;
        public Face HittingFace;
        public XYZ RayVecotor;
        public UV SourceUV;
        public UV HittingUV;
        public XYZ SourceXYZ;
        public XYZ HittingXYZ;
        public SubfaceType SubFaceType;
        public double AreaWeight;
        public double Distance;
        public bool Ignore;
    }

}
