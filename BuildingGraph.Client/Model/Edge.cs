using System.Collections.Generic;

namespace BuildingGraph.Client.Model
{
    public class Edge
    {
        public virtual string TypeLabel
        {
            get; set;
        }

        public MEPEdgeTypes EdgeType { get; set; }


        Dictionary<string, object> _extendedProperties = new Dictionary<string, object>();
        public Dictionary<string, object> ExtendedProperties { get => _extendedProperties; }
        public virtual Dictionary<string, object> GetAllProperties()
        {
            var allProps = new Dictionary<string, object>();

            foreach (var kvp in ExtendedProperties)
            {
                allProps.Add(kvp.Key, kvp.Value);
            }

            return allProps;

        }
    }

}
