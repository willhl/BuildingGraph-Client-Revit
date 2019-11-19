using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using BuildingGraph.Client;

public static class Mutations
{



    [NodeName("Create/Update Node")]
    [NodeDescription("Creates or updates a node")]
    [NodeCategory("Mutations")]
    [NodeSearchTags("Graphql")]
    public static BGNode CreateNode(BuildingGraphAPIClient client, BGNode node, Dictionary<string, object> vars)
    {

        var _client = client.Client;
        var elmId = node.Id;
        if (node.intPendingNode.WasCommited) return node;

        Dictionary<string, object> mergeOn = null;
        if (!string.IsNullOrEmpty(elmId) && node.WasCommited)
        {
            mergeOn = new Dictionary<string, object>();
            mergeOn.Add("Id", elmId);
        }

        var result = _client.Push(node.Name, vars, mergeOn);
        _client.Commit();

        if (result != null)
        {
            node.intPendingNode = result;
            node.Id = result.TempId;
            node.WasCommited = result.WasCommited;
        }
    
        return node;
    }


    public static BGNode UpdateNode(BuildingGraphAPIClient client, BGNode node, Dictionary<string, object> vars)
    {

        var _client = client.Client;
        var elmId = node.Id;
        Dictionary<string, object> mergeOn = null;
        if (!string.IsNullOrEmpty(elmId) && node.WasCommited)
        {
            mergeOn = new Dictionary<string, object>();
            mergeOn.Add("Id", elmId);

            var result = _client.Push(node.Name, vars, mergeOn);
            _client.Commit();

        }

        return node;
    }

    public static string RelateNode(BuildingGraphAPIClient client, BGNode fromNode, BGNode toNode, string relationshipType)
    {

        var _client = client.Client;


        if (string.IsNullOrEmpty(fromNode.Id)) return "From node must have an ID";
        if (string.IsNullOrEmpty(toNode.Id)) return "To node must have an ID";
       
        var fromPn = new PendingNode(fromNode.Name, fromNode.Id);
        var toPn = new PendingNode(toNode.Name, toNode.Id);
        fromNode.intPendingNode = fromPn;
        toNode.intPendingNode = toPn;

        //variables in relationships not supported yet
        _client.Relate(fromPn, toPn, relationshipType, null);
        _client.Commit();

        return "OK";
    }

}

