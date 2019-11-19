using BuildingGraph.Client;

public class BGNode
{

    internal BGNode() { }

    internal BGNode(PendingNode pendingNode)
    {
        intPendingNode = pendingNode;
        Name = pendingNode.NodeName;
        Id = pendingNode.TempId;
        WasCommited = pendingNode.WasCommited;
    }

    public static BGNode FromNameAndId(string name, string id)
    {
        return new BGNode(new PendingNode(name, id));
    }

    public static BGNode FromName(string name)
    {
        return new BGNode(new PendingNode(name));
    }

    internal PendingNode intPendingNode { get; set; }

    public string Name { get; internal set; }

    public string Id { get; internal set; }

    public bool WasCommited { get; internal set; }
}

