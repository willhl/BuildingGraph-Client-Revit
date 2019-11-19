using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BuildingGraph.Client.Introspection
{
    public class BuildingGraphMapping
    {
        public BuildingGraphMapping(string mpjson)
        {
            var mpobj = (dynamic)JsonConvert.DeserializeObject(mpjson);

            var _bt = new List<BuildingGraphMappedType>();
            foreach(var type in mpobj.Types)
            {
                _bt.Add(new BuildingGraphMappedType(type));
            }

            Types = new ReadOnlyCollection<BuildingGraphMappedType>(_bt);
        }

        public IReadOnlyCollection<BuildingGraphMappedType> Types;

    }

    public class BuildingGraphMappedType
    {
        internal BuildingGraphMappedType(dynamic def)
        {
           

            Name = def.Name;
            if (def.Value.Kind != null) Kind = def.Value.Kind.Value;
            if (def.Value.NativeType != null) NativeType = def.Value.NativeType.Value;
            if (def.Value.Create != null) Create = def.Value.Create.Value;
            if (def.Value.Delete != null) Delete = def.Value.Delete.Value;
            if (def.Value.Update != null) Update = def.Value.Update.Value;

            var _vm = new Dictionary<string, string>();
            if (def.Value.Mappings != null)
            {              
                foreach (var mp in def.Value.Mappings)
                {
                    _vm.Add(mp.Name, mp.Value.Value);
                }
            }
            ValueMap = new ReadOnlyDictionary<string, string>(_vm);
        }

        public string Kind { get; }
        public string Name { get; }
        public string NativeType { get; }

        public string Create { get; }
        public string Delete { get; }
        public string Update { get; }


        public IReadOnlyDictionary<string, string> ValueMap { get; }
    }

   
}
