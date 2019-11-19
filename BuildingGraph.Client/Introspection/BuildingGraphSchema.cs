using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace BuildingGraph.Client.Introspection
{
    public class BuildingGraphSchema : IBuildingGraphSchema
    {
        internal BuildingGraphSchema(dynamic schemaDef)
        {
            var types = new List<IBuildingGraphType>();

            if (schemaDef.types != null)
            {
                foreach (dynamic cr in schemaDef.types)
                {
                    BuildingGraphType bgType;
                    if (cr.kind == "ENUM")
                    {
                        bgType = new BuildingGraphEnum(cr);                   
                    }
                    else
                    {
                        bgType = new BuildingGraphType(cr);
                    }

                    types.Add(bgType);
                }
            }

            Types = new ReadOnlyCollection<IBuildingGraphType>(types);

        }

        public IReadOnlyCollection<IBuildingGraphType> Types { get; }

        public IEnumerable<IBuildingGraphField> GetMutations()
        {
            var mutationType = Types.Where(tl => tl.Kind == "OBJECT" && tl.Name == "Mutation").FirstOrDefault();
            return mutationType.Fields;
        }

         public IEnumerable<IBuildingGraphField> GetMutations(string typeName)
        {
            var mutationType = Types.Where(tl => tl.Kind == "OBJECT" && tl.Name == "Mutation").FirstOrDefault();
            var fields = mutationType.Fields.Where(fl => fl.Type != null && fl.Type.Kind == "OBJECT" && fl.Type.Name == typeName).ToList();
            return fields;
        }

        public IBuildingGraphType GetBuildingGraphType(string typeName)
        {
            var type = Types.Where(tl => tl.Name == typeName).FirstOrDefault();
            return type;

        }

        public IEnumerable<IBuildingGraphType> GetObjects()
        {
            return Types.Where(ts => ts.Kind == "OBJECT");
        }

        public IEnumerable<IBuildingGraphEnum> GetEnums()
        {
            return Types.Where(ts => ts.Kind == "ENUM").OfType<IBuildingGraphEnum>();
        }
    }




}
