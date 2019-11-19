using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace BuildingGraph.Client.Introspection
{
    public class BuildingGraphField : IBuildingGraphField
    {
        internal BuildingGraphField(dynamic fieldDef)
        {
            Description = fieldDef.description;
            //Notes = parse from description
            //Group = parse from description
            Name = fieldDef.name;
            Type = new BuildingGraphFieldType(fieldDef.type);

            var _args = new List<IBuildingGraphArgs>();
            foreach (var arg in fieldDef.args)
            {
                var bgarg = new BuildingGraphArgs(arg);
                _args.Add(bgarg);

                if (bgarg.Name == "units") UnitsTypeName = bgarg.TypeName;
            }

            Args = new ReadOnlyCollection<IBuildingGraphArgs>(_args);

        }

        public string Description { get; }
        public string Group { get; }
        public string Notes { get; }
        public string Name { get; }
        public IBuildingGraphFieldType Type { get; }
        public IReadOnlyCollection<IBuildingGraphArgs> Args { get; }

        public string UnitsTypeName { get; }
    }



}
