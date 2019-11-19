using System;
using Autodesk.Revit.DB;


namespace HLApps.Revit.Geometry
{
    public class PointGeometrySegment : GeometrySegment
    {
        public PointGeometrySegment(ElementId element, XYZ point)
        {
            OriginatingElement = element;
            Point = point;
            Removed = false;
            segId = Guid.NewGuid().ToString();
        }

        public PointGeometrySegment(ElementId element, XYZ point, XYZ facing, XYZ hand, bool fflip, bool hflip) : this(element, point)
        {
            Facing = facing;
            Hand = hand;
            FacingFlipped = fflip;
            HandFlipped = hflip;
        }

        public XYZ Point { get; set; }
        public XYZ Facing { get; set; }
        public XYZ Hand { get; set; }
        public bool FacingFlipped { get; private set; }
        public bool HandFlipped { get; private set; }
    }

}