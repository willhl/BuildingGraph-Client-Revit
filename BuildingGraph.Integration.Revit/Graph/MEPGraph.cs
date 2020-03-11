using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using gmdl = BuildingGraph.Client.Model;

namespace BuildingGraph.Integrations.Revit
{

    public class MEPRevitGraph
    {
        //collections to maintain relationship between revit element ids and their respective nodes
        //so that each time we look for a node we don't need to search the entire node collection
        //Dictionary<int, HashSet<MEPRevitEdge>> ElementEdges = new Dictionary<int, HashSet<MEPRevitEdge>>();
        Dictionary<int, HashSet<MEPRevitNode>> ElementNodes = new Dictionary<int, HashSet<MEPRevitNode>>();
        public HashSet<int> Systems = new HashSet<int>();


        /// <summary>
        /// Create a node for an element
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="merge">Reuse existing nodes for this element</param>
        /// <returns></returns>
        public MEPRevitNode AddElement(Element Element, bool merge = true)
        {

            var fromElmId = Element != null ? Element.Id.IntegerValue : -1;

            MEPRevitNode conn1Node = null;
            if (merge && ElementNodes.ContainsKey(fromElmId))
            {
                conn1Node = ElementNodes[fromElmId].FirstOrDefault();
            }

            if (conn1Node == null)
            {
                conn1Node = new MEPRevitNode(Element);
                AddNode(conn1Node, merge);
            }

            return conn1Node;
        }

        /// <summary>
        /// Add a node to the graph
        /// </summary>
        /// <param name="node"></param>
        /// <param name="merge">Don't add the node if it already exists</param>
        public void AddNode(MEPRevitNode node, bool merge = true)
        {
            var fromElmId = node.OriginId;
            if (!ElementNodes.ContainsKey(fromElmId))
            {
                var hs = new HashSet<MEPRevitNode>();
                hs.Add(node);
                ElementNodes.Add(fromElmId, hs);
            }
            else if (!merge || !ElementNodes[fromElmId].Contains(node))
            {
                ElementNodes[fromElmId].Add(node);
            }

            if (!_nodes.Contains(node)) _nodes.Add(node);

        }

        /// <summary>
        /// Adds a sectional connected to an element
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public MEPRevitNode NewSection(Element elm, gmdl.MEPEdgeTypes edgeType)
        {
            var sectionNode = new MEPRevitNode("Generic");
            if (elm != null)
            {
                var intNode = AddElement(elm);
                var cl = AddConnection(sectionNode, intNode, MEPPathConnectionType.SectionOf, edgeType);
            }

            return sectionNode;

        }


        /// <summary>
        /// Add a connection between two nodes
        /// </summary>
        /// <param name="fromNode"></param>
        /// <param name="toNode"></param>
        /// <param name="connectionType"></param>
        /// <param name="edgeType"></param>
        /// <returns></returns>
        public MEPRevitEdge AddConnection(MEPRevitNode fromNode, MEPRevitNode toNode, MEPPathConnectionType connectionType, gmdl.MEPEdgeTypes edgeType)
        {

            AddNode(fromNode);
            AddNode(toNode);

            var cnEdge = fromNode.Connections.FirstOrDefault(cn => cn.ConnectionType == connectionType && cn.AsNodeEdge.EdgeType == edgeType && cn.NextNode == toNode);

            if (cnEdge == null)
            {
                cnEdge = toNode.Connections.FirstOrDefault(cn => cn.ConnectionType == connectionType && cn.AsNodeEdge.EdgeType == edgeType && cn.NextNode == fromNode);
            }

            if (cnEdge == null)
            {
                cnEdge = new MEPRevitEdge(toNode.OriginId, fromNode, toNode, MEPPathDirection.In);
                _edges.Add(cnEdge);
                cnEdge.ConnectionType = connectionType;
                cnEdge.AsNodeEdge.EdgeType = edgeType;
                cnEdge.SystemId = -1;
                fromNode.Connections.Add(cnEdge);
                toNode.Connections.Add(cnEdge);
                
            }

            return cnEdge;
        }

        /// <summary>
        /// Adds a connection between two elements at specific point
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="otherElement"></param>
        /// <param name="atPoint"></param>
        /// <param name="connectionType"></param>
        /// <param name="edgeType"></param>
        /// <returns></returns>
        public MEPRevitEdge AddConnection(Element fromElement, Element otherElement, XYZ atPoint, MEPPathConnectionType connectionType, gmdl.MEPEdgeTypes edgeType)
        {
            var edge = AddConnection(fromElement, otherElement, connectionType, edgeType);
            edge.NextOrigin = atPoint;
            edge.ThisOrigin = atPoint;

            return edge;

        }

        /// <summary>
        /// Adds a connection between two elements
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="otherElement"></param>
        /// <param name="edgeType"></param>
        /// <returns></returns>
        public MEPRevitEdge AddConnection(Element fromElement, Element otherElement, MEPPathConnectionType connectionType, gmdl.MEPEdgeTypes edgeType)
        {

            var conn1Node = AddElement(fromElement);
            var conn2Node = AddElement(otherElement);
            var connEdge = AddConnection(conn1Node, conn2Node, connectionType, edgeType);

            return connEdge;

        }

        /// <summary>
        /// Adds a connection between a connector and a point on or in another element
        /// </summary>
        /// <param name="conn1"></param>
        /// <param name="otherPoint"></param>
        /// <param name="otherElement"></param>
        /// <param name="connectionType"></param>
        /// <returns></returns>
        public MEPRevitEdge AddConnection(Connector conn1, XYZ otherPoint, Element otherElement, MEPPathConnectionType connectionType, gmdl.MEPEdgeTypes edgeType)
        {
 
            //create nodes for the two elements
            var conn1Node = AddElement(conn1.Owner);
            var conn2Node = AddElement(otherElement);

            var edges = GetEdges(conn1);

            //look for an edge which already exists
            MEPRevitEdge connEdge = null;
#if REVIT2016
            connEdge = conn1Node.Connections.FirstOrDefault(ed =>
            ed.ThisConnectorIndex == conn1.Id 
            && ed.ConnectionType == connectionType
            && ed.AsNodeEdge.EdgeType == edgeType
            && ed.NextOrigin.DistanceTo(otherPoint) < 0.01);
#else
            throw new Exception("Only supported in Revit 2016 onwards");
#endif


            //direction should always be conn1 IN_TO conn2
            if (conn1.Direction == FlowDirectionType.In)
            {
                //swap over references 
                var tempconn1Node = conn1Node;
                conn1Node = conn2Node;
                conn2Node = tempconn1Node;
            }

            //create edge if an existing one wasn't found
            if (connEdge == null)
            {

#if REVIT2016
                connEdge = new MEPRevitEdge(conn1.Id, conn1Node, conn2Node, 
                    conn1.Domain == Autodesk.Revit.DB.Domain.DomainHvac 
                    || conn1.Domain == Autodesk.Revit.DB.Domain.DomainPiping ? (MEPPathDirection)(int)conn1.Direction : MEPPathDirection.Indeterminate);
                connEdge.ConnectionType = connectionType;
                connEdge.AsNodeEdge.EdgeType = edgeType;
                _edges.Add(connEdge);
#else
                throw new Exception("Only supported in Revit 2016 onwards");
#endif
                conn1Node.Connections.Add(connEdge);
                conn2Node.Connections.Add(connEdge);
                
            }


            connEdge.NextNode = conn2Node;
            connEdge.ConnectionType = connectionType;
            connEdge.SystemId = conn1.MEPSystem != null ? conn1.MEPSystem.Id.IntegerValue : -1;
            connEdge.ThisOrigin = conn1.Origin;
            connEdge.Description = conn1.Description;
            connEdge.AsNodeEdge.EdgeType = edgeType;
            if (conn1.Domain == Autodesk.Revit.DB.Domain.DomainHvac || conn1.Domain == Autodesk.Revit.DB.Domain.DomainPiping) connEdge.Weights.Add("Flow", conn1.Flow);
           // if (conn1.Domain == Autodesk.Revit.DB.Domain.DomainElectrical) connEdge.Weights.Add("Load", conn1.);

            MEPSystem system = null;
            if (otherElement is MEPCurve)
            {
                system = (otherElement as MEPCurve).MEPSystem;
            }
            else if (otherElement is FamilyInstance)
            {
                var famInstElm = (otherElement as FamilyInstance);
                if (famInstElm.MEPModel != null && famInstElm.MEPModel.ConnectorManager != null)
                {
                    //just get the system from the first connector with valid system
                    var systemConn = famInstElm.MEPModel.ConnectorManager.Connectors.OfType<Connector>().FirstOrDefault(cn => cn.MEPSystem != null);
                    if (systemConn != null)
                    {
                        system = systemConn.MEPSystem;
                    }
                }
            }

          
            if (conn1.MEPSystem != null && !Systems.Contains(conn1.MEPSystem.Id.IntegerValue))
            {
                Systems.Add(conn1.MEPSystem.Id.IntegerValue);
            }

            if (system != null && !Systems.Contains(system.Id.IntegerValue))
            {
                Systems.Add(system.Id.IntegerValue);
            }

         

            return connEdge;
        }

        /// <summary>
        /// Connect two connectors together
        /// </summary>
        /// <param name="conn1"></param>
        /// <param name="conn2"></param>
        /// <param name="connectionType"></param>
        /// <returns></returns>
        public MEPRevitEdge AddConnection(Connector conn1, Connector conn2, MEPPathConnectionType connectionType, gmdl.MEPEdgeTypes edgeType)
        {

            //direction should always be conn1 IN_TO conn2
            if ((conn1.Domain == Autodesk.Revit.DB.Domain.DomainPiping || conn1.Domain == Autodesk.Revit.DB.Domain.DomainHvac) && conn1.Direction == FlowDirectionType.In)
            {
                //swap over references 
                var tempConn1 = conn1;
                conn1 = conn2;
                conn2 = tempConn1;
            }

            

            MEPRevitNode conn1Node = AddElement(conn1.Owner);
            MEPRevitNode conn2Node = AddElement(conn2.Owner);

            MEPRevitEdge connEdge = null;
#if REVIT2016
            connEdge = conn1Node.Connections.FirstOrDefault(ed =>
            ed.ConnectionType == connectionType
            && ed.ThisConnectorIndex == conn1.Id
            && ed.NextNode == conn2Node
            && ed.NextConnectorIndex == conn2.Id);
#else
            throw new Exception("Only supported in Revit 2016 onwards");
#endif

            if (connEdge == null)
            {
#if REVIT2016
                connEdge = conn2Node.Connections.FirstOrDefault(ed =>
                ed.ConnectionType == connectionType
                && ed.ThisConnectorIndex == conn2.Id
                && ed.NextNode == conn1Node
                && ed.NextConnectorIndex == conn1.Id);
#endif

            }

            if (connEdge == null)
            {

#if REVIT2016
                connEdge = new MEPConnectorNodeEdge(conn1.Id, conn1Node, conn2Node, conn1.Domain == Autodesk.Revit.DB.Domain.DomainHvac || conn1.Domain == Autodesk.Revit.DB.Domain.DomainPiping ? (MEPPathDirection)(int)conn1.Direction : MEPPathDirection.Indeterminate);
                _edges.Add(connEdge);
#else
                throw new Exception("Only supported in Revit 2016 onwards");
#endif
                conn1Node.Connections.Add(connEdge);
                conn2Node.Connections.Add(connEdge);
                connEdge.ThisConnectorIndex = conn1.Id;
                connEdge.NextConnectorIndex = conn2.Id;
                connEdge.ThisOrigin = conn1.Origin;
                connEdge.NextOrigin = conn2.Origin;
                connEdge.NextNode = conn2Node;
            }

            var ow = conn1.Owner;
            if (ow != null)
            {
                var scp = ow.get_Parameter(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM);
                var sct = ow.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM);
                var scn = ow.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM);

                if (scp != null) connEdge.SetWeight("System Classification", scp.AsString());
                if (sct != null)
                {
                    var st = sct.AsValueString();
                    connEdge.SetWeight("System Type", st);
                    if (!string.IsNullOrEmpty(st)) connEdge.AsNodeEdge.TypeLabel = "FL_" + st.ToUpper().Replace(" ", "_");
                }

                if (scn != null) connEdge.SetWeight("System Name", scn.AsString());
            }


            connEdge.ConnectionType = connectionType;
            connEdge.SystemId = conn1.MEPSystem != null ? conn1.MEPSystem.Id.IntegerValue : -1;
            connEdge.Description = conn1.Description;
            //connEdge.Length = MEPGraphParserConnectors.GetDistanceBetweenConnectors(conn1, conn2);
            connEdge.AsNodeEdge.EdgeType = edgeType;


            if (conn1.Domain == Autodesk.Revit.DB.Domain.DomainHvac || conn1.Domain == Autodesk.Revit.DB.Domain.DomainPiping) connEdge.Flow = conn1.Flow;


            if (conn1.MEPSystem != null && !Systems.Contains(conn1.MEPSystem.Id.IntegerValue))
            {
                Systems.Add(conn1.MEPSystem.Id.IntegerValue);
            }

            if (conn2.MEPSystem != null && !Systems.Contains(conn2.MEPSystem.Id.IntegerValue))
            {
                Systems.Add(conn2.MEPSystem.Id.IntegerValue);
            }

            if (conn1.MEPSystem != null)
            {
                connEdge.SetWeight("SystemName", conn1.MEPSystem.Name);
            }
            else if (conn2.MEPSystem != null)
            {
                connEdge.SetWeight("SystemName", conn2.MEPSystem.Name);
            }

            return connEdge;

        }

        public IEnumerable<MEPRevitEdge> GetEdges(Element elm)
        {
            return GetEdges(elm.Id.IntegerValue);
        }

        public IEnumerable<MEPRevitEdge> GetEdges(int elmid)
        {

            if (ElementNodes.ContainsKey(elmid))
            {
                var cnNode = ElementNodes[elmid];
#if REVIT2016
                //find nodes which reference this connector directly
                //find edges which were added as the other side of a connection
                foreach (var edge in cnNode.SelectMany(ne => ne.Connections).Distinct())
                {
                    yield return edge;
                }

                /*
                foreach (var edge in Edges.Where(cn => cn.NextConnectorIndex == cnid && cnNode.Contains(cn.NextNode)).Distinct())
                {
                    yield return edge;
                }
                */
#else
                throw new Exception("Only supported in Revit 2016 onwards");
#endif


            }
        }

        public IEnumerable<MEPRevitEdge> GetEdges(Connector conn1)
        {
            var elmid = conn1.Owner.Id.IntegerValue;
            var cnid = conn1.Id;
      
            if (ElementNodes.ContainsKey(elmid))
            {
                var cnNode = ElementNodes[elmid];
#if REVIT2016
                //find nodes which reference this connector directly
                //find edges which were added as the other side of a connection
                foreach (var edge in cnNode.SelectMany(ne => ne.Connections).Where(en => en.ThisConnectorIndex == cnid).Distinct())
                {
                    yield return edge;
                }

                /*
                foreach (var edge in Edges.Where(cn => cn.NextConnectorIndex == cnid && cnNode.Contains(cn.NextNode)).Distinct())
                {
                    yield return edge;
                }
                */
#else
                throw new Exception("Only supported in Revit 2016 onwards");
#endif


            }

            

        }

        public IEnumerable<MEPRevitNode> GetNodes(Element element)
        {
            if (element == null) yield break;

            var fromElmId = element.Id.IntegerValue;
            if (ElementNodes.ContainsKey(fromElmId))
            {
                foreach (var n in ElementNodes[fromElmId])
                {
                    yield return n;
                }
            }
        }


        public bool ContainsElement(Element elm)
        {
            if (elm == null) return false;
            return ElementNodes.ContainsKey(elm.Id.IntegerValue);
        }

        public IEnumerable<MEPRevitNode> GetRootNodesForSystem(int systemId)
        {
            foreach (var ne in ElementNodes)
            {
                //check this is a root node, i.e. it has only one connection for this system
                var edgNodes = ne.Value.SelectMany(sb => sb.Connections).Where(np => np.SystemId == systemId).ToList();
                if (edgNodes.Count == 1)
                {
                    yield return edgNodes.First().ThisNode;
                }
            }

        }

        public IEnumerable<MEPRevitNode> GetAllNodesForSystem(int systemId)
        {
            return Edges.Where(np => np.SystemId == systemId).Select(sn => sn.ThisNode);
        }

        HashSet<MEPRevitNode> _nodes = new HashSet<MEPRevitNode>();
        HashSet<MEPRevitEdge> _edges = new HashSet<MEPRevitEdge>();

        public IEnumerable<MEPRevitNode> Nodes { get{ return _nodes; } }
        public IEnumerable<MEPRevitEdge> Edges { get { return _edges; } }

        //public IEnumerable<MEPRevitNode> Nodes { get => ElementNodes.SelectMany(en => en.Value); }
        //public IEnumerable<MEPRevitEdge> Edges { get => ElementNodes.SelectMany(v => v.Value).SelectMany(n => n.Connections).Distinct(); }



    }
}
