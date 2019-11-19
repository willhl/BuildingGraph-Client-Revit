using System;
using HLApps.Revit.Utils;
using Autodesk.Revit.DB;


namespace HLApps.Revit.Geometry
{
    public class SolidGeometrySegment : GeometrySegment
    {
        public SolidGeometrySegment(Solid geo, Element element, HLBoundingBoxXYZ bounds)
        {
            Geometry = geo;
            OriginatingElement = element.Id;
            OriginatingDocIdent = DocUtils.GetDocumentIdent(element.Document);
            if (element.Category != null) OriginatingElementCategory = element.Category.Id;

            Bounds = bounds;

            Removed = false;
            segId = Guid.NewGuid().ToString();
        }

        public Solid Geometry { get; set; }
        public override void Invalidate()
        {
            Geometry = null;
            Removed = true;
        }
    }

}