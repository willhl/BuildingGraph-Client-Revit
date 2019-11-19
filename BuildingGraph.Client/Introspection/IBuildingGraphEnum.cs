using System.Collections.Generic;

namespace BuildingGraph.Client.Introspection
{
    public interface IBuildingGraphEnum
    {
        IReadOnlyCollection<string> EnumValues { get; }
    }

}
