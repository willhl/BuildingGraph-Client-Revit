using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Client.Introspection
{

    public class BuildingGraphFieldType : IBuildingGraphFieldType
    {

        internal BuildingGraphFieldType(dynamic fieldDef)
        {
            Name = fieldDef.name;
            Kind = fieldDef.kind;
        }

        public string Name { get; }
        public string Kind { get; }

    }



}
