namespace BuildingGraph.Client.Introspection
{
    public class BuildingGraphArgs : IBuildingGraphArgs
    {
        public BuildingGraphArgs(dynamic argDef)
        {
            Name = argDef.name;
            DefaultValue = argDef.defaultValue;
            TypeName = argDef.type.name;

            if (string.IsNullOrEmpty(TypeName))
            {
                if (argDef.type.ofType != null && argDef.type.ofType.name != null && !string.IsNullOrEmpty(argDef.type.ofType.name.Value))
                {
                    TypeName = argDef.type.ofType.name.Value;
                }
                else
                {
                    TypeName = "String"; //default to string
                }
            }

            if (argDef.type.kind == "NON_NULL") TypeName = TypeName + "!";

        }


        public string Name { get; }
        public object DefaultValue { get; }
        public string TypeName { get; }
    }



}
