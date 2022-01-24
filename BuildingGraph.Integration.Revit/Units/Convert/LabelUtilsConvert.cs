using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    public partial class HLLabelUtils
    {

        public static string GetLableFor(HLUnitType unitType)
        {

#if REVIT2022
            return LabelUtils.GetLabelForSpec(unitType.AsForgeTypedId);
#else
            return LabelUtils.GetLabelFor(unitType.AsEnum);
#endif
        }

        public static string GetLableFor(HLUnitSymbolType unitType)
        {
#if REVIT2022
            return LabelUtils.GetLabelForSymbol(unitType.AsForgeTypedId);
#else
            return LabelUtils.GetLabelFor(unitType.AsEnum);
#endif

        }

    }
}
