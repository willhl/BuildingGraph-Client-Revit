using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using BuildingGraph.Client.Introspection;
using BuildingGraph.Integration.Revit;

namespace BuildingGraph.Integration.Revit.Graph.introspection
{
    class EnumUtils
    {

        public HLDisplayUnitTypeEnum ToRevitUnits(IBuildingGraphEnum bgEnum, string enumValue)
        {
            switch (enumValue)
            {
                case "m":
                    return HLDisplayUnitTypeEnum.DUT_METERS;
                case "mm":
                    return HLDisplayUnitTypeEnum.DUT_MILLIMETERS;
                case "A":
                    return HLDisplayUnitTypeEnum.DUT_AMPERES;
                case "mA":
                    return HLDisplayUnitTypeEnum.DUT_MILLIAMPERES;
            }

            throw new Exception("Unknown unit type " + enumValue);
        }

    }
}
