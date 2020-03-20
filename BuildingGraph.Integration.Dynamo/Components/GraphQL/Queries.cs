using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

public static class Queries
{
    /// <summary>
    /// Queries the building graph API
    /// </summary>
    /// <param name="client">the building graph api client</param>
    /// <param name="query">the GraphQL query</param>
    /// <param name="inputVariables">Dictionary of key value pairs for variables in the GraphQL query</param>
    /// <returns></returns>
    [CanUpdatePeriodically(true)]
    public static string BGGraphQLQuery(BuildingGraphAPIClient client, string query, Dictionary<string, object> inputVariables)
    {
        var _client = client.Client;
        var result = _client.ExecuteQuery(query, inputVariables);

        return result != null ? result.ToString() : string.Empty;
    }

}

