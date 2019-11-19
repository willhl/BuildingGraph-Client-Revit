using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BuildingGraph.Client.Introspection
{
    public class BuildingGraphEnum : BuildingGraphType, IBuildingGraphEnum
    {

        internal BuildingGraphEnum(object typeDefObj) : base (typeDefObj)
        {
            dynamic typeDef = (dynamic)typeDefObj;

            var enList = new List<string>();
            if (typeDef.enumValues != null)
            {
                foreach (var enumv in typeDef.enumValues)
                {
                    enList.Add(enumv.name.Value);
                }
            }
            EnumValues = new ReadOnlyCollection<string>(enList);
        }

        public IReadOnlyCollection<string> EnumValues { get; }
    }

}
