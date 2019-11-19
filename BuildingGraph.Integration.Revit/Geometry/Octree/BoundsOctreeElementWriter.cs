using System.Collections.Generic;
using Autodesk.Revit.DB;
using HLApps.Revit.Utils;

namespace HLApps.Revit.Geometry.Octree
{

    public class BoundsOctreeElementWriter
    {

        BoundsOctree<GeometrySegment> _boundsStree;
        Dictionary<int, Options> _geoOptionsOverrides;
        Options defaultGeomOptions;

        public BoundsOctreeElementWriter(BoundsOctree<GeometrySegment> boundsStree)
        {
            _geoOptionsOverrides = new Dictionary<int, Options>();
            _boundsStree = boundsStree;
            var fineGeoOp = new Options();
            fineGeoOp.DetailLevel = ViewDetailLevel.Fine;

            var courseGeomOptions = new Options();
            courseGeomOptions.DetailLevel = ViewDetailLevel.Coarse;


            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_PipeCurves, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_PipeSegments, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_PipeFitting, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_PipeAccessory, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_FlexPipeCurves, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_FlexDuctCurves, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_Windows, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_Doors, courseGeomOptions);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_Roofs, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_Floors, fineGeoOp);
            _geoOptionsOverrides.Add((int)BuiltInCategory.OST_Ceilings, fineGeoOp);


            defaultGeomOptions = new Options();
            defaultGeomOptions.DetailLevel = ViewDetailLevel.Medium;

        }

        Dictionary<int, HashSet<GeometrySegment>> SegmentCache = new Dictionary<int, HashSet<GeometrySegment>>();

        public void AddElement(ICollection<Element> elements, bool includeGeometry)
        {
            foreach (var elm in elements)
            {
                AddElement(elm, includeGeometry);
            }
        }

        public void AddElement(Element elm, bool includeGeometry, Transform tx = null)
        {
            if (_boundsStree == null || elm == null)
            {
                return;
            }
                

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

            if (includeGeometry || elm is FamilyInstance)
            {
                var gop = defaultGeomOptions;
                if (elm.Category != null && _geoOptionsOverrides.ContainsKey(elm.Category.Id.IntegerValue))
                {
                    gop = _geoOptionsOverrides[elm.Category.Id.IntegerValue];
                }

                var egeo = elm.get_Geometry(gop);
                if (egeo == null) return;

                if (tx != null) egeo = egeo.GetTransformed(tx);

                var allSolids = GeoUtils.GetAllSolidsInGeometry(egeo);
                foreach (var sld in allSolids)
                {
                    if (!solidIsValid(sld))
                    {
                        continue;
                    }

                    var hlbbox = GeoUtils.GetGeoBoundingBox(sld, Transform.Identity);
                    if (hlbbox == null) continue;
                    var geoSegment = new SolidGeometrySegment(sld, elm, hlbbox);
                    segList.Add(geoSegment);
                    _boundsStree.Add(geoSegment, hlbbox);
                }
            }

            if (elm.Location is LocationCurve)
            {
                var curve = elm.Location as LocationCurve;
                if (curve == null) return;
                var acCurve = curve.Curve;

                if (tx != null) acCurve = acCurve.CreateTransformed(tx);

                var cseg = new CurveGeometrySegment(acCurve, elm);
                segList.Add(cseg);
                _boundsStree.Add(cseg, cseg.Bounds);
            }


            if (!SegmentCache.ContainsKey(elmid))
            {
                SegmentCache.Add(elmid, segList);

            }



        }


        public BoundsOctree<GeometrySegment> OcTree
        {
            get
            {
                return _boundsStree;
            }
        }



        double _minVolume = 0.1;
        private bool solidIsValid(Solid sld)
        {
            return sld != null && sld.Volume > _minVolume;
        }

    }
}
