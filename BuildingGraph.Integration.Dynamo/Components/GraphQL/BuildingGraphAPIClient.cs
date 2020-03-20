using BuildingGraph.Client;
using System.Collections.Generic;

public class BuildingGraphAPIClient
{


    public BuildingGraphAPIClient(string endpointURL, ClientMapping clientMapping)
    {
        Endpoint = endpointURL;
        Mapping = clientMapping;

        Client = new BuildingGraphClient(Endpoint);
    }

    internal BuildingGraphClient Client { get; }
    public string Endpoint { get; }
    public ClientMapping Mapping { get; }

    public void Commit(List<BGNode> nodes)
    {
        Client.Commit();
    }

}

