using System.Collections.Generic;

namespace BuildingGraph.Client.Model
{
    public class Node
    {

        string _label;

        public Node()
        {
        }

        public Node(string Label, string Id)
        {
            _label = Label;
            UniqueId = Id;
        }

        public string UniqueId { get; set; }

        public virtual string Label
        {
            get
            {
                if (string.IsNullOrEmpty(_label)) _label = GetType().Name;
                return _label;
            }
        }

        public string Name { get; set; }
        public Dictionary<string, object> ExtendedProperties { get; } = new Dictionary<string, object>();


        public virtual Dictionary<string, object> GetAllProperties()
        {
            var allProps = new Dictionary<string, object>();

            foreach(var kvp in ExtendedProperties)
            {
                allProps.Add(kvp.Key, kvp.Value);
            }

            if(!allProps.ContainsKey("Name")) allProps.Add("Name", Name);

            return allProps;

        }

    }

}
