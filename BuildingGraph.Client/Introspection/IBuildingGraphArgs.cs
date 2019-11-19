namespace BuildingGraph.Client.Introspection
{
    public interface IBuildingGraphArgs
    {
        object DefaultValue { get; }
        string Name { get; }
        string TypeName { get; }
    }
}