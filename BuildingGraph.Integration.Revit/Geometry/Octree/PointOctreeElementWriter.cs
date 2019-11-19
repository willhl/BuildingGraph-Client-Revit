using System.Collections.Generic;

using Autodesk.Revit.DB;


namespace HLApps.Revit.Geometry.Octree
{
    public class PointOctreeElementWriter
    {
        PointOctree<GeometrySegment> _pointStree;
        Dictionary<int, HashSet<GeometrySegment>> SegmentCache = new Dictionary<int, HashSet<GeometrySegment>>();


        public PointOctreeElementWriter(PointOctree<GeometrySegment> pointStree)
        {
            _pointStree = pointStree;
        }

        public void AddElement(ICollection<Element> elements)
        {
            foreach (var elm in elements)
            {
                AddElement(elm);
            }
        }

        public void AddElement(Element elm)
        {

            if (_pointStree == null) return;

            var elmid = elm.Id.IntegerValue;

            HashSet<GeometrySegment> segList;

            if (SegmentCache.ContainsKey(elmid))
            {
                segList = SegmentCache[elmid];
            }
            else
            {
                segList = new HashSet<GeometrySegment>();
            }

            if (elm.Location is LocationPoint)
            {
                var lpoint = elm.Location as LocationPoint;
                PointGeometrySegment psg = null;
                if (elm is FamilyInstance)
                {
                    var fi = elm as FamilyInstance;
                    psg = new PointGeometrySegment(elm.Id, lpoint.Point, fi.FacingOrientation, fi.HandOrientation, fi.FacingFlipped, fi.HandFlipped);
                }
                else
                {
                    psg = new PointGeometrySegment(elm.Id, lpoint.Point);
                }

                _pointStree.Add(psg, lpoint.Point);
            }
            else if (elm.Location is LocationCurve)
            {
                var curve = elm.Location as LocationCurve;
                if (curve == null) return;

                var cseg = new CurveGeometrySegment(curve.Curve, elm);
                segList.Add(cseg);
                _pointStree.Add(cseg, cseg.MidPoint);
            }


            if (!SegmentCache.ContainsKey(elmid))
            {
                SegmentCache.Add(elmid, segList);
            }


        }

        public PointOctree<GeometrySegment> OcTree
        {
            get
            {
                return _pointStree;
            }
        }

    }
}
