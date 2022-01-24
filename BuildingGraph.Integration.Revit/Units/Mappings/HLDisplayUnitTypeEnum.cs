using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Integration.Revit
{
    public enum HLDisplayUnitTypeEnum
    {
        DUT_UNDEFINED = -2,
        DUT_CUSTOM = -1,
        DUT_METERS = 0,
        DUT_CENTIMETERS = 1,
        DUT_MILLIMETERS = 2,
        DUT_DECIMAL_FEET = 3,
        DUT_FEET_FRACTIONAL_INCHES = 4,
        DUT_FRACTIONAL_INCHES = 5,
        DUT_DECIMAL_INCHES = 6,
        DUT_ACRES = 7,
        DUT_HECTARES = 8,
        DUT_METERS_CENTIMETERS = 9,
        DUT_CUBIC_YARDS = 10,
        DUT_SQUARE_FEET = 11,
        DUT_SQUARE_METERS = 12,
        DUT_CUBIC_FEET = 13,
        DUT_CUBIC_METERS = 14,
        DUT_DECIMAL_DEGREES = 15,
        DUT_DEGREES_AND_MINUTES = 16,
        DUT_GENERAL = 17,
        DUT_FIXED = 18,
        DUT_PERCENTAGE = 19,
        DUT_SQUARE_INCHES = 20,
        DUT_SQUARE_CENTIMETERS = 21,
        DUT_SQUARE_MILLIMETERS = 22,
        DUT_CUBIC_INCHES = 23,
        DUT_CUBIC_CENTIMETERS = 24,
        DUT_CUBIC_MILLIMETERS = 25,
        //
        // Summary:
        //     liter (L)
        DUT_LITERS = 26,
        //
        // Summary:
        //     gallon (U.S.) (gal)
        DUT_GALLONS_US = 27,
        //
        // Summary:
        //     kilogram per cubic meter (kg/mÂ³)
        DUT_KILOGRAMS_PER_CUBIC_METER = 28,
        //
        // Summary:
        //     pound per cubic foot (lb/ftÂ³)
        DUT_POUNDS_MASS_PER_CUBIC_FOOT = 29,
        //
        // Summary:
        //     pound per cubic inch (lb/inÂ³)
        DUT_POUNDS_MASS_PER_CUBIC_INCH = 30,
        //
        // Summary:
        //     British thermal unit[IT] (Btu[IT])
        DUT_BRITISH_THERMAL_UNITS = 31,
        //
        // Summary:
        //     calorie[IT] (cal[IT])
        DUT_CALORIES = 32,
        //
        // Summary:
        //     kilocalorie[IT] (kcal[IT])
        DUT_KILOCALORIES = 33,
        //
        // Summary:
        //     joule (J)
        DUT_JOULES = 34,
        //
        // Summary:
        //     kilowatt hour (kW Â· h)
        DUT_KILOWATT_HOURS = 35,
        //
        // Summary:
        //     therm (EC)
        DUT_THERMS = 36,
        //
        // Summary:
        //     Inches of water per 100 feet
        DUT_INCHES_OF_WATER_PER_100FT = 37,
        //
        // Summary:
        //     pascal per meter (N/m)
        DUT_PASCALS_PER_METER = 38,
        //
        // Summary:
        //     watt (W)
        DUT_WATTS = 39,
        //
        // Summary:
        //     kilowatt (kW)
        DUT_KILOWATTS = 40,
        //
        // Summary:
        //     British thermal unit[IT] per second (Btu[IT]/s)
        DUT_BRITISH_THERMAL_UNITS_PER_SECOND = 41,
        //
        // Summary:
        //     British thermal unit[IT] per hour (Btu[IT]/h)
        DUT_BRITISH_THERMAL_UNITS_PER_HOUR = 42,
        //
        // Summary:
        //     calorie[IT] per second (cal[IT]/s)
        DUT_CALORIES_PER_SECOND = 43,
        //
        // Summary:
        //     kilocalorie[IT] per second (kcal[IT]/s)
        DUT_KILOCALORIES_PER_SECOND = 44,
        //
        // Summary:
        //     watt per square foot (W/ftÂ²)
        DUT_WATTS_PER_SQUARE_FOOT = 45,
        //
        // Summary:
        //     watt per square meter (W/mÂ²)
        DUT_WATTS_PER_SQUARE_METER = 46,
        //
        // Summary:
        //     inch of water (60.8Â°F)
        DUT_INCHES_OF_WATER = 47,
        //
        // Summary:
        //     pascal (Pa)
        DUT_PASCALS = 48,
        //
        // Summary:
        //     kilopascal (kPa)
        DUT_KILOPASCALS = 49,
        //
        // Summary:
        //     megapascal (MPa)
        DUT_MEGAPASCALS = 50,
        //
        // Summary:
        //     pound-force per square inch (psi) (lbf/in2)
        DUT_POUNDS_FORCE_PER_SQUARE_INCH = 51,
        //
        // Summary:
        //     inch of mercury conventional (inHg)
        DUT_INCHES_OF_MERCURY = 52,
        //
        // Summary:
        //     millimeter of mercury conventional (mmHg)
        DUT_MILLIMETERS_OF_MERCURY = 53,
        //
        // Summary:
        //     atmosphere standard (atm)
        DUT_ATMOSPHERES = 54,
        //
        // Summary:
        //     bar (bar)
        DUT_BARS = 55,
        //
        // Summary:
        //     degree Fahrenheit (Â°F)
        DUT_FAHRENHEIT = 56,
        //
        // Summary:
        //     degree Celsius (Â°C)
        DUT_CELSIUS = 57,
        //
        // Summary:
        //     kelvin (K)
        DUT_KELVIN = 58,
        //
        // Summary:
        //     degree Rankine (Â°R)
        DUT_RANKINE = 59,
        //
        // Summary:
        //     foot per minute (ft/min)
        DUT_FEET_PER_MINUTE = 60,
        //
        // Summary:
        //     meter per second (m/s)
        DUT_METERS_PER_SECOND = 61,
        //
        // Summary:
        //     centimeter per minute (cm/min)
        DUT_CENTIMETERS_PER_MINUTE = 62,
        //
        // Summary:
        //     cubic foot per minute (ftÂ³/min)
        DUT_CUBIC_FEET_PER_MINUTE = 63,
        //
        // Summary:
        //     liter per second (L/s)
        DUT_LITERS_PER_SECOND = 64,
        //
        // Summary:
        //     cubic meter per second (mÂ³/s)
        DUT_CUBIC_METERS_PER_SECOND = 65,
        //
        // Summary:
        //     cubic meters per hour (mÂ³/h)
        DUT_CUBIC_METERS_PER_HOUR = 66,
        //
        // Summary:
        //     gallon (U.S.) per minute (gpm) (gal/min)
        DUT_GALLONS_US_PER_MINUTE = 67,
        //
        // Summary:
        //     gallon (U.S.) per hour (gph) (gal/h)
        DUT_GALLONS_US_PER_HOUR = 68,
        //
        // Summary:
        //     ampere (A)
        DUT_AMPERES = 69,
        //
        // Summary:
        //     kiloampere (kA)
        DUT_KILOAMPERES = 70,
        //
        // Summary:
        //     milliampere (mA)
        DUT_MILLIAMPERES = 71,
        //
        // Summary:
        //     volt (V)
        DUT_VOLTS = 72,
        //
        // Summary:
        //     kilovolt (kV)
        DUT_KILOVOLTS = 73,
        //
        // Summary:
        //     millivolt (mV)
        DUT_MILLIVOLTS = 74,
        //
        // Summary:
        //     hertz (Hz)
        DUT_HERTZ = 75,
        DUT_CYCLES_PER_SECOND = 76,
        //
        // Summary:
        //     lux (lx)
        DUT_LUX = 77,
        //
        // Summary:
        //     footcandle
        DUT_FOOTCANDLES = 78,
        //
        // Summary:
        //     footlambert
        DUT_FOOTLAMBERTS = 79,
        //
        // Summary:
        //     candela per square meter (cd/mÂ²)
        DUT_CANDELAS_PER_SQUARE_METER = 80,
        //
        // Summary:
        //     candela (cd)
        DUT_CANDELAS = 81,
        //
        // Summary:
        //     obsolete
        DUT_CANDLEPOWER = 82,
        //
        // Summary:
        //     lumen (lm)
        DUT_LUMENS = 83,
        DUT_VOLT_AMPERES = 84,
        DUT_KILOVOLT_AMPERES = 85,
        //
        // Summary:
        //     horsepower (550 ft Â· lbf/s)
        DUT_HORSEPOWER = 86,
        DUT_NEWTONS = 87,
        DUT_DECANEWTONS = 88,
        DUT_KILONEWTONS = 89,
        DUT_MEGANEWTONS = 90,
        DUT_KIPS = 91,
        DUT_KILOGRAMS_FORCE = 92,
        DUT_TONNES_FORCE = 93,
        DUT_POUNDS_FORCE = 94,
        DUT_NEWTONS_PER_METER = 95,
        DUT_DECANEWTONS_PER_METER = 96,
        DUT_KILONEWTONS_PER_METER = 97,
        DUT_MEGANEWTONS_PER_METER = 98,
        DUT_KIPS_PER_FOOT = 99,
        DUT_KILOGRAMS_FORCE_PER_METER = 100,
        DUT_TONNES_FORCE_PER_METER = 101,
        DUT_POUNDS_FORCE_PER_FOOT = 102,
        DUT_NEWTONS_PER_SQUARE_METER = 103,
        DUT_DECANEWTONS_PER_SQUARE_METER = 104,
        DUT_KILONEWTONS_PER_SQUARE_METER = 105,
        DUT_MEGANEWTONS_PER_SQUARE_METER = 106,
        DUT_KIPS_PER_SQUARE_FOOT = 107,
        DUT_KILOGRAMS_FORCE_PER_SQUARE_METER = 108,
        DUT_TONNES_FORCE_PER_SQUARE_METER = 109,
        DUT_POUNDS_FORCE_PER_SQUARE_FOOT = 110,
        DUT_NEWTON_METERS = 111,
        DUT_DECANEWTON_METERS = 112,
        DUT_KILONEWTON_METERS = 113,
        DUT_MEGANEWTON_METERS = 114,
        DUT_KIP_FEET = 115,
        DUT_KILOGRAM_FORCE_METERS = 116,
        DUT_TONNE_FORCE_METERS = 117,
        DUT_POUND_FORCE_FEET = 118,
        DUT_METERS_PER_KILONEWTON = 119,
        DUT_FEET_PER_KIP = 120,
        DUT_SQUARE_METERS_PER_KILONEWTON = 121,
        DUT_SQUARE_FEET_PER_KIP = 122,
        DUT_CUBIC_METERS_PER_KILONEWTON = 123,
        DUT_CUBIC_FEET_PER_KIP = 124,
        DUT_INV_KILONEWTONS = 125,
        DUT_INV_KIPS = 126,
        //
        // Summary:
        //     foot of water conventional (ftH2O) per 100 ft
        DUT_FEET_OF_WATER_PER_100FT = 127,
        //
        // Summary:
        //     foot of water conventional (ftH2O)
        DUT_FEET_OF_WATER = 128,
        //
        // Summary:
        //     pascal second (Pa Â· s)
        DUT_PASCAL_SECONDS = 129,
        //
        // Summary:
        //     pound per foot second (lb/(ft Â· s))
        DUT_POUNDS_MASS_PER_FOOT_SECOND = 130,
        //
        // Summary:
        //     centipoise (cP)
        DUT_CENTIPOISES = 131,
        //
        // Summary:
        //     foot per second (ft/s)
        DUT_FEET_PER_SECOND = 132,
        DUT_KIPS_PER_SQUARE_INCH = 133,
        //
        // Summary:
        //     kilnewtons per cubic meter (kN/mÂ³)
        DUT_KILONEWTONS_PER_CUBIC_METER = 134,
        //
        // Summary:
        //     pound per cubic foot (kip/ftÂ³)
        DUT_POUNDS_FORCE_PER_CUBIC_FOOT = 135,
        //
        // Summary:
        //     pound per cubic foot (kip/inÂ³)
        DUT_KIPS_PER_CUBIC_INCH = 136,
        DUT_INV_FAHRENHEIT = 137,
        DUT_INV_CELSIUS = 138,
        DUT_NEWTON_METERS_PER_METER = 139,
        DUT_DECANEWTON_METERS_PER_METER = 140,
        DUT_KILONEWTON_METERS_PER_METER = 141,
        DUT_MEGANEWTON_METERS_PER_METER = 142,
        DUT_KIP_FEET_PER_FOOT = 143,
        DUT_KILOGRAM_FORCE_METERS_PER_METER = 144,
        DUT_TONNE_FORCE_METERS_PER_METER = 145,
        DUT_POUND_FORCE_FEET_PER_FOOT = 146,
        //
        // Summary:
        //     pound per foot hour (lb/(ft Â· h))
        DUT_POUNDS_MASS_PER_FOOT_HOUR = 147,
        DUT_KIPS_PER_INCH = 148,
        //
        // Summary:
        //     pound per cubic foot (kip/ftÂ³)
        DUT_KIPS_PER_CUBIC_FOOT = 149,
        DUT_KIP_FEET_PER_DEGREE = 150,
        DUT_KILONEWTON_METERS_PER_DEGREE = 151,
        DUT_KIP_FEET_PER_DEGREE_PER_FOOT = 152,
        DUT_KILONEWTON_METERS_PER_DEGREE_PER_METER = 153,
        //
        // Summary:
        //     watt per square meter kelvin (W/(mÂ² Â· K))
        DUT_WATTS_PER_SQUARE_METER_KELVIN = 154,
        //
        // Summary:
        //     British thermal unit[IT] per hour square foot degree Fahrenheit (Btu[IT]/(h Â·
        //     ftÂ² Â· Â°F)
        DUT_BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT_FAHRENHEIT = 155,
        //
        // Summary:
        //     cubic foot per minute square foot
        DUT_CUBIC_FEET_PER_MINUTE_SQUARE_FOOT = 156,
        //
        // Summary:
        //     liter per second square meter
        DUT_LITERS_PER_SECOND_SQUARE_METER = 157,
        DUT_RATIO_10 = 158,
        DUT_RATIO_12 = 159,
        DUT_SLOPE_DEGREES = 160,
        DUT_RISE_OVER_INCHES = 161,
        DUT_RISE_OVER_FOOT = 162,
        DUT_RISE_OVER_MMS = 163,
        //
        // Summary:
        //     watt per cubic foot (W/mÂ³)
        DUT_WATTS_PER_CUBIC_FOOT = 164,
        //
        // Summary:
        //     watt per cubic meter (W/mÂ³)
        DUT_WATTS_PER_CUBIC_METER = 165,
        //
        // Summary:
        //     British thermal unit[IT] per hour square foot (Btu[IT]/(h Â· ftÂ²)
        DUT_BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT = 166,
        //
        // Summary:
        //     British thermal unit[IT] per hour cubic foot (Btu[IT]/(h Â· ftÂ³)
        DUT_BRITISH_THERMAL_UNITS_PER_HOUR_CUBIC_FOOT = 167,
        //
        // Summary:
        //     Ton of refrigeration (12 000 Btu[IT]/h)
        DUT_TON_OF_REFRIGERATION = 168,
        //
        // Summary:
        //     cubic foot per minute cubic foot
        DUT_CUBIC_FEET_PER_MINUTE_CUBIC_FOOT = 169,
        //
        // Summary:
        //     liter per second cubic meter
        DUT_LITERS_PER_SECOND_CUBIC_METER = 170,
        //
        // Summary:
        //     cubic foot per minute ton of refrigeration
        DUT_CUBIC_FEET_PER_MINUTE_TON_OF_REFRIGERATION = 171,
        //
        // Summary:
        //     liter per second kilowatt
        DUT_LITERS_PER_SECOND_KILOWATTS = 172,
        //
        // Summary:
        //     square foot per ton of refrigeration
        DUT_SQUARE_FEET_PER_TON_OF_REFRIGERATION = 173,
        //
        // Summary:
        //     square meter per kilowatt
        DUT_SQUARE_METERS_PER_KILOWATTS = 174,
        DUT_CURRENCY = 175,
        DUT_LUMENS_PER_WATT = 176,
        //
        // Summary:
        //     square foot per thousand British thermal unit[IT] per hour
        DUT_SQUARE_FEET_PER_THOUSAND_BRITISH_THERMAL_UNITS_PER_HOUR = 177,
        DUT_KILONEWTONS_PER_SQUARE_CENTIMETER = 178,
        DUT_NEWTONS_PER_SQUARE_MILLIMETER = 179,
        DUT_KILONEWTONS_PER_SQUARE_MILLIMETER = 180,
        DUT_RISE_OVER_120_INCHES = 181,
        DUT_1_RATIO = 182,
        DUT_RISE_OVER_10_FEET = 183,
        DUT_HOUR_SQUARE_FOOT_FAHRENHEIT_PER_BRITISH_THERMAL_UNIT = 184,
        DUT_SQUARE_METER_KELVIN_PER_WATT = 185,
        DUT_BRITISH_THERMAL_UNIT_PER_FAHRENHEIT = 186,
        DUT_JOULES_PER_KELVIN = 187,
        DUT_KILOJOULES_PER_KELVIN = 188,
        //
        // Summary:
        //     kilograms (kg)
        DUT_KILOGRAMS_MASS = 189,
        //
        // Summary:
        //     tonnes (t)
        DUT_TONNES_MASS = 190,
        //
        // Summary:
        //     pounds (lb)
        DUT_POUNDS_MASS = 191,
        //
        // Summary:
        //     meters per second squared (m/sÂ²)
        DUT_METERS_PER_SECOND_SQUARED = 192,
        //
        // Summary:
        //     kilometers per second squared (km/sÂ²)
        DUT_KILOMETERS_PER_SECOND_SQUARED = 193,
        //
        // Summary:
        //     inches per second squared (in/sÂ²)
        DUT_INCHES_PER_SECOND_SQUARED = 194,
        //
        // Summary:
        //     feet per second squared (ft/sÂ²)
        DUT_FEET_PER_SECOND_SQUARED = 195,
        //
        // Summary:
        //     miles per second squared (mi/sÂ²)
        DUT_MILES_PER_SECOND_SQUARED = 196,
        //
        // Summary:
        //     feet to the fourth power (ft^4)
        DUT_FEET_TO_THE_FOURTH_POWER = 197,
        //
        // Summary:
        //     inches to the fourth power (in^4)
        DUT_INCHES_TO_THE_FOURTH_POWER = 198,
        //
        // Summary:
        //     millimeters to the fourth power (mm^4)
        DUT_MILLIMETERS_TO_THE_FOURTH_POWER = 199,
        //
        // Summary:
        //     centimeters to the fourth power (cm^4)
        DUT_CENTIMETERS_TO_THE_FOURTH_POWER = 200,
        //
        // Summary:
        //     Meters to the fourth power (m^4)
        DUT_METERS_TO_THE_FOURTH_POWER = 201,
        //
        // Summary:
        //     feet to the sixth power (ft^6)
        DUT_FEET_TO_THE_SIXTH_POWER = 202,
        //
        // Summary:
        //     inches to the sixth power (in^6)
        DUT_INCHES_TO_THE_SIXTH_POWER = 203,
        //
        // Summary:
        //     millimeters to the sixth power (mm^6)
        DUT_MILLIMETERS_TO_THE_SIXTH_POWER = 204,
        //
        // Summary:
        //     centimeters to the sixth power (cm^6)
        DUT_CENTIMETERS_TO_THE_SIXTH_POWER = 205,
        //
        // Summary:
        //     meters to the sixth power (m^6)
        DUT_METERS_TO_THE_SIXTH_POWER = 206,
        //
        // Summary:
        //     square feet per foot (ftÂ²/ft)
        DUT_SQUARE_FEET_PER_FOOT = 207,
        //
        // Summary:
        //     square inches per foot (inÂ²/ft)
        DUT_SQUARE_INCHES_PER_FOOT = 208,
        //
        // Summary:
        //     square millimeters per meter (mmÂ²/m)
        DUT_SQUARE_MILLIMETERS_PER_METER = 209,
        //
        // Summary:
        //     square centimeters per meter (cmÂ²/m)
        DUT_SQUARE_CENTIMETERS_PER_METER = 210,
        //
        // Summary:
        //     square meters per meter (mÂ²/m)
        DUT_SQUARE_METERS_PER_METER = 211,
        //
        // Summary:
        //     kilograms per meter (kg/m)
        DUT_KILOGRAMS_MASS_PER_METER = 212,
        //
        // Summary:
        //     pounds per foot (lb/ft)
        DUT_POUNDS_MASS_PER_FOOT = 213,
        //
        // Summary:
        //     radians
        DUT_RADIANS = 214,
        //
        // Summary:
        //     grads
        DUT_GRADS = 215,
        //
        // Summary:
        //     radians per second
        DUT_RADIANS_PER_SECOND = 216,
        //
        // Summary:
        //     millisecond
        DUT_MILISECONDS = 217,
        //
        // Summary:
        //     second
        DUT_SECONDS = 218,
        //
        // Summary:
        //     minutes
        DUT_MINUTES = 219,
        //
        // Summary:
        //     hours
        DUT_HOURS = 220,
        //
        // Summary:
        //     kilometers per hour
        DUT_KILOMETERS_PER_HOUR = 221,
        //
        // Summary:
        //     miles per hour
        DUT_MILES_PER_HOUR = 222,
        //
        // Summary:
        //     kilojoules
        DUT_KILOJOULES = 223,
        //
        // Summary:
        //     kilograms per square meter (kg/mÂ²)
        DUT_KILOGRAMS_MASS_PER_SQUARE_METER = 224,
        //
        // Summary:
        //     pounds per square foot (lb/ftÂ²)
        DUT_POUNDS_MASS_PER_SQUARE_FOOT = 225,
        //
        // Summary:
        //     Watts per meter kelvin (W/(mÂ·K))
        DUT_WATTS_PER_METER_KELVIN = 226,
        //
        // Summary:
        //     Joules per gram Celsius (J/(gÂ·Â°C))
        DUT_JOULES_PER_GRAM_CELSIUS = 227,
        //
        // Summary:
        //     Joules per gram (J/g)
        DUT_JOULES_PER_GRAM = 228,
        //
        // Summary:
        //     Nanograms per pascal second square meter (ng/(PaÂ·sÂ·mÂ²))
        DUT_NANOGRAMS_PER_PASCAL_SECOND_SQUARE_METER = 229,
        //
        // Summary:
        //     Ohm meters
        DUT_OHM_METERS = 230,
        //
        // Summary:
        //     BTU per hour foot Fahrenheit (BTU/(hÂ·ftÂ·Â°F))
        DUT_BRITISH_THERMAL_UNITS_PER_HOUR_FOOT_FAHRENHEIT = 231,
        //
        // Summary:
        //     BTU per pound Fahrenheit (BTU/(lbÂ·Â°F))
        DUT_BRITISH_THERMAL_UNITS_PER_POUND_FAHRENHEIT = 232,
        //
        // Summary:
        //     BTU per pound (BTU/lb)
        DUT_BRITISH_THERMAL_UNITS_PER_POUND = 233,
        //
        // Summary:
        //     Grains per hour square foot inch mercury (gr/(hÂ·ftÂ²Â·inHg))
        DUT_GRAINS_PER_HOUR_SQUARE_FOOT_INCH_MERCURY = 234,
        //
        // Summary:
        //     Per mille or per thousand(â€°)
        DUT_PER_MILLE = 235,
        //
        // Summary:
        //     Decimeters
        DUT_DECIMETERS = 236,
        //
        // Summary:
        //     Joules per kilogram Celsius (J/(kgÂ·Â°C))
        DUT_JOULES_PER_KILOGRAM_CELSIUS = 237,
        //
        // Summary:
        //     Micrometers per meter Celsius (um/(mÂ·Â°C))
        DUT_MICROMETERS_PER_METER_CELSIUS = 238,
        //
        // Summary:
        //     Microinches per inch Fahrenheit (uin/(inÂ·Â°F))
        DUT_MICROINCHES_PER_INCH_FAHRENHEIT = 239,
        //
        // Summary:
        //     US tonnes (T, Tons, ST)
        DUT_USTONNES_MASS = 240,
        //
        // Summary:
        //     US tonnes (Tonsf, STf)
        DUT_USTONNES_FORCE = 241,
        //
        // Summary:
        //     liters per minute (L/min)
        DUT_LITERS_PER_MINUTE = 242,
        //
        // Summary:
        //     degree Fahrenheit difference (delta Â°F)
        DUT_FAHRENHEIT_DIFFERENCE = 243,
        //
        // Summary:
        //     degree Celsius difference (delta Â°C)
        DUT_CELSIUS_DIFFERENCE = 244,
        //
        // Summary:
        //     kelvin difference (delta K)
        DUT_KELVIN_DIFFERENCE = 245,
        //
        // Summary:
        //     degree Rankine difference (delta Â°R)
        DUT_RANKINE_DIFFERENCE = 246,
        //
        // Summary:
        //     stationing meters value (XXX+YYY.ZZZ (base is always 1000 meters, YYY is always
        //     3 digits) 1024.555 = 1+024.555)
        DUT_STATIONING_METERS = 247,
        //
        // Summary:
        //     stationing feet value (XXX+YY.ZZZ Decimal Feet (base is always 100 feet, YY is
        //     always 2 digits) 1024.555 = 10+24.555)
        DUT_STATIONING_FEET = 248,
        //
        // Summary:
        //     cubic feet per hour
        DUT_CUBIC_FEET_PER_HOUR = 249,
        //
        // Summary:
        //     liters per hour
        DUT_LITERS_PER_HOUR = 250,
        //
        // Summary:
        //     ratio to 1
        DUT_RATIO_TO_1 = 251,
        DUT_DECIMAL_US_SURVEY_FEET = 605
    }
}
