using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    public partial class HLUnitUtils
    {

        public static double FromInternalUnits(double from, HLDisplayUnitTypeEnum to)
        {

#if REVIT2022
            var forgeType = HLConvert.ToForgeTypeId(to);
            return UnitUtils.ConvertFromInternalUnits(from, forgeType);
#else
            return UnitUtils.ConvertFromInternalUnits(from, (DisplayUnitType)(int)to);
#endif

        }



        public static double FromInternalUnits(double from, HLDisplayUnitType displayUnit)
        {
#if REVIT2022
            return UnitUtils.ConvertFromInternalUnits(from, displayUnit.AsForgeTypedId);
#else
            return UnitUtils.ConvertFromInternalUnits(from, (DisplayUnitType)(int)displayUnit.AsEnum);
#endif
        }
    }
}
