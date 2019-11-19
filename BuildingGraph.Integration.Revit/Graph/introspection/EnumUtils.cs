using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using BuildingGraph.Client.Introspection;

namespace BuildingGraph.Integrations.Revit.Graph.introspection
{
    class EnumUtils
    {

        public DisplayUnitType ToRevitUnits(IBuildingGraphEnum bgEnum, string enumValue)
        {
            switch (enumValue)
            {
                case "m":
                    return DisplayUnitType.DUT_METERS;
                case "mm":
                    return DisplayUnitType.DUT_MILLIMETERS;
                case "A":
                    return DisplayUnitType.DUT_AMPERES;
                case "mA":
                    return DisplayUnitType.DUT_MILLIAMPERES;
            }

            throw new Exception("Uknown unit type " + enumValue);
        }

    }
}
