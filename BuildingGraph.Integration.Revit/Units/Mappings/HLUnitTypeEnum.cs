using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Integration.Revit
{
    public enum HLUnitTypeEnum
    {
        //
        // Summary:
        //     Undefined unit value
        UT_Undefined = -2,
        //
        // Summary:
        //     A custom unit value
        UT_Custom = -1,
        //
        // Summary:
        //     Length, e.g. ft, in, m, mm
        UT_Length = 0,
        //
        // Summary:
        //     Area, e.g. ftÂ², inÂ², mÂ², mmÂ²
        UT_Area = 1,
        //
        // Summary:
        //     Volume, e.g. ftÂ³, inÂ³, mÂ³, mmÂ³
        UT_Volume = 2,
        //
        // Summary:
        //     Angular measurement, e.g. radians, degrees
        UT_Angle = 3,
        //
        // Summary:
        //     General format unit, appropriate for general counts or percentages
        UT_Number = 4,
        //
        // Summary:
        //     Sheet length
        UT_SheetLength = 5,
        //
        // Summary:
        //     Site angle
        UT_SiteAngle = 6,
        //
        // Summary:
        //     Density (HVAC) e.g. kg/mÂ³
        UT_HVAC_Density = 7,
        //
        // Summary:
        //     Energy (HVAC) e.g. (mÂ² Â· kg)/sÂ², J
        UT_HVAC_Energy = 8,
        //
        // Summary:
        //     Friction (HVAC) e.g. kg/(mÂ² Â· sÂ²), Pa/m
        UT_HVAC_Friction = 9,
        //
        // Summary:
        //     Power (HVAC) e.g. (mÂ² Â· kg)/sÂ³, W
        UT_HVAC_Power = 10,
        //
        // Summary:
        //     Power Density (HVAC), e.g. kg/sÂ³, W/mÂ²
        UT_HVAC_Power_Density = 11,
        //
        // Summary:
        //     Pressure (HVAC) e.g. kg/(m Â· sÂ²), Pa
        UT_HVAC_Pressure = 12,
        //
        // Summary:
        //     Temperature (HVAC) e.g. K, C, F
        UT_HVAC_Temperature = 13,
        //
        // Summary:
        //     Velocity (HVAC) e.g. m/s
        UT_HVAC_Velocity = 14,
        //
        // Summary:
        //     Air Flow (HVAC) e.g. mÂ³/s
        UT_HVAC_Airflow = 15,
        //
        // Summary:
        //     Duct Size (HVAC) e.g. mm, in
        UT_HVAC_DuctSize = 16,
        //
        // Summary:
        //     Cross Section (HVAC) e.g. mmÂ², inÂ²
        UT_HVAC_CrossSection = 17,
        //
        // Summary:
        //     Heat Gain (HVAC) e.g. (mÂ² Â· kg)/sÂ³, W
        UT_HVAC_HeatGain = 18,
        //
        // Summary:
        //     Current (Electrical) e.g. A
        UT_Electrical_Current = 19,
        //
        // Summary:
        //     Electrical Potential e.g. (mÂ² Â· kg) / (sÂ³Â· A), V
        UT_Electrical_Potential = 20,
        //
        // Summary:
        //     Frequency (Electrical) e.g. 1/s, Hz
        UT_Electrical_Frequency = 21,
        //
        // Summary:
        //     Illuminance (Electrical) e.g. (cd Â· sr)/mÂ², lm/mÂ²
        UT_Electrical_Illuminance = 22,
        //
        // Summary:
        //     Luminous Flux (Electrical) e.g. cd Â· sr, lm
        UT_Electrical_Luminous_Flux = 23,
        //
        // Summary:
        //     Power (Electrical) e.g. (mÂ² Â· kg)/sÂ³, W
        UT_Electrical_Power = 24,
        //
        // Summary:
        //     Roughness factor (HVAC) e,g. ft, in, mm
        UT_HVAC_Roughness = 25,
        //
        // Summary:
        //     Force, e.g. (kg Â· m)/sÂ², N
        UT_Force = 26,
        //
        // Summary:
        //     Force per unit length, e.g. kg/sÂ², N/m
        UT_LinearForce = 27,
        //
        // Summary:
        //     Force per unit area, e.g. kg/(m Â· sÂ²), N/mÂ²
        UT_AreaForce = 28,
        //
        // Summary:
        //     Moment, e.g. (kg Â· mÂ²)/sÂ², N Â· m
        UT_Moment = 29,
        //
        // Summary:
        //     Force scale, e.g. m / N
        UT_ForceScale = 30,
        //
        // Summary:
        //     Linear force scale, e.g. mÂ² / N
        UT_LinearForceScale = 31,
        //
        // Summary:
        //     Area force scale, e.g. mÂ³ / N
        UT_AreaForceScale = 32,
        //
        // Summary:
        //     Moment scale, e.g. 1 / N
        UT_MomentScale = 33,
        //
        // Summary:
        //     Apparent Power (Electrical), e.g. (mÂ² Â· kg)/sÂ³, W
        UT_Electrical_Apparent_Power = 34,
        //
        // Summary:
        //     Power Density (Electrical), e.g. kg/sÂ³, W/mÂ²
        UT_Electrical_Power_Density = 35,
        //
        // Summary:
        //     Density (Piping) e.g. kg/mÂ³
        UT_Piping_Density = 36,
        //
        // Summary:
        //     Flow (Piping), e.g. mÂ³/s
        UT_Piping_Flow = 37,
        //
        // Summary:
        //     Friction (Piping), e.g. kg/(mÂ² Â· sÂ²), Pa/m
        UT_Piping_Friction = 38,
        //
        // Summary:
        //     Pressure (Piping), e.g. kg/(m Â· sÂ²), Pa
        UT_Piping_Pressure = 39,
        //
        // Summary:
        //     Temperature (Piping), e.g. K
        UT_Piping_Temperature = 40,
        //
        // Summary:
        //     Velocity (Piping), e.g. m/s
        UT_Piping_Velocity = 41,
        //
        // Summary:
        //     Dynamic Viscosity (Piping), e.g. kg/(m Â· s), Pa Â· s
        UT_Piping_Viscosity = 42,
        //
        // Summary:
        //     Pipe Size (Piping), e.g. m
        UT_PipeSize = 43,
        //
        // Summary:
        //     Roughness factor (Piping), e.g. ft, in, mm
        UT_Piping_Roughness = 44,
        //
        // Summary:
        //     Stress, e.g. kg/(m Â· sÂ²), ksi, MPa
        UT_Stress = 45,
        //
        // Summary:
        //     Unit weight, e.g. N/mÂ³
        UT_UnitWeight = 46,
        //
        // Summary:
        //     Thermal expansion, e.g. 1/K
        UT_ThermalExpansion = 47,
        //
        // Summary:
        //     Linear moment, e,g. (N Â· m)/m, lbf / ft
        UT_LinearMoment = 48,
        //
        // Summary:
        //     Linear moment scale, e.g. ft/kip, m/kN
        UT_LinearMomentScale = 49,
        //
        // Summary:
        //     Point Spring Coefficient, e.g. kg/sÂ², N/m
        UT_ForcePerLength = 50,
        //
        // Summary:
        //     Rotational Point Spring Coefficient, e.g. (kg Â· mÂ²)/(sÂ² Â· rad), (N Â· m)/rad
        UT_ForceLengthPerAngle = 51,
        //
        // Summary:
        //     Line Spring Coefficient, e.g. kg/(m Â· sÂ²), (N Â· m)/mÂ²
        UT_LinearForcePerLength = 52,
        //
        // Summary:
        //     Rotational Line Spring Coefficient, e.g. (kg Â· m)/(sÂ² Â· rad), N/rad
        UT_LinearForceLengthPerAngle = 53,
        //
        // Summary:
        //     Area Spring Coefficient, e.g. kg/(mÂ² Â· sÂ²), N/mÂ³
        UT_AreaForcePerLength = 54,
        //
        // Summary:
        //     Pipe Volume, e.g. gallons, liters
        UT_Piping_Volume = 55,
        //
        // Summary:
        //     Dynamic Viscosity (HVAC), e.g. kg/(m Â· s), Pa Â· s
        UT_HVAC_Viscosity = 56,
        //
        // Summary:
        //     Coefficient of Heat Transfer (U-value) (HVAC), e.g. kg/(sÂ³ Â· K), W/(mÂ² Â·
        //     K)
        UT_HVAC_CoefficientOfHeatTransfer = 57,
        //
        // Summary:
        //     Air Flow Density (HVAC), mÂ³/(s Â· mÂ²)
        UT_HVAC_Airflow_Density = 58,
        //
        // Summary:
        //     Slope, rise/run
        UT_Slope = 59,
        //
        // Summary:
        //     Cooling load (HVAC), e.g. (mÂ² Â· kg)/sÂ³, W, kW, Btu/s, Btu/h
        UT_HVAC_Cooling_Load = 60,
        //
        // Summary:
        //     Cooling load per unit area (HVAC), e.g. kg/sÂ³, W/mÂ², W/ftÂ², Btu/(hÂ·ftÂ²)
        UT_HVAC_Cooling_Load_Divided_By_Area = 61,
        //
        // Summary:
        //     Cooling load per unit volume (HVAC), e.g. kg/(sÂ³ Â· m), W/mÂ³, Btu/(hÂ·ftÂ³)
        UT_HVAC_Cooling_Load_Divided_By_Volume = 62,
        //
        // Summary:
        //     Heating load (HVAC), e.g. (mÂ² Â· kg)/sÂ³, W, kW, Btu/s, Btu/h
        UT_HVAC_Heating_Load = 63,
        //
        // Summary:
        //     Heating load per unit area (HVAC), e.g. kg/sÂ³, W/mÂ², W/ftÂ², Btu/(hÂ·ftÂ²)
        UT_HVAC_Heating_Load_Divided_By_Area = 64,
        //
        // Summary:
        //     Heating load per unit volume (HVAC), e.g. kg/(sÂ³ Â· m), W/mÂ³, Btu/(hÂ·ftÂ³)
        UT_HVAC_Heating_Load_Divided_By_Volume = 65,
        //
        // Summary:
        //     Airflow per unit volume (HVAC), e.g. mÂ³/(s Â· mÂ³), CFM/ftÂ³, CFM/CF, L/(sÂ·mÂ³)
        UT_HVAC_Airflow_Divided_By_Volume = 66,
        //
        // Summary:
        //     Airflow per unit cooling load (HVAC), e.g. (m Â· sÂ²)/kg, ftÂ²/ton, SF/ton, mÂ²/kW
        UT_HVAC_Airflow_Divided_By_Cooling_Load = 67,
        //
        // Summary:
        //     Area per unit cooling load (HVAC), e.g. sÂ³/kg, ftÂ²/ton, mÂ²/kW
        UT_HVAC_Area_Divided_By_Cooling_Load = 68,
        //
        // Summary:
        //     Wire Size (Electrical), e.g. mm, inch
        UT_WireSize = 69,
        //
        // Summary:
        //     Slope (HVAC)
        UT_HVAC_Slope = 70,
        //
        // Summary:
        //     Slope (Piping)
        UT_Piping_Slope = 71,
        //
        // Summary:
        //     Currency
        UT_Currency = 72,
        //
        // Summary:
        //     Electrical efficacy (lighting), e.g. cdÂ·srÂ·sÂ³/(mÂ²Â·kg), lm/W
        UT_Electrical_Efficacy = 73,
        //
        // Summary:
        //     Wattage (lighting), e.g. (mÂ² Â· kg)/sÂ³, W
        UT_Electrical_Wattage = 74,
        //
        // Summary:
        //     Color temperature (lighting), e.g. K
        UT_Color_Temperature = 75,
        //
        // Summary:
        //     Sheet length in decimal form, decimal inches, mm
        UT_DecSheetLength = 76,
        //
        // Summary:
        //     Luminous Intensity (Lighting), e.g. cd, cd
        UT_Electrical_Luminous_Intensity = 77,
        //
        // Summary:
        //     Luminance (Lighting), cd/mÂ², cd/mÂ²
        UT_Electrical_Luminance = 78,
        //
        // Summary:
        //     Area per unit heating load (HVAC), e.g. sÂ³/kg, ftÂ²/ton, mÂ²/kW
        UT_HVAC_Area_Divided_By_Heating_Load = 79,
        //
        // Summary:
        //     Heating and coooling factor, percentage
        UT_HVAC_Factor = 80,
        //
        // Summary:
        //     Temperature (electrical), e.g. F, C
        UT_Electrical_Temperature = 81,
        //
        // Summary:
        //     Cable tray size (electrical), e.g. in, mm
        UT_Electrical_CableTraySize = 82,
        //
        // Summary:
        //     Conduit size (electrical), e.g. in, mm
        UT_Electrical_ConduitSize = 83,
        //
        // Summary:
        //     Structural reinforcement volume, e.g. inÂ³, cmÂ³
        UT_Reinforcement_Volume = 84,
        //
        // Summary:
        //     Structural reinforcement length, e.g. mm, in, ft
        UT_Reinforcement_Length = 85,
        //
        // Summary:
        //     Electrical demand factor, percentage
        UT_Electrical_Demand_Factor = 86,
        //
        // Summary:
        //     Duct Insulation Thickness (HVAC), e.g. mm, in
        UT_HVAC_DuctInsulationThickness = 87,
        //
        // Summary:
        //     Duct Lining Thickness (HVAC), e.g. mm, in
        UT_HVAC_DuctLiningThickness = 88,
        //
        // Summary:
        //     Pipe Insulation Thickness (Piping), e.g. mm, in
        UT_PipeInsulationThickness = 89,
        //
        // Summary:
        //     Thermal Resistance (HVAC), R Value, e.g. mÂ²Â·K/W
        UT_HVAC_ThermalResistance = 90,
        //
        // Summary:
        //     Thermal Mass (HVAC), e.g. J/K, BTU/F
        UT_HVAC_ThermalMass = 91,
        //
        // Summary:
        //     Acceleration, e.g. m/sÂ², km/sÂ², in/sÂ², ft/sÂ², mi/sÂ²
        UT_Acceleration = 92,
        //
        // Summary:
        //     Bar Diameter, e.g. ', LF, ", m, cm, mm
        UT_Bar_Diameter = 93,
        //
        // Summary:
        //     Crack Width, e.g. ', LF, ", m, cm, mm
        UT_Crack_Width = 94,
        //
        // Summary:
        //     Displacement/Deflection, e.g. ', LF, ", m, cm, mm
        UT_Displacement_Deflection = 95,
        //
        // Summary:
        //     Energy, e.g. J, kJ, kgf-m, lb-ft, N-m
        UT_Energy = 96,
        //
        // Summary:
        //     FREQUENCY, Frequency (Structural) e.g. Hz
        UT_Structural_Frequency = 97,
        //
        // Summary:
        //     Mass, e.g. kg, lb, t
        UT_Mass = 98,
        //
        // Summary:
        //     Mass per Unit Length, e.g. kg/m, lb/ft
        UT_Mass_per_Unit_Length = 99,
        //
        // Summary:
        //     Moment of Inertia, e.g. ft^4, in^4, mm^4, cm^4, m^4
        UT_Moment_of_Inertia = 100,
        //
        // Summary:
        //     Surface Area, e.g. ftÂ²/ft, mÂ²/m
        UT_Surface_Area = 101,
        //
        // Summary:
        //     Period, e.g. ms, s, min, h
        UT_Period = 102,
        //
        // Summary:
        //     Pulsation, e.g. rad/s
        UT_Pulsation = 103,
        //
        // Summary:
        //     Reinforcement Area, e.g. SF, ftÂ², inÂ², mmÂ², cmÂ², mÂ²
        UT_Reinforcement_Area = 104,
        //
        // Summary:
        //     Reinforcement Area per Unit Length, e.g. ftÂ²/ft, inÂ²/ft, mmÂ²/m, cmÂ²/m, mÂ²/m
        UT_Reinforcement_Area_per_Unit_Length = 105,
        //
        // Summary:
        //     Reinforcement Cover, e.g. ', LF, ", m, cm, mm
        UT_Reinforcement_Cover = 106,
        //
        // Summary:
        //     Reinforcement Spacing, e.g. ', LF, ", m, cm, mm
        UT_Reinforcement_Spacing = 107,
        //
        // Summary:
        //     Rotation, e.g. Â°, rad, grad
        UT_Rotation = 108,
        //
        // Summary:
        //     Section Area, e.g. ftÂ²/ft, inÂ²/ft, mmÂ²/m, cmÂ²/m, mÂ²/m
        UT_Section_Area = 109,
        //
        // Summary:
        //     Section Dimension, e.g. ', LF, ", m, cm, mm
        UT_Section_Dimension = 110,
        //
        // Summary:
        //     Section Modulus, e.g. ft^3, in^3, mm^3, cm^3, m^3
        UT_Section_Modulus = 111,
        //
        // Summary:
        //     Section Property, e.g. ', LF, ", m, cm, mm
        UT_Section_Property = 112,
        //
        // Summary:
        //     Section Property, e.g. km/h, m/s, ft/min, ft/s, mph
        UT_Structural_Velocity = 113,
        //
        // Summary:
        //     Warping Constant, e.g. ft^6, in^6, mm^6, cm^6, m^6
        UT_Warping_Constant = 114,
        //
        // Summary:
        //     Weight, e.g. N, daN, kN, MN, kip, kgf, Tf, lb, lbf
        UT_Weight = 115,
        //
        // Summary:
        //     Weight per Unit Length, e.g. N/m, daN/m, kN/m, MN/m, kip/ft, kgf/m, Tf/m, lb/ft,
        //     lbf/ft, kip/in
        UT_Weight_per_Unit_Length = 116,
        //
        // Summary:
        //     Thermal Conductivity (HVAC), e.g. W/(mÂ·K)
        UT_HVAC_ThermalConductivity = 117,
        //
        // Summary:
        //     Specific Heat (HVAC), e.g. J/(gÂ·Â°C)
        UT_HVAC_SpecificHeat = 118,
        //
        // Summary:
        //     Specific Heat of Vaporization, e.g. J/g
        UT_HVAC_SpecificHeatOfVaporization = 119,
        //
        // Summary:
        //     Permeability, e.g. ng/(PaÂ·sÂ·mÂ²)
        UT_HVAC_Permeability = 120,
        //
        // Summary:
        //     Electrical Resistivity, e.g.
        UT_Electrical_Resistivity = 121,
        //
        // Summary:
        //     Mass Density, e.g. kg/mÂ³, lb/ftÂ³
        UT_MassDensity = 122,
        //
        // Summary:
        //     Mass Per Unit Area, e.g. kg/mÂ², lb/ftÂ²
        UT_MassPerUnitArea = 123,
        //
        // Summary:
        //     Length unit for pipe dimension, e.g. in, mm
        UT_Pipe_Dimension = 124,
        //
        // Summary:
        //     Mass, e.g. kg, lb, t
        UT_PipeMass = 125,
        //
        // Summary:
        //     Mass per Unit Length, e.g. kg/m, lb/ft
        UT_PipeMassPerUnitLength = 126,
        //
        // Summary:
        //     Temperature Difference (HVAC) e.g. C, F, K, R
        UT_HVAC_TemperatureDifference = 127,
        //
        // Summary:
        //     Temperature Difference (Piping), e.g. C, F, K, R
        UT_Piping_TemperatureDifference = 128,
        //
        // Summary:
        //     Temperature Difference (Electrical), e.g. C, F, K, R
        UT_Electrical_TemperatureDifference = 129,
        //
        // Summary:
        //     Interval of time e.g. ms, s, min, h
        UT_TimeInterval = 130,
        //
        // Summary:
        //     Distance interval over time e.g. m/h etc.
        UT_Speed = 131,
        //
        // Summary:
        //     Infrastructure Alignment stationing/distance e.g. 1+020 ft etc.
        UT_Stationing = 132
    }
}
