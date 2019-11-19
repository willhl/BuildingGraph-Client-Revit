using System.Collections.Generic;

namespace BuildingGraph.Client.Introspection
{
    public interface IBuildingGraphType
    {
        string Description { get; }
        IReadOnlyCollection<IBuildingGraphField> Fields { get; }
        string Kind { get; }
        string Name { get; }

        IReadOnlyList<string> Interfaces { get; }
    }
}