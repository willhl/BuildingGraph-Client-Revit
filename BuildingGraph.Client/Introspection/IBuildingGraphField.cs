using System.Collections.Generic;

namespace BuildingGraph.Client.Introspection
{
    public interface IBuildingGraphField
    {
        string Description { get; }
        string Group { get; }
        string Name { get; }
        string Notes { get; }
        IBuildingGraphFieldType Type { get; }

        IReadOnlyCollection<IBuildingGraphArgs> Args { get; }
    }
}