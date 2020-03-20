using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Graph.Nodes;
using BuildingGraph.Client.Neo4j;
using Nito.AsyncEx;

public static class Neo4j
{
    [NodeName("Create/Update Neo4jClient")]
    [NodeDescription("Creates Neo4jClient")]
    [NodeCategory("Neo4jClient")]
    [NodeSearchTags("Neo4j")]
    public static Neo4jClient CreateClient(string host, string username, string password)
    {
        var n4c = new Neo4jClient(new Uri(host), username, password);
        return n4c;
    }


    public static string RunCypherQuery(Neo4jClient client, string cypher, Dictionary<string, object> vars)
    {
   
        var task = Task.Run(() => AsyncContext.Run(() => client.RunCypherQueryAsync(cypher, vars)));

        if (task.Exception != null) throw new Exception("Query failed: " + task.Exception.Message, task.Exception);

        return task.Result.AsJson;
    }
}
