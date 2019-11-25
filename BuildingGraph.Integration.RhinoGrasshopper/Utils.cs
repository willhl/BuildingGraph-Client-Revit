using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;

namespace BuildingGraph.Client.RhinoGrasshopper
{
    class Utils
    {

        public static object ToGraphQLCompatibleValue(object ghValue)
        {
            //it's ok to be null
            if (ghValue is null) return ghValue;
            if (ghValue is GH_Integer) return ((GH_Integer)ghValue).Value;
            if (ghValue is GH_Number) return ((GH_Number)ghValue).Value;
            if (ghValue is GH_String) return ((GH_String)ghValue).Value;

            //throw it back anyway... sould probably raise an exception about unsuported type
            return ghValue;
        }

        public static Dictionary<string, object> GHInputsToDictionary(IList<string> variableNames, IList<object> variableValues)
        {
            var inputVariables = new Dictionary<string, object>();
            for (int varIdx = 0; varIdx < variableNames.Count() && varIdx < variableValues.Count(); varIdx++)
            {
                var name = variableNames[varIdx];
                if (!string.IsNullOrEmpty(name) && !inputVariables.ContainsKey(name))
                {
                    var value = ToGraphQLCompatibleValue(variableValues[varIdx]);
                    var qlname = Client.Utils.GetGraphQLCompatibleFieldName(name);
                    inputVariables.Add(qlname, value);
                }
            }

            return inputVariables;
        }
    }
}
