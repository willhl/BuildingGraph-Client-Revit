using System.Collections.Generic;

namespace BuildingGraph.Client.Introspection
{
    public interface IBuildingGraphSchema
    {
        IReadOnlyCollection<IBuildingGraphType> Types { get; }
        IEnumerable<IBuildingGraphField> GetMutations(string typeName);
        IEnumerable<IBuildingGraphField> GetMutations();
        IBuildingGraphType GetBuildingGraphType(string typeName);
    }
}