namespace BuildingGraph.Client.Introspection
{
    public interface IBuildingGraphFieldType
    {
        string Kind { get; }
        string Name { get; }
    }
}