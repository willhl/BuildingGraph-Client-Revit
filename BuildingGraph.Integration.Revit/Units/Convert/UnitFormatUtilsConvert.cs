using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    public partial class HLUnitFormatUtils
    {

        public static bool TryParse(Units units, HLUnitType hlUnit, string stringToParse, out double value)
        {

#if REVIT2022
            return UnitFormatUtils.TryParse(units, hlUnit.AsForgeTypedId, stringToParse, out value);
#else
            return UnitFormatUtils.TryParse(units, hlUnit.AsEnum, stringToParse, out value);
#endif

        }

    }
    
}
