using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

using HLApps.Revit.Utils;
using HLApps.Revit.Geometry;
using HLApps.Revit.Geometry.Octree;
using Model = BuildingGraph.Client.Model;
using BuildingGraph.Integration.Revit.Geometry;

namespace BuildingGraph.Integrations.Revit.Parsers
{


    public class MEPGraphParserSpaces : IRevitGraphParser
    {

        public ElementFilter GetFilter()
        {

            var cats = new BuiltInCategory[] { BuiltInCategory.OST_MEPSpaces, BuiltInCategory.OST_Rooms };
            var emcf = new ElementMulticategoryFilter(cats);
            return emcf;

        }

        public bool CanParse(Element elm)
        {
            return elm is SpatialElement;
        }

        public void ParseFrom(Element elm, MEPRevitGraphWriter writer)
        {

            var space = elm as Autodesk.Revit.DB.Mechanical.Space;
            if (space == null) return;

            if (space.Volume < 0.5) return;

            var scannedElements = writer.Cache.ParsedElements;
            var cpTree = writer.Cache.connectorsCache;
            var geoTree = writer.Cache.geoCache;
            var maxDepth = writer.Cache.MaxDepth;
            var graph = writer.Graph;

            var lvl = space.Level;
            if (lvl != null)
            {
                graph.AddConnection(elm, lvl, MEPPathConnectionType.Proximity, Model.MEPEdgeTypes.IS_ON);
            }

            //get elements in the space


            //get areas with edges and add to walls

            var sbopt = new SpatialElementBoundaryOptions();
            sbopt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;
            sbopt.StoreFreeBoundaryFaces = true;

            //var geoOpt = new Options();
            //var spgeo = space.get_Geometry(geoOpt);

            //get nearby spaces by extending bb of space
            //foreach each face of space geometry
            //split into uv gripd
            //construct line from each point
            //get intersect with faces on nearby faces
            //if it hits increment area count for direction

            var doc = space.Document;
            SpatialElementGeometryCalculator sg = new SpatialElementGeometryCalculator(doc, sbopt);
            var spgets = sg.CalculateSpatialElementGeometry(space);
            var spgeo = spgets.GetGeometry();
            //var spbb = spgeo.GetBoundingBox();
            var spbb = elm.get_BoundingBox(null);

            //var spbbhl = new HLBoundingBoxXYZ(spbb);
            //spbbhl.Size = spbbhl.Size + new XYZ(2, 2, 4);
            

            //get faces
            var uvdensity = 0.75;
            var maxDistance = 4D;
            //var nearbyFaces = nearbySpaces.OfType<SolidGeometrySegment>().Where(spc => spc.OriginatingElement != space.Id).SelectMany(sp => sp.Geometry.Faces.OfType<Face>());

            var rayIncidents = new HashSet<FaceIntersectRay>();


            //get all the faces in the geometry
            var spfaces = spgeo.Faces.OfType<Face>().ToList();
            foreach (var gface in spfaces)
            {

                //extract the faces which bound with other elements (Walls, floors, ceilings, windows etc)
                var sfaceInfos = spgets.GetBoundaryFaceInfo(gface);
                foreach (var sfaceInfo in sfaceInfos)
                {
                    //get the geo face and element of this bounding face
                    var sface = sfaceInfo.GetSubface();

                    var elmId = sfaceInfo.SpatialBoundaryElement;

                    var lelm = GetElementFromLinkedElement(elmId, doc);
                    var docIdent = string.Empty;
                    //if (lelm == null) continue; //ignore this face if it doesn't resolve to a valid element

                    //find the bounding uv box so we can work out a grid of points 
                    var fbb = sface.GetBoundingBox();
                    var uExt = uvdensity;// (fbb.Max.U - fbb.Min.U) / uvdensity;
                    var vExt = uvdensity;// (fbb.Max.V - fbb.Min.V) / uvdensity;

                    var u = fbb.Min.U;
                    var v = fbb.Min.V;

                    //var sb = new GeoLib.C2DPolygon();

                    //construct grid for ray tracing
                    Stack<UV> gridPoints = new Stack<UV>();
                    while (u <= fbb.Max.U)
                    {
                        v = fbb.Min.V;
                        while (v <= fbb.Max.V)
                        {
                            var uvp = new UV(u, v);
                            v += uvdensity;
                            if (!sface.IsInside(uvp)) continue; //only include points that are actually on this face
                            gridPoints.Push(uvp);
                        }
                        u += uvdensity;
                    }

                    var nerbyCheckCats = new int[] { };
                    IList<ElementId> hostElms = new List<ElementId>();

                    if (lelm != null && !(lelm is HostObject))
                    {
                        var n = lelm;
                    }

                    if (lelm != null)
                    {
                        docIdent = DocUtils.GetDocumentIdent(lelm.Document);
                        //get cutting elemtns if it's a wall so we can find door and windows
                        if (lelm is HostObject)
                        {
                            var whost = lelm as HostObject;
                            hostElms = whost.FindInserts(true, true, true, true);
                            //build oct tree of hostelems so we can quickly ray trace them later
                            foreach (var hostElm in hostElms)
                            {
                                //ignoring any link and transform for now
                                var ehl = whost.Document.GetElement(hostElm);
                                writer.Cache.geoCacheWriter.AddElement(ehl, true);
                            }
                        }

                        //we need the nearby check to find the cut out elements
                        nerbyCheckCats = new int[] { (int)BuiltInCategory.OST_Doors, (int)BuiltInCategory.OST_Windows };
                    }
                    else
                    {

                        nerbyCheckCats = new int[] { (int)BuiltInCategory.OST_Doors, (int)BuiltInCategory.OST_Windows, (int)BuiltInCategory.OST_Floors, (int)BuiltInCategory.OST_Walls, (int)BuiltInCategory.OST_Roofs };

                        //we need the nearby check to find the bounding element
                        switch (sfaceInfo.SubfaceType)
                        {
                            case SubfaceType.Bottom:
                                nerbyCheckCats = new int[] { (int)BuiltInCategory.OST_Floors};
                                break;

                            case SubfaceType.Top:
                                nerbyCheckCats = new int[] { (int)BuiltInCategory.OST_Roofs, (int)BuiltInCategory.OST_Floors, (int)BuiltInCategory.OST_Windows };
                                break;

                            case SubfaceType.Side:
                                nerbyCheckCats = new int[] { (int)BuiltInCategory.OST_Doors, (int)BuiltInCategory.OST_Windows, (int)BuiltInCategory.OST_Walls };
                                break;
                        }

                    }

                    //option 1 - brute force ray trace
                    //option 2 - construct 2d polygon from edges of each face, translate each face into the same plane, then boolean intersect, get area of each intersect
                    //calc space boundaries at midpoint?
                    Face optLastIntermeidateFace = null;
                    SolidGeometrySegment optLastIntermeidateSegment = null;
                    Face optLastHitFace = null;
                    SolidGeometrySegment optLastHitSegment = null;
                    var arWeight = sface.Area / gridPoints.Count;
                    while (gridPoints.Count > 0)
                    {
                        var pt = gridPoints.Pop();
                        var rayIncident = new FaceIntersectRay();
                        
                        var nv = sface.ComputeNormal(pt).Normalize();
                        //var mx = sface.ComputeSecondDerivatives(pt).MixedDerivative.Normalize();
                        rayIncident.SourceElement = space;
                        rayIncident.SourceFace = sface;
                        rayIncident.SourceUV = pt;

                        rayIncident.RayVecotor = nv;
                        rayIncident.IntermediatDocIdent = docIdent;
                        rayIncident.IntermeidateElement = lelm != null ? lelm.Id : ElementId.InvalidElementId;
                        rayIncident.AreaWeight = arWeight;
                        rayIncident.SubFaceType = sfaceInfo.SubfaceType;
                        rayIncidents.Add(rayIncident);

                        var sp = sface.Evaluate(pt);
                        rayIncident.SourceXYZ = sp;
                        var ray = Line.CreateBound(sp, sp + nv * 4);
                        var rayBB = new HLBoundingBoxXYZ(sp, (sp + nv * 4), true);
                        //rayBB.Size = rayBB.Size + new XYZ(0.5, 0.5, 0.5);
                        //Plane geoPlane = Plane.c(sp, sp + nv * 5, sp + nv * 5 + (mx * 0.2));
                        //SketchPlane skPlane = SketchPlane.Create(doc, geoPlane);
                        //doc.Create.NewModelCurve(ray, skPlane);

                        //check cache for hit on otherside, if there is one nearby on this face we can ignore it as we're not including both sides
                        //var nearbyrayHits = writer.Cache.rayhitCache.GetNearby(ray, 0.4F);
                        //var validSimilarHit = nearbyrayHits.FirstOrDefault(rh => rh.HittingSegment != null && rh.HittingSegment.OriginatingElement == space.Id && rh.IntermeidateElement == rayIncident.IntermeidateElement);
                        //if (validSimilarHit != null && sface.IsInside(validSimilarHit.HittingUV))
                        //{
                        //    rayIncident.Ignore = true;
                        //    log.Info("Got hit on other side, ignoring");
                        //    continue;
                       // }

                        if (optLastIntermeidateFace != null)
                        {
                            IntersectionResultArray issRes = null;
                            var issGeoHit = getIntersect(pt, optLastIntermeidateFace, sface, maxDistance, out issRes, out double distance, nv, doc);
                            if (issGeoHit)
                            {
                                rayIncident.IntermediatDocIdent = optLastIntermeidateSegment.OriginatingDocIdent;
                                rayIncident.IntermeidateElement = optLastIntermeidateSegment.OriginatingElement;
                            }
                            else
                            {
                                optLastIntermeidateFace = null;
                                optLastIntermeidateSegment = null;

                            }
                        }

                        GeometrySegment[] nearbyElements = null;

                        if (optLastIntermeidateFace == null)
                        {
                            nearbyElements = geoTree.GetColliding(rayBB);

                            var nearbyCutoutElements = nearbyElements.Where(iel =>
                            (hostElms.Count == 0 || hostElms.Contains(iel.OriginatingElement))
                            && nerbyCheckCats.Contains(iel.OriginatingElementCategory.IntegerValue))
                            .OfType<SolidGeometrySegment>();


                            IntersectionResultArray isRes = null;
                            bool isGeoHit = false;
                            foreach (var extSegment in nearbyCutoutElements)
                            {
                                foreach (var extFace in extSegment.Geometry.Faces.OfType<Face>())
                                {
                                    isGeoHit = getIntersect(pt, extFace, sface, maxDistance, out isRes, out double distance, nv, doc);
                                    if (isGeoHit)
                                    {
                                        rayIncident.IntermediatDocIdent = extSegment.OriginatingDocIdent;
                                        rayIncident.IntermeidateElement = extSegment.OriginatingElement;
                                        optLastIntermeidateFace = extFace;
                                        optLastIntermeidateSegment = extSegment;
                                        break;
                                    }
                                }
                                if (isGeoHit) break;
                            }
                        }



                        if (optLastHitFace != null)
                        {
                            var isHit = getIntersect(pt, optLastHitFace, sface, maxDistance, out var isRe, out double distance, nv, doc);
                            var isRes = isRe;
                            //project point onto other face instead?
                            var srcXYZ = sface.Evaluate(pt);
                            var otXYZRes = optLastHitFace.Project(srcXYZ);

                            if (isHit)
                            {
                                var itx = isRes.OfType<IntersectionResult>().FirstOrDefault();
                                rayIncident.HittingFace = optLastHitFace;
                                rayIncident.HittingSegment = optLastHitSegment;
                                rayIncident.HittingUV = itx.UVPoint;
                                rayIncident.HittingXYZ = itx.XYZPoint;
                                rayIncident.Distance = distance;
                                nv = sface.ComputeNormal(pt).Normalize();
                                continue; //shortcut if we find a hit on the same face again
                            }
                            else
                            {
                                optLastHitFace = null;
                                optLastHitSegment = null;
                            }
                        }


                        if (nearbyElements == null)
                        {
                            nearbyElements = geoTree.GetColliding(rayBB, (ob) => { return ob.OriginatingElementCategory.IntegerValue == (int)BuiltInCategory.OST_MEPSpaces; });
                        }

                        //BoundingBoxIntersectsFilter bif = new BoundingBoxIntersectsFilter(new Outline(sp - new XYZ(0.1, 0.1, 0.1), (sp + nv * 2) + new XYZ(0.1, 0.1, 0.1)));
                        //var sfv = new FilteredElementCollector(space.Document);
                        //var sepl = sfv.WherePasses(bif).ToElements();

                        var nearbySpaces = nearbyElements.Where(ne => ne.OriginatingElementCategory.IntegerValue == (int)BuiltInCategory.OST_MEPSpaces).OfType<SolidGeometrySegment>().Distinct().ToList();

                        //find the extents of this face which face faces on other nearby faces (whaaat?)       
                        //check each face of each nearby space for intersect with ray
                        foreach (var nearSpace in nearbySpaces)
                        {
                            var isHit = false;
                            foreach (var otFace in nearSpace.Geometry.Faces.OfType<Face>())
                            {
                                isHit = getIntersect(pt, otFace, sface, maxDistance, out var isRe, out double distance, nv, doc);
                                var isRes = isRe;
                                //project point onto other face instead?
                                var srcXYZ = sface.Evaluate(pt);
                                var otXYZRes = otFace.Project(srcXYZ);

                                if (isHit)
                                {
                                    if (nearSpace.OriginatingElement.IntegerValue != elm.Id.IntegerValue)
                                    {
                                       
                                        var itx = isRes.OfType<IntersectionResult>().FirstOrDefault();
                                        rayIncident.HittingFace = otFace;
                                        rayIncident.HittingSegment = nearSpace;
                                        rayIncident.HittingUV = itx.UVPoint;
                                        rayIncident.HittingXYZ = itx.XYZPoint;
                                        rayIncident.Distance = distance;

                                        nv = sface.ComputeNormal(pt).Normalize();

                                        //optimization: check this face again first for the next ray check, since it's likely to be another hit
                                        optLastHitFace = otFace;
                                        optLastHitSegment = nearSpace;

                                        rayIncident.Ignore = false;
                                    }
                                    else
                                    {
                                        if (distance < 0.1) isHit = false; //looks like we hit our own face, ouch!
                                        rayIncident.Ignore = true;
                                    }

                                    break;
                                }

                            }
                            if (isHit) break;
                        }

                    }
                }

                /*
                space
                   face
                      intermediate element (Wall/window/door)
                          face (space)
                   face
                

                add connection
                this space -> section -> other space
                wall ------------^
                */
            }

            //var ec = new elmComparer();
            var srcNode = graph.AddElement(space);
            double minIncluedArea = 4;
            VectorBucketiser vbw = new VectorBucketiser(8, 5);

            var includeRays = rayIncidents.Where(r => !r.Ignore);

            var outsideNode = graph.Nodes.FirstOrDefault(n => n.Name == "Outside");
            var groundNode = graph.Nodes.FirstOrDefault(n => n.Name == "Ground");

            //group by the intermediate element (wall/floor/etc)
            foreach (var docGroup in includeRays.GroupBy(ri => ri.IntermediatDocIdent))//, ec))
            {
                var sdoc = DocUtils.GetDocument(docGroup.Key, elm.Document.Application);

                foreach (var intermediateElemGroup in docGroup.GroupBy(ri => ri.IntermeidateElement.IntegerValue))//, ec))
                {
                    var selmid = new ElementId(intermediateElemGroup.Key);
                    Element selm = sdoc != null ? sdoc.GetElement(selmid) : null;
                    //group similar vectors into buckets
                    foreach (var rayVectorBuckets in intermediateElemGroup.GroupBy(ri => vbw.GetBucket(ri.RayVecotor)))
                    {
                        var rg = rayVectorBuckets.ToList();

                        var gs = rg.GroupBy(vr => vr.HittingSegment == null ? null : vr.HittingSegment.OriginatingElement).ToList();
                        //group each vector and intermediate element by the element it hits
                        foreach (var orgElmGroup in gs)
                        {
                            //find a section already matching this section
                            //actually easier to treat each path as separate sections
                            //var edNodes = graph.GetEdges(intermediateElemGroup.Key).Where(ed => ed.NextNode.AsAbstractNode.Name == "Surface" 
                            //&& ed.NextNode.Connections.Any(cn => cn.NextNode.OriginId == spNode.OriginId)).Where(ed => ed.;

                            var apporxIntersect = orgElmGroup.Sum(et => et.AreaWeight);
                            var vector = orgElmGroup.First().RayVecotor;
                            if (apporxIntersect < minIncluedArea) continue;

                            var direction = VectorBucketiser.GetZeroClamppedPoint(orgElmGroup.First().RayVecotor); //should be rayVectorGroup.Key.AverageVector, but not yet implemented;

                            MEPRevitNode spNode = null;
                            if (orgElmGroup.Key != null)
                            {
                                var otherSpace = doc.GetElement(orgElmGroup.Key);
                                spNode = graph.AddElement(otherSpace);
                            }
                            else
                            {
                                if (selm != null && (selm.Name.ToLower().Contains("exterior") || selm is Autodesk.Revit.DB.Opening || selm.Name.ToLower().Contains("window") || selm is Autodesk.Revit.DB.RoofBase))
                                {
                                    if (outsideNode == null) outsideNode = new MEPRevitNode("Outside", "External", "Outside", new Model.Environment());
                                    spNode = outsideNode;
                                }
                                else if (selm != null && (selm.Name.ToLower().Contains("floor") || selm is Autodesk.Revit.DB.Floor))
                                {

                                    if (groundNode == null) groundNode = new MEPRevitNode("Ground", "External", "Ground", new Model.Environment());
                                    spNode = groundNode;
                                }
                                else
                                {
                                    spNode = new MEPRevitNode("Void", "External", "Void", new Model.VoidVolume());
                                    continue; //ignore void boundaries for now
                                }
                            }                            


                            var sectionN = graph.NewSection(selm, Model.MEPEdgeTypes.IS_ON);
                            sectionN.AsAbstractNode.Label = "Surface";
                            sectionN.AsElementNode.Label = "Surface";
                            

                            if (selm == null)
                            {
                                var emptyBondary = new MEPRevitNode();
                                emptyBondary.AsAbstractNode.Name = "OpenBoundary";
                                var cl = graph.AddConnection(emptyBondary, sectionN, MEPPathConnectionType.SectionOf, Model.MEPEdgeTypes.IS_ON);
                                cl.AsNodeEdge.ExtendedProperties.Add("rvid", intermediateElemGroup.Key);
                            }

                       
                            var edgesf = graph.AddConnection(srcNode, sectionN, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.BOUNDED_BY);
                            
                            //total up intersecting area
                            var sampleIntersect = orgElmGroup.First();
                            
                            edgesf.SetWeight("Area", apporxIntersect);
                            edgesf.SetWeight("Direction", new Model.Types.Point3D(direction.X, direction.Y, direction.Z));
                            edgesf.SetWeight("SubFaceType", sampleIntersect.SubFaceType.ToString());
                            
                            sectionN.AsAbstractNode.ExtendedProperties.Add("Area", apporxIntersect);
                            sectionN.AsAbstractNode.ExtendedProperties.Add("SubFaceType", sampleIntersect.SubFaceType.ToString());

                            
                            HLBoundingBoxXYZ bb = new HLBoundingBoxXYZ();
                            foreach (var et in orgElmGroup)
                            {
                                if (et.HittingXYZ != null)
                                {
                                    bb.ExpandToContain(et.HittingXYZ);
                                }
                            }

                            if (!bb.IsInvalid)
                            {
                                sectionN.BoundingBox = bb;
                                var avgCenterPoint = bb.MidPoint;
                                var size = bb.Size;
                                sectionN.SetProperty("Center", avgCenterPoint.ToBuildingGraph());
                                sectionN.SetProperty("Size", size.ToBuildingGraph());
                            }

                            

                            var edgest = graph.AddConnection(sectionN, spNode, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.BOUNDED_BY);
                            var directionn = direction.Negate();
                            edgest.SetWeight("Area", apporxIntersect);
                            edgest.SetWeight("Direction", directionn.ToBuildingGraph());
                            edgest.SetWeight("SubFaceType", (int)sampleIntersect.SubFaceType);
                        }

                    }
                }
            }


            


        }


        public void ParseFrom(ICollection<Element> elms, MEPRevitGraphWriter writer)
        {
            foreach (var elm in elms)
            {
                ParseFrom(elm, writer);
            }
        }

        private void ScanElementsFromSpaces(SpatialElement space, MEPRevitGraph graph, HashSet<int> scannedElements, BoundsOctree<GeometrySegment> geoTree)
        {



        }

        bool getIntersect(UV pt, Face otherFace, Face sourceface, double maxDistance, out IntersectionResultArray isRe, out double Distance, XYZ nv, Document doc)
        {

            var sp = sourceface.Evaluate(pt);
            var ray = Line.CreateBound(sp + nv * 0.01, sp + nv * 3);
            var ir = otherFace.Intersect(ray, out var isRes);
            isRe = isRes;
            Distance = maxDistance;

            if (isRes != null)
            {
                var itsr = isRes.OfType<IntersectionResult>().FirstOrDefault();
                if (itsr != null && ir == SetComparisonResult.Overlap)
                {

                    var dt = sp.DistanceTo(itsr.XYZPoint);
                    Distance = dt;
                    return dt < maxDistance;
                }
            }
            return false;
        }

        Element GetElementFromLinkedElement(LinkElementId leid, Document doc)
        {
            var lih = doc.GetElement(leid.HostElementId);
            if (lih != null) return lih;

            var li = doc.GetElement(leid.LinkInstanceId) as RevitLinkInstance;

            var linkedDoc = li != null ? li.GetLinkDocument() : doc;
            var elm = linkedDoc.GetElement(leid.LinkedElementId);


            return elm;
        }


        public void InitializeGraph(MEPRevitGraphWriter writer)
        {

        }

        public void FinalizeGraph(MEPRevitGraphWriter writer)
        {
            //remove duplicate paths
            

        }

    }


    class VectorBucketiser
    {
        public VectorBucketiser(int height, int width) : this (height, width, XYZ.BasisX , XYZ.BasisZ)
        {
        }

        public VectorBucketiser(int height, int width, XYZ zeroHVector, XYZ zeroWVector)
        {
            var perpCheck = zeroHVector.DotProduct(zeroWVector);
            //if (perpCheck > 0.1 || perpCheck < -0.1) throw new Exception("Zero vectors must be perpendicular");

            Buckets = new VectorBucket[height, width];
            _height = height;
            _width = width;
            _zeroHVector = zeroHVector;
            _zeroWVector = zeroWVector;

            createBuckets();
        }

        public VectorBucket[,] Buckets { get; }

        XYZ _zeroHVector;
        XYZ _zeroWVector;
        int _height;
        int _width;
        public VectorBucket GetBucket(XYZ vector)
        {
            var nv = vector.Normalize();
            nv = GetZeroClamppedPoint(nv);

            var hAngle = _zeroHVector.AngleOnPlaneTo(nv, _zeroWVector);
            var hbucket = (int)(hAngle % _height);

            var wAngle = _zeroWVector.AngleOnPlaneTo(nv, _zeroHVector);
            var wbucket = (int)(wAngle % _width);

            if (Buckets.GetLength(0) > hbucket && Buckets.GetLength(1) > wbucket)
            {
                var av = Buckets[hbucket, wbucket];
                return av;
            }

            return null;
        }

        
        void createBuckets()
        {
            var latSegmet = Math.PI * 2 / _height;
            var lngSegmet = Math.PI * 2 / _width;

            for (int h = 0; h < _height; h++)
            {
                var hl = latSegmet * h;
                var hlVector = GeoUtils.RotateAboutVector(_zeroWVector, _zeroHVector, hl);
                for (int w = 0; w < _width; w++)
                {
                    Buckets[h, w] = new VectorBucket(h, w);
                }
            }
        }

        public static XYZ GetZeroClamppedPoint(XYZ point)
        {
            var nv = new XYZ(
                point.X < 0.01 && point.X > -0.01 ? 0 : point.X,
                point.Y < 0.01 && point.Y > -0.01 ? 0 : point.Y,
                point.Z < 0.01 && point.Z > -0.01 ? 0 : point.Z);
            return nv;
        }
    }

    public class VectorBucket
    {
        public VectorBucket(int h, int w)
        {
            H = h;
            W = w;
        }
        public int H { get; }
        public int W { get; }

        public XYZ AverageVector { get; }


    }

}
