using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

using HLApps.Revit.Utils;
using HLApps.Revit.Geometry;
using Model = BuildingGraph.Client.Model;


namespace BuildingGraph.Integrations.Revit.Parsers
{


    public class MEPGraphParserConnectors : IRevitGraphParser
    {

        public virtual bool CanParse(Element elm)
        {
            return MEPUtils.GetConnectionManager(elm) != null;
        }

        public virtual ElementFilter GetFilter()
        {

            var cats = new BuiltInCategory[] { BuiltInCategory.OST_DuctCurves, BuiltInCategory.OST_PipeCurves, BuiltInCategory.OST_CableTray, BuiltInCategory.OST_DuctTerminal };
            var emcf = new ElementMulticategoryFilter(cats);
            return emcf;

        }


        public virtual void ParseFrom(ICollection<Element> elms, MEPRevitGraphWriter writer)
        {
            foreach (var elm in elms)
            {
                ParseFrom(elm, writer);
            }
        }

        public virtual void ParseFrom(Element elm, MEPRevitGraphWriter writer)
        {

            var scannedElements = writer.Cache.ParsedElements;
            var cpTree = writer.Cache.connectorsCache;
            var geoTree = writer.Cache.geoCache;
            var maxDepth = writer.Cache.MaxDepth;
            var graph = writer.Graph;


            var elmStack = new Stack<Tuple<Element, XYZ>>();
            elmStack.Push(new Tuple<Element, XYZ>(elm, null));


            var previouseConnections = new HashSet<Connector>();
            while (elmStack.Count >= 1)
            {
                var currentElementAndpt = elmStack.Pop();
                var currentElement = currentElementAndpt.Item1;
                var orgPoint = currentElementAndpt.Item2;
                scannedElements.Add(currentElement.Id.IntegerValue);

                ConnectorManager currentCm = MEPUtils.GetConnectionManager(currentElement);

                if (currentCm == null)
                {
                    ScanSpacesFromElement(currentElement, graph, scannedElements);
                    return;
                }

                var connStack = new Stack<Connector>();
                foreach (var conn in currentCm.Connectors.OfType<Connector>().Where(cn => cn.ConnectorType == ConnectorType.End || cn.ConnectorType == ConnectorType.Curve))// && !previouseConnections.Any(pc => pc.IsConnectedTo(cn))))
                {
                    connStack.Push(conn);
                }

                XYZ nextOrigin = null;
                while (connStack.Count >= 1)
                {

                    var currentConn = connStack.Pop();
                    Connector nextConnector = null;
                    Element nextElement = null;
                    nextOrigin = currentConn.Origin;
                    MEPRevitEdge edge = null;

                    var meps = GetSectionForConnctor(currentConn);
                    var edgeType = Model.MEPEdgeTypes.FLOWS_TO;
                    switch (currentConn.Domain)
                    {
                        case Autodesk.Revit.DB.Domain.DomainCableTrayConduit:
                            edgeType = Model.MEPEdgeTypes.CABLETRAY_FLOW_TO;
                             break;

                        case Autodesk.Revit.DB.Domain.DomainElectrical:
                            edgeType = Model.MEPEdgeTypes.ELECTRICAL_FLOW_TO;
                            break;

                        case Autodesk.Revit.DB.Domain.DomainPiping:
                            edgeType = Model.MEPEdgeTypes.HYDRONIC_FLOW_TO;
                            break;

                        case Autodesk.Revit.DB.Domain.DomainHvac:
                            edgeType = Model.MEPEdgeTypes.AIR_FLOW_TO;
                            break;

                        case Autodesk.Revit.DB.Domain.DomainUndefined:
                            edgeType = Model.MEPEdgeTypes.FLOWS_TO;
                            break;

                    }

                    if (currentConn.IsConnected)
                    {
                        //var ci = currentConn.GetMEPConnectorInfo();
                        //connection search
                        nextConnector = currentConn.AllRefs.OfType<Connector>().Where(cn => (cn.ConnectorType == ConnectorType.End || cn.ConnectorType == ConnectorType.Curve) && cn.IsConnectedTo(currentConn) && cn.Owner.Id.IntegerValue != currentElement.Id.IntegerValue).FirstOrDefault();
                        if (nextConnector != null)
                        {
                            previouseConnections.Add(currentConn);
                            edge = graph.AddConnection(currentConn, nextConnector, MEPPathConnectionType.Phyiscal, edgeType);
                            nextElement = nextConnector.Owner;
                            
                        }
                    }

                    if (nextConnector == null)
                    {
                        //geometry search
                        var connPoint = currentConn.Origin;
                        var nearPoints = cpTree.GetNearby(connPoint, 1F);
                        var nearPointsOrdered = nearPoints.Where(el => el.OriginatingElement.IntegerValue != currentElement.Id.IntegerValue).OrderBy(d => d.Point.DistanceTo(connPoint)).ToList();
                        var nearest = nearPointsOrdered.FirstOrDefault();
                        if (nearest != null)
                        {
                            var distance = nearest.Point.DistanceTo(connPoint);
                            if (distance < 0.01F)
                            {
                                var nearElm = elm.Document.GetElement(nearest.OriginatingElement);
                                if (nearElm != null)
                                {
                                    var mepm = MEPUtils.GetConnectionManager(nearElm);
#if REVIT2016
                                    nextConnector = (mepm != null) ? mepm.Connectors.OfType<Connector>().Where(cn => (cn.ConnectorType == ConnectorType.End || cn.ConnectorType == ConnectorType.Curve) && cn.Id == nearest.ConnectorIndex).FirstOrDefault() : null;
#else
                                    throw new Exception("Only supported in Revit 2016 onwards");
#endif
                                    if (nextConnector != null)
                                    {
                                        previouseConnections.Add(currentConn);
                                        edge = graph.AddConnection(currentConn, nextConnector, MEPPathConnectionType.Proximity, edgeType);
                                        nextElement = nextConnector.Owner;
                                        //nextOrigin = nextConnector.Origin;
                                    }
                                }
                            }
                        }

                        if (nextConnector == null && geoTree != null)
                        {
                            //todo: curve - point intersection check
                            var colBox = new HLBoundingBoxXYZ(connPoint, new XYZ(0.1, 0.1, 0.1));
                            var nearbyCurves = geoTree.GetColliding(colBox);

                            var nearbyCurve = nearbyCurves.OfType<CurveGeometrySegment>().Where(nb => nb.OriginatingElement.IntegerValue != currentConn.Owner.Id.IntegerValue).OrderBy(nd => nd.Geometry.Distance(connPoint)).FirstOrDefault();
                            if (nearbyCurve != null)
                            {
                                var distance = nearbyCurve.Geometry.Distance(connPoint);
                                //check if connector is sitting near or with extent of the mepcurve
                                var tolerence = 0.01;
                                if (distance < tolerence ||
                                    nearbyCurve.Radius > 0.001 && distance <= (nearbyCurve.Radius + tolerence)
                                    || nearbyCurve.Width > 0.001 && distance <= (nearbyCurve.Width + tolerence)
                                    || nearbyCurve.Height > 0.001 && distance <= (nearbyCurve.Height + tolerence))
                                {
                                    //how to add this connection to the graph with no next connector??
                                    var orgElme = elm.Document.GetElement(nearbyCurve.OriginatingElement);
                                    edge = graph.AddConnection(currentConn, connPoint, orgElme, MEPPathConnectionType.ProximityNoConnector, edgeType);
                                    nextElement = orgElme;
                                    //nextOrigin = connPoint;
                                }
                            }
                        }

                    }

                    if (edge != null && edge.Length < 0.1)
                    {
                        if (meps != null)
                        {
                            edge.Length = meps.TotalCurveLength;
                            edge.SetWeight("Roughness", meps.Roughness);
                            edge.SetWeight("ReynoldsNumber", meps.ReynoldsNumber);
                            edge.SetWeight("Velocity", meps.Velocity);
                            edge.SetWeight("VelocityPressure", meps.VelocityPressure);
                            edge.SetWeight("Flow", meps.Flow);
                        }
                        else if (orgPoint != null && nextOrigin != null)
                        {
                            var length = Math.Abs(orgPoint.DistanceTo(nextOrigin));
                            //very crude way to get length, should really be using MEPSection
                            if (length >= 0.1) edge.Length = length;

                            var height = Math.Abs(orgPoint.Z - nextOrigin.Z);
                            if (edge.AsNodeEdge.ExtendedProperties.ContainsKey("Height"))
                            {
                                edge.AsNodeEdge.ExtendedProperties["Height"] = height;
                            }
                            else
                            {
                                edge.AsNodeEdge.ExtendedProperties.Add("Height", height);
                            }
                        }
                    }


                    ScanSpacesFromElement(currentConn, graph, scannedElements, nextElement == null);


                    var currentDepth = elmStack.Count;
                    if (nextElement != null && (maxDepth == -1 || currentDepth < maxDepth) && !scannedElements.Contains(nextElement.Id.IntegerValue))// && !graph.ContainsElement(nextElement))
                    {
                        elmStack.Push(new Tuple<Element, XYZ>(nextElement, nextOrigin));

                        //ScanFromElement(elm, graph, cpTree, maxDepth, currentDepth++);
                    }

                }

                //check curve for nearby unconnected points
                if (cpTree != null && currentElement is MEPCurve)
                {
                    //point - curve intersection check
                    var curveElm = currentElement as MEPCurve;

                    var locCurve = curveElm.Location as LocationCurve;
                    var curve = locCurve.Curve;
                    var nearbyCurve = new CurveGeometrySegment(curve, currentElement);
                    var P1 = curve.GetEndPoint(0);
                    var P2 = curve.GetEndPoint(1);
                    var curveAsLine = curve is Line ? curve as Line : Line.CreateBound(P1, P2);
                    var nearbyCurves = cpTree.GetNearby(curveAsLine, (float)nearbyCurve.MaxDimension);

                    foreach (var cb in nearbyCurves)
                    {
                        if (cb.OriginatingElement.IntegerValue == currentElement.Id.IntegerValue) continue;
                        if (P1.DistanceTo(cb.Point) < 0.01) continue;
                        if (P2.DistanceTo(cb.Point) < 0.01) continue;

                        var tolerence = 0.01;
                        var cnpoint = curveAsLine.Project(cb.Point);
                        var distance = cnpoint.Distance;

                        //distance check is a crude way to check this element is sitting in the duct/pipe, could be nearby but intended to be connected.
                        if (distance < (nearbyCurve.MaxDimension / 2) + tolerence)
                        {
                            var orgElme = elm.Document.GetElement(cb.OriginatingElement);
                            if (orgElme == null) continue;
                            var cmgr = MEPUtils.GetConnectionManager(orgElme);
                            if (cmgr == null) continue;
#if REVIT2016
                            var nc = cmgr.Connectors.OfType<Connector>().FirstOrDefault(cn => cn.Id == cb.ConnectorIndex);
#else
                            Connector nc = null;
#endif
                            if (nc == null || nc.IsConnected) continue;

                            //further check that direction of point intersects curve
                            var prjInX = nc.CoordinateSystem.OfPoint(new XYZ(0, 0, 5));
                            var prjInnX = nc.CoordinateSystem.OfPoint(new XYZ(0, 0, -5));

                            var prjLine = Line.CreateBound(prjInnX, prjInX);
                            var ix = curveAsLine.Intersect(prjLine);
                            if (ix != SetComparisonResult.Overlap) continue;


                            var edge = graph.AddConnection(nc, cnpoint.XYZPoint, currentElement, MEPPathConnectionType.ProximityNoConnector, Model.MEPEdgeTypes.FLOWS_TO);

                            if (orgPoint != null)
                            {
                                edge.Length = Math.Abs(orgPoint.DistanceTo(cnpoint.XYZPoint));
                            }


                            if (!scannedElements.Contains(orgElme.Id.IntegerValue) && !elmStack.Any(el => el.Item1 == orgElme))// && !graph.ContainsElement(nextElement))
                            {
                                elmStack.Push(new Tuple<Element, XYZ>(orgElme, cnpoint.XYZPoint));
                            }
                        }
                    }

                }

                ScanSpacesFromElement(currentElement, graph, scannedElements);
            }

        }
        
        public MEPSection GetSectionForConnctor(Connector conn)
        {
            return null;
            /* doesn't work... need rethink
            var _system = conn.MEPSystem as MechanicalSystem;
            if (_system == null) return null;

            var orgId = conn.Owner.Id;
            var _sectionList = new List<MEPSection>();
            for (int i = 0; i < _system.SectionsCount; i++)
            {
                MEPSection mEPSection = _system.GetSectionByIndex(i);

                if (mEPSection.GetElementIds().Contains(orgId))
                {
                    _sectionList.Add(mEPSection);
                }
            }

            var section = _sectionList.OrderBy(n => n.Number).LastOrDefault();
            

            return section;*/
        }





        protected void ScanSpacesFromElement(Connector conn, MEPRevitGraph graph, HashSet<int> scannedElements, bool connectFlow)
        {

            var elm = conn.Owner;
            var doc = elm.Document;

            var phase = doc.GetElement(elm.CreatedPhaseId) as Phase;
            var org = conn.Origin;
            var pos = conn.Origin;

            var sp = elm.Document.GetSpaceAtPoint(pos, phase);
            Level lvl = null;
            if (sp == null)
            {
                var lvPAram = elm.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM);
                if (lvPAram != null)
                {
                    lvl = doc.GetElement(lvPAram.AsElementId()) as Level;
                    if (lvl != null)
                    {
                        var lvPs = new XYZ(pos.X, pos.Y, lvl.ProjectElevation + 1);
                        sp = elm.Document.GetSpaceAtPoint(lvPs, phase);
                    }
                }
            }


            if (sp == null)
            {
                var lvPAram = elm.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM);
                if (lvPAram != null)
                {
                    lvl = doc.GetElement(lvPAram.AsElementId()) as Level;
                    if (lvl != null)
                    {
                        var lvPs = new XYZ(pos.X, pos.Y, lvl.ProjectElevation + 1);
                        sp = elm.Document.GetSpaceAtPoint(lvPs, phase);
                    }
                }
            }

            //this check is needed!
            if (sp == null)
            {
                var p1 = XYZ.BasisZ;
                var p2 = conn.CoordinateSystem.OfVector(p1).Normalize();
                var tp2 = p2 * 0.2;
                var pos2 = pos + tp2;


                sp = elm.Document.GetSpaceAtPoint(pos2, phase);

                if (sp == null && lvl != null)
                {
                    pos2 = new XYZ(pos2.X, pos2.Y, lvl.ProjectElevation + 1);
                    sp = elm.Document.GetSpaceAtPoint(pos2, phase);
                }


            }

            if (connectFlow && (elm is Autodesk.Revit.DB.Mechanical.Duct) 
                || (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctFitting) 
                || (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctTerminal))
            {

                if (sp != null)
                {
                    scannedElements.Add(sp.Id.IntegerValue);

                    //connect air flow if it's part of a duct network and has an open side (either as an unconnected end or terminal)
                    if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctTerminal || (conn.Domain == Autodesk.Revit.DB.Domain.DomainHvac && !conn.IsConnected))
                    {
                        if (conn.Domain == Autodesk.Revit.DB.Domain.DomainHvac && conn.Direction == FlowDirectionType.In)
                        {
                            graph.AddConnection(elm, sp, org, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.FLOWS_TO_SPACE);
                        }
                        else
                        {
                            graph.AddConnection(sp, elm, org, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.FLOWS_TO_SPACE);
                        }
                    }
                }

                if (lvl != null)
                {
                    graph.AddConnection(elm, lvl, MEPPathConnectionType.Proximity, Model.MEPEdgeTypes.IS_ON);
                }
            }

            if (elm is MEPCurve && sp != null && !graph.GetEdges(elm).Any(ed => ed.AsNodeEdge.EdgeType == Model.MEPEdgeTypes.IS_IN_SPACE && ed.NextNode.OriginId == sp.Id.IntegerValue))
            {
                graph.AddConnection(elm, sp, pos, MEPPathConnectionType.Phyiscal, Model.MEPEdgeTypes.IS_IN_SPACE);

                
            }

            //TODO: also find extents of duct which intersects space
        }

        protected void ScanSpacesFromElement(Element elm, MEPRevitGraph graph, HashSet<int> scannedElements)
        {
            ScanSpacesFromElement(elm, graph, scannedElements, Transform.Identity, elm.Document);
        }


        protected void ScanSpacesFromElement(Element elm, MEPRevitGraph graph, HashSet<int> scannedElements, Transform hostTx, Document SpacesModel)
        {
            var elmDoc = elm.Document;

            var gpNode = graph.AddElement(elm, true);

            if (elm is FamilyInstance) //elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctTerminal || elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_MechanicalEquipment)
            {
                var fi = elm as FamilyInstance;
                var org = (elm.Location as LocationPoint).Point;
                var pos = (elm.Location as LocationPoint).Point;

                if (fi.HasSpatialElementCalculationPoint)
                {
                    pos = fi.GetSpatialElementCalculationPoint();
                }

                pos = hostTx.OfPoint(pos);

                Level lvl = null;
                //need to do phase mapping if elmDoc is not the same as the SpacesModel
                var phase = SpacesModel.GetElement(elm.CreatedPhaseId) as Phase;

                var sp = fi.Space;
              
                if (sp == null)
                {
                    sp = SpacesModel.GetSpaceAtPoint(pos, phase);
                }

                if (lvl == null)
                {
                    var lvPAram = elm.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM);
                    if (lvPAram != null)
                    {
                        lvl = elmDoc.GetElement(lvPAram.AsElementId()) as Level;
                    }
                }
                

                if (lvl == null)
                {
                    var lvPAram = elm.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM);
                    if (lvPAram != null)
                    {
                        lvl = elmDoc.GetElement(lvPAram.AsElementId()) as Level;
                    }
                }

                if (lvl == null && sp != null)
                {
                    lvl = sp.Level;
                }

                if (lvl == null)
                {
                    lvl = elmDoc.GetElement(fi.LevelId) as Level;
                }

                //check around element for a space
                double chDistance = 3.28084; //3.28084 = 1m

                if (sp == null)
                {
                    var hxOrt = fi.HandOrientation.Normalize().Negate();
                    pos = org + hxOrt.Multiply(-chDistance);
                    pos = hostTx.OfPoint(pos);
                    sp = SpacesModel.GetSpaceAtPoint(pos, phase);
                }


                if (sp == null)
                {
                    var hxOrt = fi.HandOrientation.Normalize().Negate();
                    pos = org + hxOrt.Multiply(chDistance);
                    pos = hostTx.OfPoint(pos);
                    sp = SpacesModel.GetSpaceAtPoint(pos, phase);
                }

                if (sp == null)
                {
                    var hxOrt = fi.FacingOrientation.Normalize().Negate();
                    pos = org + hxOrt.Multiply(-chDistance);
                    pos = hostTx.OfPoint(pos);
                    sp = SpacesModel.GetSpaceAtPoint(pos, phase);
                }

                if (sp == null)
                {
                    var hxOrt = fi.FacingOrientation.Normalize().Negate();
                    pos = org + hxOrt.Multiply(chDistance);
                    pos = hostTx.OfPoint(pos);
                    sp = SpacesModel.GetSpaceAtPoint(pos, phase);
                }

                if (sp == null)
                {
                    pos = org + new XYZ(0, 0, -chDistance);
                    pos = hostTx.OfPoint(pos);
                    sp = SpacesModel.GetSpaceAtPoint(pos, phase);
                }

                if (sp != null && sp.Level != null)
                {
                    lvl = sp.Level;
                }

                if (sp == null && lvl != null)
                {
                    var lvPs = new XYZ(pos.X, pos.Y, lvl.ProjectElevation + 2);
                    lvPs = hostTx.OfPoint(lvPs);
                    sp = elm.Document.GetSpaceAtPoint(lvPs, phase);
                }

                if (sp != null)
                {
                    scannedElements.Add(sp.Id.IntegerValue);

                    graph.AddConnection(elm, sp, pos, MEPPathConnectionType.Phyiscal, Model.MEPEdgeTypes.IS_IN_SPACE);

                    if (elm.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctTerminal)
                    {
                        //try to find out if this terminal is supply or extract
                        ConnectorManager currentCm = MEPUtils.GetConnectionManager(elm);

                        var inConn = currentCm != null ? currentCm.Connectors.OfType<Connector>().Where(cn =>
                        cn.Domain == Autodesk.Revit.DB.Domain.DomainHvac
                        && (cn.ConnectorType == ConnectorType.End || cn.ConnectorType == ConnectorType.Curve)
                        && cn.Direction == FlowDirectionType.In).FirstOrDefault() : null;

                        if (inConn != null)
                        {
                            graph.AddConnection(elm, sp, pos, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.FLOWS_TO_SPACE);
                        }
                        else
                        {
                            graph.AddConnection(sp, elm, pos, MEPPathConnectionType.Analytical, Model.MEPEdgeTypes.FLOWS_TO_SPACE);
                        }
                    }
                }

                if (lvl != null)
                {
                    gpNode.LevelId = lvl.Id.IntegerValue;
                }
            }

        }


        /// <summary>
        /// Gets the distance between the two connectors. Both connectors should be on the same element
        /// </summary>
        /// <param name="connector1"></param>
        /// <param name="connector2"></param>
        /// <returns></returns>
        public static double GetDistanceBetweenConnectors(Connector connector1, Connector connector2)
        {

            //some preliminary check to validate the supplied connectors
            if (connector1 == null | connector2 == null) return 0;


            var elm1 = connector1.Owner;
            var elm2 = connector2.Owner;
            var distanceConn1ToConn2 = 0D;

            if (elm1 == null || elm2 == null) return 0;

            //if it's a flex duct return length of duct instead
            //assuming flex ducts only have two connectors
            if ((elm1.Id.IntegerValue == elm2.Id.IntegerValue)
                && (elm1 is Autodesk.Revit.DB.Mechanical.FlexDuct))
            {
                distanceConn1ToConn2 = (elm1.Location as LocationCurve).Curve.Length;
                return distanceConn1ToConn2;
            }

            MEPCurve mepCurve1 = elm1 as MEPCurve;
            MEPCurve mepCurve2 = elm2 as MEPCurve;

            //get the XYZ points of each connector, and if they're curve connections  
            //project location of the connector onto the center line (curve) of the duct

            XYZ conn1Point = connector1.Origin;
            if (connector1.ConnectorType == ConnectorType.Curve && mepCurve1 != null)
            {
                //
                LocationCurve mepCurveLoc = mepCurve1.Location as LocationCurve;
                var dcInteresctResult = mepCurveLoc.Curve.Project(conn1Point);
                conn1Point = dcInteresctResult.XYZPoint;
                //add in the distance which forms right angle from duct curve to connector.
                distanceConn1ToConn2 += Math.Abs(dcInteresctResult.Distance);
            }

            XYZ conn2Point = connector2.Origin;
            if (connector2.ConnectorType == ConnectorType.Curve && mepCurve2 != null)
            {
                LocationCurve mepCurveLoc = mepCurve1.Location as LocationCurve;
                var dcInteresctResult = mepCurveLoc.Curve.Project(conn2Point);
                conn2Point = dcInteresctResult.XYZPoint;
                //add in the distance which forms right angle from duct curve to connector.
                distanceConn1ToConn2 += Math.Abs(dcInteresctResult.Distance);
            }


            //More accurate but does still assume the duct is a straight line.        
            distanceConn1ToConn2 += Math.Abs(conn1Point.DistanceTo(conn2Point));


            return distanceConn1ToConn2;
        }

        public void InitializeGraph(MEPRevitGraphWriter writer)
        {

        }

        public void FinalizeGraph(MEPRevitGraphWriter writer)
        {


        }


    }

}
