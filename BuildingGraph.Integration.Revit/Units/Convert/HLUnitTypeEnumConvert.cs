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


        public static HLUnitType ToHLUnitType(HLUnitTypeEnum unitType)
        {

#if REVIT2022
            var forgeType = ToForgeTypeId(unitType);
            return new HLUnitType(forgeType);
#else 

            return new HLUnitType((UnitType)(int)unitType);
#endif

        }


#if REVIT2022

        static Dictionary<HLUnitTypeEnum, ForgeTypeId> ForgeTypeMap;

        /// <summary>
        /// To be used when checking for expected or known legacy UnitType from Revit 2021 and earlier.
        /// Generally, for Revit 2022+, it recommended to obtain the ForgeTypeID
        /// using Revit's provided methods if you don't need to check 
        /// for equality with the known enums.
        /// </summary>
        /// <param name="unitType">The HLUnitType enumeration to lookup SpecId</param>
        /// <returns></returns>
        public static ForgeTypeId ToForgeTypeId(HLUnitTypeEnum unitType)
        {

            if (ForgeTypeMap == null)
            {
                ForgeTypeMap = new Dictionary<HLUnitTypeEnum, ForgeTypeId>();

                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Undefined, new ForgeTypeId());


                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Acceleration, SpecTypeId.Acceleration);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Angle, SpecTypeId.Angle);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Custom, SpecTypeId.Custom);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Length, SpecTypeId.Length);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Area, SpecTypeId.Area);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Volume, SpecTypeId.Volume);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Number, SpecTypeId.Number);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_SheetLength, SpecTypeId.SheetLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_SiteAngle, SpecTypeId.SiteAngle);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Density, SpecTypeId.HvacDensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Energy, SpecTypeId.HvacEnergy);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Friction, SpecTypeId.HvacFriction);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Power, SpecTypeId.HvacPower);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Power_Density, SpecTypeId.HvacPowerDensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Pressure, SpecTypeId.HvacPressure);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Temperature, SpecTypeId.HvacTemperature);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Velocity, SpecTypeId.HvacVelocity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Airflow, SpecTypeId.AirFlow);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_DuctSize, SpecTypeId.DuctSize);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_CrossSection, SpecTypeId.CrossSection);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_HeatGain, SpecTypeId.HeatGain);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Current, SpecTypeId.Current);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Potential, SpecTypeId.ElectricalPotential);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Frequency, SpecTypeId.ElectricalFrequency);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Illuminance, SpecTypeId.Illuminance);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Luminous_Flux, SpecTypeId.LuminousFlux);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Power, SpecTypeId.ElectricalPower);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Roughness, SpecTypeId.HvacRoughness);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Force, SpecTypeId.Force);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_LinearForce, SpecTypeId.LinearForce);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_AreaForce, SpecTypeId.AreaForce);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Moment, SpecTypeId.Moment);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_ForceScale, SpecTypeId.ForceScale);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_LinearForceScale, SpecTypeId.LinearForceScale);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_AreaForceScale, SpecTypeId.AreaForceScale);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_MomentScale, SpecTypeId.MomentScale);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Apparent_Power, SpecTypeId.ApparentPower);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Power_Density, SpecTypeId.ElectricalPowerDensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Density, SpecTypeId.PipingDensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Flow, SpecTypeId.Flow);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Friction, SpecTypeId.PipingFriction);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Pressure, SpecTypeId.PipingPressure);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Temperature, SpecTypeId.PipingTemperature);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Velocity, SpecTypeId.PipingVelocity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Viscosity, SpecTypeId.PipingViscosity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_PipeSize, SpecTypeId.PipeSize);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Roughness, SpecTypeId.PipingRoughness);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Stress, SpecTypeId.Stress);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_UnitWeight, SpecTypeId.UnitWeight);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_ThermalExpansion, SpecTypeId.ThermalExpansionCoefficient);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_LinearMoment, SpecTypeId.LinearMoment);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_LinearMomentScale, SpecTypeId.LinearMomentScale);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Volume, SpecTypeId.PipingVolume);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Viscosity, SpecTypeId.HvacViscosity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_CoefficientOfHeatTransfer, SpecTypeId.HeatTransferCoefficient);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Airflow_Density, SpecTypeId.AirFlowDensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Slope, SpecTypeId.Slope);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Cooling_Load, SpecTypeId.CoolingLoad);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Cooling_Load_Divided_By_Area, SpecTypeId.CoolingLoadDividedByArea);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Cooling_Load_Divided_By_Volume, SpecTypeId.CoolingLoadDividedByVolume);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Heating_Load, SpecTypeId.HeatingLoad);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Heating_Load_Divided_By_Area, SpecTypeId.HeatingLoadDividedByArea);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Heating_Load_Divided_By_Volume, SpecTypeId.HeatingLoadDividedByVolume);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Airflow_Divided_By_Volume, SpecTypeId.AirFlowDividedByVolume);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Airflow_Divided_By_Cooling_Load, SpecTypeId.AirFlowDividedByCoolingLoad);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Area_Divided_By_Cooling_Load, SpecTypeId.AreaDividedByCoolingLoad);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_WireSize, SpecTypeId.WireDiameter);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Slope, SpecTypeId.HvacSlope);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_Slope, SpecTypeId.PipingSlope);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Currency, SpecTypeId.Currency);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Efficacy, SpecTypeId.Efficacy);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Wattage, SpecTypeId.Wattage);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Color_Temperature, SpecTypeId.ColorTemperature);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_DecSheetLength, SpecTypeId.DecimalSheetLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Luminous_Intensity, SpecTypeId.LuminousIntensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Luminance, SpecTypeId.Luminance);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Area_Divided_By_Heating_Load, SpecTypeId.AreaDividedByHeatingLoad);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Factor, SpecTypeId.Factor);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Temperature, SpecTypeId.ElectricalTemperature);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_CableTraySize, SpecTypeId.CableTraySize);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_ConduitSize, SpecTypeId.ConduitSize);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Reinforcement_Volume, SpecTypeId.ReinforcementVolume);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Reinforcement_Length, SpecTypeId.ReinforcementLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Demand_Factor, SpecTypeId.DemandFactor);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_DuctInsulationThickness, SpecTypeId.DuctInsulationThickness);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_DuctLiningThickness, SpecTypeId.DuctLiningThickness);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_PipeInsulationThickness, SpecTypeId.PipeInsulationThickness);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_ThermalResistance, SpecTypeId.ThermalResistance);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_ThermalMass, SpecTypeId.ThermalMass);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Bar_Diameter, SpecTypeId.BarDiameter);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Crack_Width, SpecTypeId.CrackWidth);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Displacement_Deflection, SpecTypeId.Displacement);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Energy, SpecTypeId.Energy);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Structural_Frequency, SpecTypeId.StructuralFrequency);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Mass, SpecTypeId.Mass);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Mass_per_Unit_Length, SpecTypeId.MassPerUnitLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Moment_of_Inertia, SpecTypeId.MomentOfInertia);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Surface_Area, SpecTypeId.SurfaceAreaPerUnitLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Period, SpecTypeId.Period);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Pulsation, SpecTypeId.Pulsation);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Reinforcement_Area, SpecTypeId.ReinforcementArea);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Reinforcement_Area_per_Unit_Length, SpecTypeId.ReinforcementAreaPerUnitLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Reinforcement_Cover, SpecTypeId.ReinforcementCover);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Reinforcement_Spacing, SpecTypeId.ReinforcementSpacing);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Rotation, SpecTypeId.Rotation);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Section_Area, SpecTypeId.SectionArea);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Section_Dimension, SpecTypeId.SectionDimension);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Section_Modulus, SpecTypeId.SectionModulus);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Section_Property, SpecTypeId.SectionProperty);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Structural_Velocity, SpecTypeId.StructuralVelocity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Warping_Constant, SpecTypeId.WarpingConstant);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Weight, SpecTypeId.Weight);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Weight_per_Unit_Length, SpecTypeId.WeightPerUnitLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_ThermalConductivity, SpecTypeId.ThermalConductivity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_SpecificHeat, SpecTypeId.SpecificHeat);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_SpecificHeatOfVaporization, SpecTypeId.SpecificHeatOfVaporization);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_Permeability, SpecTypeId.Permeability);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_Resistivity, SpecTypeId.ElectricalResistivity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_MassDensity, SpecTypeId.MassDensity);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_MassPerUnitArea, SpecTypeId.MassPerUnitArea);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Pipe_Dimension, SpecTypeId.PipeDimension);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_PipeMass, SpecTypeId.PipingMass);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_PipeMassPerUnitLength, SpecTypeId.PipeMassPerUnitLength);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_HVAC_TemperatureDifference, SpecTypeId.HvacTemperatureDifference);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Piping_TemperatureDifference, SpecTypeId.PipingTemperatureDifference);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Electrical_TemperatureDifference, SpecTypeId.ElectricalTemperatureDifference);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_TimeInterval, SpecTypeId.Time);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Speed, SpecTypeId.Speed);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_Stationing, SpecTypeId.Stationing);

                //There is no obvious mapping for these:
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_ForcePerLength, SpecTypeId.Custom);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_ForceLengthPerAngle, SpecTypeId.Custom);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_LinearForcePerLength, SpecTypeId.Custom);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_LinearForceLengthPerAngle, SpecTypeId.Custom);
                ForgeTypeMap.Add(HLUnitTypeEnum.UT_AreaForcePerLength, SpecTypeId.Custom);



            }

            if (ForgeTypeMap.ContainsKey(unitType)) return ForgeTypeMap[unitType];


            throw new NoForgeTypeFoundException("No type found for: " + unitType.ToString());
        }


#endif


    }
}
