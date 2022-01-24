using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    public partial class HLConvert
    {
#if REVIT2022

        /// <summary>
        /// Spec is UnitType in Revit 2022 <
        /// </summary>
        /// <param name="unitTypeName"></param>
        /// <returns></returns>
        public static ForgeTypeId SpecToForgeTypeId(string unitTypeName)
        {
            var specs = SpecUtils.GetAllSpecs();

            var spec = specs.FirstOrDefault(s => s.TypeId == unitTypeName);

            return spec;
        }
#endif




    }
}
