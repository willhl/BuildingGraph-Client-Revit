using System.Collections.Generic;
using Neo4j.Driver.V1;


namespace BuildingGraph.Client.Neo4j
{
    delegate void OnCommit(IStatementResult result);

    class PendingCypher
    {
        public string Query { get; set; }
        public Dictionary<string, object> Props { get; set; }

        public OnCommit Committed { get; set; }
        public Model.Node Node { get; set; }

        public PendingNode FromNode { get; set; }
        public PendingNode ToNode { get; set; }
        public Model.MEPEdgeTypes RelType { get; set; }
    }
}
