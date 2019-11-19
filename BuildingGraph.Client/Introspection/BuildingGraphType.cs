using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BuildingGraph.Client.Introspection
{
    public class BuildingGraphType : IBuildingGraphType
    {

        internal BuildingGraphType(object typeDefObj)
        {
            dynamic typeDef = (dynamic)typeDefObj;

            var _fields = new List<IBuildingGraphField>();
            Kind = typeDef.kind;
            Name = typeDef.name;
            Description = typeDef.description;

            if (typeDef.fields != null)
            {
                foreach (dynamic cr in typeDef.fields)
                {

                    var bgField = new BuildingGraphField(cr);
                    _fields.Add(bgField);
                }
            }

            Fields = new ReadOnlyCollection<IBuildingGraphField>(_fields);

            var interfaces = new List<string>();
            if (typeDef.interfaces != null)
            {
                foreach (dynamic inf in typeDef.interfaces)
                {
                    interfaces.Add(inf.name.Value);
                }
            }
            Interfaces = new ReadOnlyCollection<string>(interfaces);
        }

        public IReadOnlyList<string> Interfaces { get; }

        public string Kind { get; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyCollection<IBuildingGraphField> Fields { get; }

    }

}
