using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    public partial class HLConvert
    {

        public static HLDisplayUnitType ToHLDisplayUnitType(HLDisplayUnitTypeEnum unitType)
        {

#if REVIT2022
            var forgeType = ToForgeTypeId(unitType);
            return new HLDisplayUnitType(forgeType);
#else 

            return new HLDisplayUnitType((DisplayUnitType)(int)unitType);
#endif

        }


#if REVIT2022

        static Dictionary<HLDisplayUnitTypeEnum, ForgeTypeId> ForgeTypeDisplayunitMap;


        /// <summary>
        /// To be used when checking for expected or known legacy UnitType from Revit 2021 and earlier.
        /// Generally, for Revit 2022+, it recommended to obtain the ForgeTypeID
        /// using Revit's provided methods if you don't need to check 
        /// for equality with the known enums.
        /// </summary>
        /// <param name="unitType">The HLDisplayUnitTypeEnum enumeration to lookup UnitTypeId</param>
        /// <returns></returns>
        public static ForgeTypeId ToForgeTypeId(HLDisplayUnitTypeEnum displayType)
        {

            if (ForgeTypeDisplayunitMap == null)
            {

                ForgeTypeDisplayunitMap = new Dictionary<HLDisplayUnitTypeEnum, ForgeTypeId>();

                //Some of these had no obvious mapping
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_UNDEFINED, new ForgeTypeId());


                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECIMAL_FEET, UnitTypeId.Feet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECIMAL_INCHES, UnitTypeId.Inches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECIMAL_DEGREES, UnitTypeId.Degrees);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DEGREES_AND_MINUTES, UnitTypeId.DegreesMinutes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CANDLEPOWER, UnitTypeId.Custom);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RISE_OVER_INCHES, UnitTypeId.RiseDividedBy12Inches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RISE_OVER_FOOT, UnitTypeId.RiseDividedBy1Foot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RISE_OVER_MMS, UnitTypeId.RiseDividedBy1000Millimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FAHRENHEIT_DIFFERENCE, UnitTypeId.FahrenheitInterval);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CELSIUS_DIFFERENCE, UnitTypeId.CelsiusInterval);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KELVIN_DIFFERENCE, UnitTypeId.KelvinInterval);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RANKINE_DIFFERENCE, UnitTypeId.RankineInterval);


                //all these should be fine
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_1_RATIO, UnitTypeId.OneToRatio);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUSTOM, UnitTypeId.Custom);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS, UnitTypeId.Meters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CENTIMETERS, UnitTypeId.Centimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_FRACTIONAL_INCHES, UnitTypeId.FeetFractionalInches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FRACTIONAL_INCHES, UnitTypeId.FractionalInches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_ACRES, UnitTypeId.Acres);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_HECTARES, UnitTypeId.Hectares);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS_CENTIMETERS, UnitTypeId.MetersCentimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_YARDS, UnitTypeId.CubicYards);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_FEET, UnitTypeId.SquareFeet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_METERS, UnitTypeId.SquareMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET, UnitTypeId.CubicFeet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_METERS, UnitTypeId.CubicMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_GENERAL, UnitTypeId.General);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FIXED, UnitTypeId.Fixed);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_PERCENTAGE, UnitTypeId.Percentage);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_INCHES, UnitTypeId.SquareInches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_CENTIMETERS, UnitTypeId.SquareCentimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_MILLIMETERS, UnitTypeId.SquareMillimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_INCHES, UnitTypeId.CubicInches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_CENTIMETERS, UnitTypeId.CubicCentimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_MILLIMETERS, UnitTypeId.CubicMillimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS, UnitTypeId.Liters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_GALLONS_US, UnitTypeId.UsGallons);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_PER_CUBIC_METER, UnitTypeId.KilogramsPerCubicMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS_PER_CUBIC_FOOT, UnitTypeId.PoundsMassPerCubicFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS_PER_CUBIC_INCH, UnitTypeId.PoundsMassPerCubicInch);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS, UnitTypeId.BritishThermalUnits);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CALORIES, UnitTypeId.Calories);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOCALORIES, UnitTypeId.Kilocalories);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_JOULES, UnitTypeId.Joules);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOWATT_HOURS, UnitTypeId.KilowattHours);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_THERMS, UnitTypeId.Therms);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INCHES_OF_WATER_PER_100FT, UnitTypeId.InchesOfWater60DegreesFahrenheitPer100Feet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_PASCALS_PER_METER, UnitTypeId.PascalsPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS, UnitTypeId.Watts);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOWATTS, UnitTypeId.Kilowatts);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_SECOND, UnitTypeId.BritishThermalUnitsPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_HOUR, UnitTypeId.BritishThermalUnitsPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CALORIES_PER_SECOND, UnitTypeId.CaloriesPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOCALORIES_PER_SECOND, UnitTypeId.KilocaloriesPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS_PER_SQUARE_FOOT, UnitTypeId.WattsPerSquareFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS_PER_SQUARE_METER, UnitTypeId.WattsPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INCHES_OF_WATER, UnitTypeId.InchesOfWater60DegreesFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_PASCALS, UnitTypeId.Pascals);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOPASCALS, UnitTypeId.Kilopascals);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MEGAPASCALS, UnitTypeId.Megapascals);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_FORCE_PER_SQUARE_INCH, UnitTypeId.PoundsForcePerSquareInch);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INCHES_OF_MERCURY, UnitTypeId.InchesOfMercury32DegreesFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILLIMETERS_OF_MERCURY, UnitTypeId.MillimetersOfMercury);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_ATMOSPHERES, UnitTypeId.Atmospheres);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BARS, UnitTypeId.Bars);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FAHRENHEIT, UnitTypeId.Fahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CELSIUS, UnitTypeId.Celsius);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KELVIN, UnitTypeId.Kelvin);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RANKINE, UnitTypeId.Rankine);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_PER_MINUTE, UnitTypeId.FeetPerMinute);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS_PER_SECOND, UnitTypeId.MetersPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CENTIMETERS_PER_MINUTE, UnitTypeId.CentimetersPerMinute);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET_PER_MINUTE, UnitTypeId.CubicFeetPerMinute);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS_PER_SECOND, UnitTypeId.LitersPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_METERS_PER_SECOND, UnitTypeId.CubicMetersPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_METERS_PER_HOUR, UnitTypeId.CubicMetersPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_GALLONS_US_PER_MINUTE, UnitTypeId.UsGallonsPerMinute);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_GALLONS_US_PER_HOUR, UnitTypeId.UsGallonsPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_AMPERES, UnitTypeId.Amperes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOAMPERES, UnitTypeId.Kiloamperes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILLIAMPERES, UnitTypeId.Milliamperes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_VOLTS, UnitTypeId.Volts);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOVOLTS, UnitTypeId.Kilovolts);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILLIVOLTS, UnitTypeId.Millivolts);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_HERTZ, UnitTypeId.Hertz);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CYCLES_PER_SECOND, UnitTypeId.CyclesPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LUX, UnitTypeId.Lux);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FOOTCANDLES, UnitTypeId.Footcandles);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FOOTLAMBERTS, UnitTypeId.Footlamberts);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CANDELAS_PER_SQUARE_METER, UnitTypeId.CandelasPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CANDELAS, UnitTypeId.Candelas);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LUMENS, UnitTypeId.Lumens);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_VOLT_AMPERES, UnitTypeId.VoltAmperes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOVOLT_AMPERES, UnitTypeId.KilovoltAmperes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_HORSEPOWER, UnitTypeId.Horsepower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NEWTONS, UnitTypeId.Newtons);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECANEWTONS, UnitTypeId.Dekanewtons);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTONS, UnitTypeId.Kilonewtons);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MEGANEWTONS, UnitTypeId.Meganewtons);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS, UnitTypeId.Kips);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_FORCE, UnitTypeId.KilogramsForce);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TONNES_FORCE, UnitTypeId.TonnesForce);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_FORCE, UnitTypeId.PoundsForce);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NEWTONS_PER_METER, UnitTypeId.NewtonsPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECANEWTONS_PER_METER, UnitTypeId.DekanewtonsPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTONS_PER_METER, UnitTypeId.KilonewtonsPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MEGANEWTONS_PER_METER, UnitTypeId.MeganewtonsPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS_PER_FOOT, UnitTypeId.KipsPerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_FORCE_PER_METER, UnitTypeId.KilogramsForcePerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TONNES_FORCE_PER_METER, UnitTypeId.TonnesForcePerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_FORCE_PER_FOOT, UnitTypeId.PoundsForcePerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NEWTONS_PER_SQUARE_METER, UnitTypeId.NewtonsPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECANEWTONS_PER_SQUARE_METER, UnitTypeId.DekanewtonsPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTONS_PER_SQUARE_METER, UnitTypeId.KilonewtonsPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MEGANEWTONS_PER_SQUARE_METER, UnitTypeId.MeganewtonsPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS_PER_SQUARE_FOOT, UnitTypeId.KipsPerSquareFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_FORCE_PER_SQUARE_METER, UnitTypeId.KilogramsForcePerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TONNES_FORCE_PER_SQUARE_METER, UnitTypeId.TonnesForcePerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_FORCE_PER_SQUARE_FOOT, UnitTypeId.PoundsForcePerSquareFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NEWTON_METERS, UnitTypeId.NewtonMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECANEWTON_METERS, UnitTypeId.DekanewtonMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTON_METERS, UnitTypeId.KilonewtonMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MEGANEWTON_METERS, UnitTypeId.MeganewtonMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIP_FEET, UnitTypeId.KipFeet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAM_FORCE_METERS, UnitTypeId.KilogramForceMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TONNE_FORCE_METERS, UnitTypeId.TonneForceMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUND_FORCE_FEET, UnitTypeId.PoundForceFeet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS_PER_KILONEWTON, UnitTypeId.MetersPerKilonewton);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_PER_KIP, UnitTypeId.FeetPerKip);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_METERS_PER_KILONEWTON, UnitTypeId.SquareMetersPerKilonewton);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_FEET_PER_KIP, UnitTypeId.SquareFeetPerKip);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_METERS_PER_KILONEWTON, UnitTypeId.CubicMetersPerKilonewton);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET_PER_KIP, UnitTypeId.CubicFeetPerKip);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INV_KILONEWTONS, UnitTypeId.InverseKilonewtons);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INV_KIPS, UnitTypeId.InverseKips);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_OF_WATER_PER_100FT, UnitTypeId.InchesOfWater60DegreesFahrenheitPer100Feet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_OF_WATER, UnitTypeId.FeetOfWater39_2DegreesFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_PASCAL_SECONDS, UnitTypeId.PascalSeconds);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS_PER_FOOT_SECOND, UnitTypeId.PoundsMassPerFootSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CENTIPOISES, UnitTypeId.Centipoises);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_PER_SECOND, UnitTypeId.FeetPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS_PER_SQUARE_INCH, UnitTypeId.KipsPerSquareInch);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTONS_PER_CUBIC_METER, UnitTypeId.KilonewtonsPerCubicMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_FORCE_PER_CUBIC_FOOT, UnitTypeId.PoundsForcePerCubicFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS_PER_CUBIC_INCH, UnitTypeId.KipsPerCubicInch);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INV_FAHRENHEIT, UnitTypeId.InverseDegreesFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INV_CELSIUS, UnitTypeId.InverseDegreesCelsius);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NEWTON_METERS_PER_METER, UnitTypeId.NewtonMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECANEWTON_METERS_PER_METER, UnitTypeId.DekanewtonMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTON_METERS_PER_METER, UnitTypeId.KilonewtonMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MEGANEWTON_METERS_PER_METER, UnitTypeId.MeganewtonMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIP_FEET_PER_FOOT, UnitTypeId.KipFeetPerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAM_FORCE_METERS_PER_METER, UnitTypeId.KilogramForceMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TONNE_FORCE_METERS_PER_METER, UnitTypeId.TonneForceMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUND_FORCE_FEET_PER_FOOT, UnitTypeId.PoundForceFeetPerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS_PER_FOOT_HOUR, UnitTypeId.PoundsMassPerFootHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS_PER_INCH, UnitTypeId.KipsPerInch);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIPS_PER_CUBIC_FOOT, UnitTypeId.KipsPerCubicFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIP_FEET_PER_DEGREE, UnitTypeId.KipFeetPerDegree);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTON_METERS_PER_DEGREE, UnitTypeId.KilonewtonMetersPerDegree);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KIP_FEET_PER_DEGREE_PER_FOOT, UnitTypeId.KipFeetPerDegreePerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTON_METERS_PER_DEGREE_PER_METER, UnitTypeId.KilonewtonMetersPerDegreePerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS_PER_SQUARE_METER_KELVIN, UnitTypeId.WattsPerSquareMeterKelvin);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT_FAHRENHEIT, UnitTypeId.BritishThermalUnitsPerHourSquareFootDegreeFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET_PER_MINUTE_SQUARE_FOOT, UnitTypeId.CubicFeetPerMinuteSquareFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS_PER_SECOND_SQUARE_METER, UnitTypeId.LitersPerSecondSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RATIO_10, UnitTypeId.RatioTo10);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RATIO_12, UnitTypeId.RatioTo12);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SLOPE_DEGREES, UnitTypeId.SlopeDegrees);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS_PER_CUBIC_FOOT, UnitTypeId.WattsPerCubicFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS_PER_CUBIC_METER, UnitTypeId.WattsPerCubicMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT, UnitTypeId.BritishThermalUnitsPerHourSquareFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_HOUR_CUBIC_FOOT, UnitTypeId.BritishThermalUnitsPerHourCubicFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TON_OF_REFRIGERATION, UnitTypeId.TonsOfRefrigeration);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET_PER_MINUTE_CUBIC_FOOT, UnitTypeId.CubicFeetPerMinuteCubicFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS_PER_SECOND_CUBIC_METER, UnitTypeId.LitersPerSecondCubicMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET_PER_MINUTE_TON_OF_REFRIGERATION, UnitTypeId.CubicFeetPerMinuteTonOfRefrigeration);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS_PER_SECOND_KILOWATTS, UnitTypeId.LitersPerSecondKilowatt);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_FEET_PER_TON_OF_REFRIGERATION, UnitTypeId.SquareFeetPerTonOfRefrigeration);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_METERS_PER_KILOWATTS, UnitTypeId.SquareMetersPerKilowatt);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CURRENCY, UnitTypeId.Currency);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LUMENS_PER_WATT, UnitTypeId.LumensPerWatt);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_FEET_PER_THOUSAND_BRITISH_THERMAL_UNITS_PER_HOUR, UnitTypeId.SquareFeetPer1000BritishThermalUnitsPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTONS_PER_SQUARE_CENTIMETER, UnitTypeId.KilonewtonsPerSquareCentimeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NEWTONS_PER_SQUARE_MILLIMETER, UnitTypeId.NewtonsPerSquareMillimeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILONEWTONS_PER_SQUARE_MILLIMETER, UnitTypeId.KilonewtonsPerSquareMillimeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RISE_OVER_120_INCHES, UnitTypeId.RiseDividedBy120Inches);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RISE_OVER_10_FEET, UnitTypeId.RiseDividedBy10Feet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_HOUR_SQUARE_FOOT_FAHRENHEIT_PER_BRITISH_THERMAL_UNIT, UnitTypeId.HourSquareFootDegreesFahrenheitPerBritishThermalUnit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_METER_KELVIN_PER_WATT, UnitTypeId.SquareMeterKelvinsPerWatt);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNIT_PER_FAHRENHEIT, UnitTypeId.BritishThermalUnitsPerDegreeFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_JOULES_PER_KELVIN, UnitTypeId.JoulesPerKelvin);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOJOULES_PER_KELVIN, UnitTypeId.KilojoulesPerKelvin);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_MASS, UnitTypeId.Kilograms);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_TONNES_MASS, UnitTypeId.Tonnes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS, UnitTypeId.PoundsMass);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS_PER_SECOND_SQUARED, UnitTypeId.MetersPerSecondSquared);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOMETERS_PER_SECOND_SQUARED, UnitTypeId.KilometersPerSecondSquared);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INCHES_PER_SECOND_SQUARED, UnitTypeId.InchesPerSecondSquared);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_PER_SECOND_SQUARED, UnitTypeId.FeetPerSecondSquared);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILES_PER_SECOND_SQUARED, UnitTypeId.MilesPerSecondSquared);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_TO_THE_FOURTH_POWER, UnitTypeId.FeetToTheFourthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INCHES_TO_THE_FOURTH_POWER, UnitTypeId.InchesToTheFourthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILLIMETERS_TO_THE_FOURTH_POWER, UnitTypeId.MillimetersToTheFourthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CENTIMETERS_TO_THE_FOURTH_POWER, UnitTypeId.CentimetersToTheFourthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS_TO_THE_FOURTH_POWER, UnitTypeId.MetersToTheFourthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_FEET_TO_THE_SIXTH_POWER, UnitTypeId.FeetToTheSixthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_INCHES_TO_THE_SIXTH_POWER, UnitTypeId.InchesToTheSixthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILLIMETERS_TO_THE_SIXTH_POWER, UnitTypeId.MillimetersToTheSixthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CENTIMETERS_TO_THE_SIXTH_POWER, UnitTypeId.CentimetersToTheSixthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_METERS_TO_THE_SIXTH_POWER, UnitTypeId.MetersToTheSixthPower);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_FEET_PER_FOOT, UnitTypeId.SquareFeetPerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_INCHES_PER_FOOT, UnitTypeId.SquareInchesPerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_MILLIMETERS_PER_METER, UnitTypeId.SquareMillimetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_CENTIMETERS_PER_METER, UnitTypeId.SquareCentimetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SQUARE_METERS_PER_METER, UnitTypeId.SquareMetersPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_MASS_PER_METER, UnitTypeId.KilogramsPerMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS_PER_FOOT, UnitTypeId.PoundsMassPerFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RADIANS, UnitTypeId.Radians);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_GRADS, UnitTypeId.Gradians);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RADIANS_PER_SECOND, UnitTypeId.RadiansPerSecond);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILISECONDS, UnitTypeId.Milliseconds);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_SECONDS, UnitTypeId.Seconds);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MINUTES, UnitTypeId.Minutes);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_HOURS, UnitTypeId.Hours);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOMETERS_PER_HOUR, UnitTypeId.KilometersPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILES_PER_HOUR, UnitTypeId.MilesPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOJOULES, UnitTypeId.Kilojoules);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_KILOGRAMS_MASS_PER_SQUARE_METER, UnitTypeId.KilogramsPerSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_POUNDS_MASS_PER_SQUARE_FOOT, UnitTypeId.PoundsMassPerSquareFoot);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_WATTS_PER_METER_KELVIN, UnitTypeId.WattsPerMeterKelvin);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_JOULES_PER_GRAM_CELSIUS, UnitTypeId.JoulesPerGramDegreeCelsius);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_JOULES_PER_GRAM, UnitTypeId.JoulesPerGram);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_NANOGRAMS_PER_PASCAL_SECOND_SQUARE_METER, UnitTypeId.NanogramsPerPascalSecondSquareMeter);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_OHM_METERS, UnitTypeId.OhmMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_HOUR_FOOT_FAHRENHEIT, UnitTypeId.BritishThermalUnitsPerHourSquareFootDegreeFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_POUND_FAHRENHEIT, UnitTypeId.BritishThermalUnitsPerPoundDegreeFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_BRITISH_THERMAL_UNITS_PER_POUND, UnitTypeId.BritishThermalUnitsPerPound);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_GRAINS_PER_HOUR_SQUARE_FOOT_INCH_MERCURY, UnitTypeId.GrainsPerHourSquareFootInchMercury);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_PER_MILLE, UnitTypeId.PerMille);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECIMETERS, UnitTypeId.Decimeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_JOULES_PER_KILOGRAM_CELSIUS, UnitTypeId.JoulesPerKilogramDegreeCelsius);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MICROMETERS_PER_METER_CELSIUS, UnitTypeId.MicrometersPerMeterDegreeCelsius);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MICROINCHES_PER_INCH_FAHRENHEIT, UnitTypeId.MicroinchesPerInchDegreeFahrenheit);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_USTONNES_MASS, UnitTypeId.UsTonnesMass);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_USTONNES_FORCE, UnitTypeId.UsTonnesForce);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS_PER_MINUTE, UnitTypeId.LitersPerMinute);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_STATIONING_METERS, UnitTypeId.StationingMeters);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_STATIONING_FEET, UnitTypeId.StationingFeet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_CUBIC_FEET_PER_HOUR, UnitTypeId.CubicFeetPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_LITERS_PER_HOUR, UnitTypeId.LitersPerHour);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_RATIO_TO_1, UnitTypeId.RatioTo1);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_DECIMAL_US_SURVEY_FEET, UnitTypeId.UsSurveyFeet);
                ForgeTypeDisplayunitMap.Add(HLDisplayUnitTypeEnum.DUT_MILLIMETERS, UnitTypeId.Millimeters);
            }


            if (ForgeTypeDisplayunitMap.ContainsKey(displayType)) return ForgeTypeDisplayunitMap[displayType];

            throw new NoForgeTypeFoundException("No type found for: " + displayType.ToString());

        }

#endif

    }
}
