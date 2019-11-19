using BuildingGraph.Client.Introspection;

public class ClientMapping
{
    public ClientMapping(string mappingJSON)
    {

        var clienMapping = new BuildingGraphMapping(mappingJSON);
    }

}

