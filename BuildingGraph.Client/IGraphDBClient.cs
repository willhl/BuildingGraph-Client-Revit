using System.Collections.Generic;
using BuildingGraph.Client.Model;

namespace BuildingGraph.Client
{
    public interface IGraphDBClient
    {
        void Dispose();
        PendingNode Push(Node node, Dictionary<string, object> variables);
        void Relate(PendingNode fromNodeId, PendingNode toNodeId, MEPEdgeTypes relType, Dictionary<string, object> variables);
        void Relate(Node fromNode, Node toNode, string relType, Dictionary<string, object> variables);

        void Commit();
    }
}