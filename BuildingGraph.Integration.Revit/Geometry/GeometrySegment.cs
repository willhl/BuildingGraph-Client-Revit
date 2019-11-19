using Autodesk.Revit.DB;


namespace HLApps.Revit.Geometry
{

    public abstract class GeometrySegment
    {
 
        public HLBoundingBoxXYZ Bounds { get; set; }

        public string OriginatingDocIdent { get; set; }
        public ElementId OriginatingElement { get; set; }
        public ElementId OriginatingElementCategory { get; set; }

        public bool Removed { get; set; }

        protected string segId = string.Empty;
        public string SegmentId
        {
            get
            {
                return segId.ToString();
            }
        }
        public virtual void Invalidate()
        {
            Removed = true;
        }
    }

}