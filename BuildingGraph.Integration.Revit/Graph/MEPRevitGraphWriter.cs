using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using HLApps.Revit.Geometry;
using HLApps.Revit.Geometry.Octree;
using HLApps.Revit.Utils;
using BuildingGraph.Integration.Revit.Parsers;

namespace BuildingGraph.Integration.Revit
{
    public class MEPRevitGraphWriter
    {
        MEPRevitGraph graph = new MEPRevitGraph();

        public MEPRevitGraphWriter(MEPRevitGraph graph)
        {
            Cache = new MEPPathWriteCache();
            Graph = graph;
            Parsers = new List<IRevitGraphParser>();

        }

        public List<IRevitGraphParser> Parsers { get; private set; }
        public MEPPathWriteCache Cache { get; private set; }
 
        public MEPRevitGraph Graph { get; private set; }

        public ElementFilter GetFilterForAllParsers
        {
            get
            {
                var orFlre = new LogicalOrFilter(Parsers.Select(p => p.GetFilter()).ToList());
                return orFlre;
            }
        }

        public void Write(ICollection<Element> scanElements, ICollection<Element> geoSerchElements, int maxDepth, bool doGeometryMatch, Document doc)
        {

            Cache.connectorsCache = new PointOctree<ConnectorPointGeometrySegment>(6.56168F, XYZ.Zero, 2F);
            Cache.MaxDepth = maxDepth;
            Cache.geoCache = new BoundsOctree<GeometrySegment>(2, XYZ.Zero, 2F, 1.001f);
            Cache.geoCacheWriter = new BoundsOctreeElementWriter(Cache.geoCache);
            Cache.ParsedElements = new HashSet<int>();
            Cache.rayhitCache = new PointOctree<FaceIntersectRay>(2, XYZ.Zero, 2F);

            var dsCount = geoSerchElements.Count;

            foreach (var elm in geoSerchElements)
            {
                var cmn = MEPUtils.GetConnectionManager(elm);
                if (cmn != null)
                {
 
                    foreach (var conn in cmn.Connectors.OfType<Connector>())
                    {
#if REVIT2016
                        var gesseg = new ConnectorPointGeometrySegment(elm.Id, conn.Origin, conn.Id);
                        Cache.connectorsCache.Add(gesseg, conn.Origin);
#else
                        throw new Exception("Only supported in Revit 2016 onwards");
#endif
                    }


                }

                if (doGeometryMatch) Cache.geoCacheWriter.AddElement(elm, true);
            }

            if (doGeometryMatch)
            {
                var li = DocUtils.GetLinkInstances(doc);
                dsCount = li.Count();
                //cache geometry in linked documents. The geometry is expected to remain static, or changed as a block, so we don't need to keep track of each element.
                foreach (var linkedInstance in li)
                {
                    Transform docTransform = linkedInstance.GetTotalTransform();
                    Transform docTransformInverse = docTransform.Inverse;

                    //RevitLinkType linkType = (RevitLinkType)rDoc.GetElement(linkedInstance.GetTypeId()) as RevitLinkType;
                    var ldoc = linkedInstance.GetLinkDocument();// HLRevitUtilities.GetLinkedDocumentFromType(_rdoc.Application, linkType);
                    if (ldoc == null) continue;

                    var lnGeoCol = new FilteredElementCollector(ldoc);
                    var lnGeoFilter = new ElementMulticategoryFilter(new BuiltInCategory[] { BuiltInCategory.OST_Floors, BuiltInCategory.OST_Roofs });

                    foreach (var lnelm in lnGeoCol.WherePasses(lnGeoFilter).WhereElementIsNotElementType().ToElements())
                    {
                        Cache.geoCacheWriter.AddElement(lnelm, true, docTransformInverse);
                    }
                }
            }


            var elmcache = new HashSet<int>();
            var parserCount = Parsers.Count;

            foreach (var parser in Parsers)
            { 
                parser.InitializeGraph(this);
                var elms = scanElements.Where(el => parser.CanParse(el)).ToList();
                dsCount = elms.Count();

                foreach (var elm in elms)
                {
                    parser.ParseFrom(elm, this);
                }
                parser.FinalizeGraph(this);
            }
        
        }


    }
}
    
