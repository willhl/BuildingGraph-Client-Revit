using System;
using HLApps.Revit.Utils;
using Autodesk.Revit.DB;


namespace HLApps.Revit.Geometry
{
    public class CurveGeometrySegment : GeometrySegment
    {
        public CurveGeometrySegment(Curve curve, Element element)
        {
            Geometry = curve;

            OriginatingElement = element.Id;
            OriginatingDocIdent = DocUtils.GetDocumentIdent(element.Document);
            if (element.Category != null) OriginatingElementCategory = element.Category.Id;

            var min = curve.GetEndPoint(0);
            var max = curve.GetEndPoint(1);
            MidPoint = (min + max) / 2;
            Bounds = new HLBoundingBoxXYZ(min,  max, false);
            Removed = false;
            segId = Guid.NewGuid().ToString();

            if (element is MEPCurve)
            {
                var dParam = element.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM);
                if (dParam != null)
                {
                    Radius = dParam.AsDouble() * 0.5;
                }
                else
                {
                    var wParam = element.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM);
                    if (wParam != null) Width = wParam.AsDouble();
                    var hParam = element.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM);
                    if (hParam != null) Height = hParam.AsDouble();
                }

            }
        }

        public double MaxDimension
        {
            get
            {
                if (Radius > Height) return Radius;
                if (Radius > Width) return Radius;
                if (Height > Width) return Height;
                return Width;
            }
        }

        public Curve Geometry { get; set; }

        public override void Invalidate()
        {
            Geometry = null;
            Removed = true;
        }

        public double Width { private set; get; }
        public double Height { private set; get; }
        public double Radius { private set; get; }

        public XYZ MidPoint { get; private set; }
    }

}