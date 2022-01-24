using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Integration.Revit
{
    public enum HLParameterTypeEnum
    {
        //
        // Summary:
        //     The parameter type is invalid. This value should not be used.
        Invalid = 0,
        //
        // Summary:
        //     The parameter data should be interpreted as a string of text.
        Text = 1,
        //
        // Summary:
        //     The parameter data should be interpreted as a whole number, positive or negative.
        Integer = 2,
        //
        // Summary:
        //     The parameter data should be interpreted as a real number, possibly including
        //     decimal points.
        Number = 3,
        //
        // Summary:
        //     The parameter data represents a length.
        Length = 4,
        //
        // Summary:
        //     The parameter data represents an area.
        Area = 5,
        //
        // Summary:
        //     The parameter data represents a volume.
        Volume = 6,
        //
        // Summary:
        //     The parameter data represents an angle.
        Angle = 7,
        //
        // Summary:
        //     A text string that represents a web address.
        URL = 8,
        //
        // Summary:
        //     The value of this property is considered to be a material.
        Material = 9,
        //
        // Summary:
        //     A boolean value that will be represented as Yes or No.
        YesNo = 10,
        //
        // Summary:
        //     The data value will be represented as a force.
        Force = 11,
        //
        // Summary:
        //     The data value will be represented as a linear force.
        LinearForce = 12,
        //
        // Summary:
        //     The data value will be represented as an area force.
        AreaForce = 13,
        //
        // Summary:
        //     The data value will be represented as a moment.
        Moment = 14,
        //
        // Summary:
        //     A parameter value that represents the number of poles, as used in electrical
        //     disciplines.
        NumberOfPoles = 15,
        //
        // Summary:
        //     A parameter value that represents the fixture units, as used in piping disciplines.
        FixtureUnit = 16,
        //
        // Summary:
        //     A parameter used to control the type of a family nested within another family.
        FamilyType = 17,
        //
        // Summary:
        //     A parameter value that represents the load classification units, as used in electrical
        //     disciplines.
        LoadClassification = 18,
        //
        // Summary:
        //     The value of this parameter is the id of an image.
        Image = 19,
        //
        // Summary:
        //     The value of this parameter will be represented as multiline text.
        MultilineText = 20,
        //
        // Summary:
        //     The data value will be represented as a HVACDensity.
        HVACDensity = 107,
        //
        // Summary:
        //     The data value will be represented as a HVACEnergy.
        HVACEnergy = 108,
        //
        // Summary:
        //     The data value will be represented as a HVACFriction.
        HVACFriction = 109,
        //
        // Summary:
        //     The data value will be represented as a HVACPower.
        HVACPower = 110,
        //
        // Summary:
        //     The data value will be represented as a HVACPowerDensity.
        HVACPowerDensity = 111,
        //
        // Summary:
        //     The data value will be represented as a HVACPressure.
        HVACPressure = 112,
        //
        // Summary:
        //     The data value will be represented as a HVACTemperature.
        HVACTemperature = 113,
        //
        // Summary:
        //     The data value will be represented as a HVACVelocity.
        HVACVelocity = 114,
        //
        // Summary:
        //     The data value will be represented as a HVACAirflow.
        HVACAirflow = 115,
        //
        // Summary:
        //     The data value will be represented as a HVACDuctSize.
        HVACDuctSize = 116,
        //
        // Summary:
        //     The data value will be represented as a HVACCrossSection.
        HVACCrossSection = 117,
        //
        // Summary:
        //     The data value will be represented as a HVACHeatGain.
        HVACHeatGain = 118,
        //
        // Summary:
        //     The data value will be represented as an ElectricalCurrent.
        ElectricalCurrent = 119,
        //
        // Summary:
        //     The data value will be represented as an ElectricalPotential.
        ElectricalPotential = 120,
        //
        // Summary:
        //     The data value will be represented as an ElectricalFrequency.
        ElectricalFrequency = 121,
        //
        // Summary:
        //     The data value will be represented as an ElectricalIlluminance.
        ElectricalIlluminance = 122,
        //
        // Summary:
        //     The data value will be represented as an ElectricalLuminousFlux.
        ElectricalLuminousFlux = 123,
        //
        // Summary:
        //     The data value will be represented as an ElectricalPower.
        ElectricalPower = 124,
        //
        // Summary:
        //     The data value will be represented as a HVACRoughness.
        HVACRoughness = 125,
        //
        // Summary:
        //     The data value will be represented as an ElectricalApparentPower.
        ElectricalApparentPower = 134,
        //
        // Summary:
        //     The data value will be represented as an ElectricalPowerDensity.
        ElectricalPowerDensity = 135,
        //
        // Summary:
        //     The data value will be represented as a PipingDensity.
        PipingDensity = 136,
        //
        // Summary:
        //     The data value will be represented as a PipingFlow.
        PipingFlow = 137,
        //
        // Summary:
        //     The data value will be represented as a PipingFriction.
        PipingFriction = 138,
        //
        // Summary:
        //     The data value will be represented as a PipingPressure.
        PipingPressure = 139,
        //
        // Summary:
        //     The data value will be represented as a PipingTemperature.
        PipingTemperature = 140,
        //
        // Summary:
        //     The data value will be represented as a PipingVelocity.
        PipingVelocity = 141,
        //
        // Summary:
        //     The data value will be represented as a PipingViscosity.
        PipingViscosity = 142,
        //
        // Summary:
        //     The data value will be represented as a PipeSize.
        PipeSize = 143,
        //
        // Summary:
        //     The data value will be represented as a PipingRoughness.
        PipingRoughness = 144,
        //
        // Summary:
        //     The data value will be represented as a Stress.
        Stress = 145,
        //
        // Summary:
        //     The data value will be represented as a UnitWeight.
        UnitWeight = 146,
        //
        // Summary:
        //     The data value will be represented as a ThermalExpansion.
        ThermalExpansion = 147,
        //
        // Summary:
        //     The data value will be represented as a LinearMoment.
        LinearMoment = 148,
        //
        // Summary:
        //     The data value will be represented as a ForcePerLength.
        ForcePerLength = 150,
        //
        // Summary:
        //     The data value will be represented as a ForceLengthPerAngle.
        ForceLengthPerAngle = 151,
        //
        // Summary:
        //     The data value will be represented as a LinearForcePerLength.
        LinearForcePerLength = 152,
        //
        // Summary:
        //     The data value will be represented as a LinearForceLengthPerAngle.
        LinearForceLengthPerAngle = 153,
        //
        // Summary:
        //     The data value will be represented as an AreaForcePerLength.
        AreaForcePerLength = 154,
        //
        // Summary:
        //     The data value will be represented as a PipingVolume.
        PipingVolume = 155,
        //
        // Summary:
        //     The data value will be represented as a HVACViscosity.
        HVACViscosity = 156,
        //
        // Summary:
        //     The data value will be represented as a HVACCoefficientOfHeatTransfer.
        HVACCoefficientOfHeatTransfer = 157,
        //
        // Summary:
        //     The data value will be represented as a HVACAirflowDensity.
        HVACAirflowDensity = 158,
        //
        // Summary:
        //     The data value will be represented as a Slope.
        Slope = 159,
        //
        // Summary:
        //     The data value will be represented as a HVACCoolingLoad.
        HVACCoolingLoad = 160,
        //
        // Summary:
        //     The data value will be represented as a HVACCoolingLoadDividedByArea.
        HVACCoolingLoadDividedByArea = 161,
        //
        // Summary:
        //     The data value will be represented as a HVACCoolingLoadDividedByVolume.
        HVACCoolingLoadDividedByVolume = 162,
        //
        // Summary:
        //     The data value will be represented as a HVACHeatingLoad.
        HVACHeatingLoad = 163,
        //
        // Summary:
        //     The data value will be represented as a HVACHeatingLoadDividedByArea.
        HVACHeatingLoadDividedByArea = 164,
        //
        // Summary:
        //     The data value will be represented as a HVACHeatingLoadDividedByVolume.
        HVACHeatingLoadDividedByVolume = 165,
        //
        // Summary:
        //     The data value will be represented as a HVACAirflowDividedByVolume.
        HVACAirflowDividedByVolume = 166,
        //
        // Summary:
        //     The data value will be represented as a HVACAirflowDividedByCoolingLoad.
        HVACAirflowDividedByCoolingLoad = 167,
        //
        // Summary:
        //     The data value will be represented as a HVACAreaDividedByCoolingLoad.
        HVACAreaDividedByCoolingLoad = 168,
        //
        // Summary:
        //     The data value will be represented as a WireSize.
        WireSize = 169,
        //
        // Summary:
        //     The data value will be represented as a HVACSlope.
        HVACSlope = 170,
        //
        // Summary:
        //     The data value will be represented as a PipingSlope.
        PipingSlope = 171,
        //
        // Summary:
        //     The data value will be represented as a Currency.
        Currency = 172,
        //
        // Summary:
        //     The data value will be represented as an ElectricalEfficacy.
        ElectricalEfficacy = 173,
        //
        // Summary:
        //     The data value will be represented as an ElectricalWattage.
        ElectricalWattage = 174,
        //
        // Summary:
        //     The data value will be represented as a ColorTemperature.
        ColorTemperature = 175,
        //
        // Summary:
        //     The data value will be represented as an ElectricalLuminousIntensity.
        ElectricalLuminousIntensity = 177,
        //
        // Summary:
        //     The data value will be represented as an ElectricalLuminance.
        ElectricalLuminance = 178,
        //
        // Summary:
        //     The data value will be represented as a HVACAreaDividedByHeatingLoad.
        HVACAreaDividedByHeatingLoad = 179,
        //
        // Summary:
        //     The data value will be represented as a HVACFactor.
        HVACFactor = 180,
        //
        // Summary:
        //     The data value will be represented as a ElectricalTemperature.
        ElectricalTemperature = 181,
        //
        // Summary:
        //     The data value will be represented as a ElectricalCableTraySize.
        ElectricalCableTraySize = 182,
        //
        // Summary:
        //     The data value will be represented as a ElectricalConduitSize.
        ElectricalConduitSize = 183,
        //
        // Summary:
        //     The data value will be represented as a ReinforcementVolume.
        ReinforcementVolume = 184,
        //
        // Summary:
        //     The data value will be represented as a ReinforcementLength.
        ReinforcementLength = 185,
        //
        // Summary:
        //     The data value will be represented as a ElectricalDemandFactor.
        ElectricalDemandFactor = 186,
        //
        // Summary:
        //     The data value will be represented as a HVACDuctInsulationThickness.
        HVACDuctInsulationThickness = 187,
        //
        // Summary:
        //     The data value will be represented as a HVACDuctLiningThickness.
        HVACDuctLiningThickness = 188,
        //
        // Summary:
        //     The data value will be represented as a PipeInsulationThickness.
        PipeInsulationThickness = 189,
        //
        // Summary:
        //     The data value will be represented as a HVACThermalResistance.
        HVACThermalResistance = 190,
        //
        // Summary:
        //     The data value will be represented as a HVACThermalMass.
        HVACThermalMass = 191,
        //
        // Summary:
        //     The data value will be represented as an Acceleration.
        Acceleration = 192,
        //
        // Summary:
        //     The data value will be represented as a BarDiameter.
        BarDiameter = 193,
        //
        // Summary:
        //     The data value will be represented as a CrackWidth.
        CrackWidth = 194,
        //
        // Summary:
        //     The data value will be represented as a DisplacementDeflection.
        DisplacementDeflection = 195,
        //
        // Summary:
        //     The data value will be represented as an Energy.
        Energy = 196,
        //
        // Summary:
        //     The data value will be represented as a StructuralFrequency.
        StructuralFrequency = 197,
        //
        // Summary:
        //     The data value will be represented as a Mass.
        Mass = 198,
        //
        // Summary:
        //     The data value will be represented as a MassPerUnitLength.
        MassPerUnitLength = 199,
        //
        // Summary:
        //     The data value will be represented as a MomentOfInertia.
        MomentOfInertia = 200,
        //
        // Summary:
        //     The data value will be represented as a SurfaceArea.
        SurfaceArea = 201,
        //
        // Summary:
        //     The data value will be represented as a Period.
        Period = 202,
        //
        // Summary:
        //     The data value will be represented as a Pulsation.
        Pulsation = 203,
        //
        // Summary:
        //     The data value will be represented as a ReinforcementArea.
        ReinforcementArea = 204,
        //
        // Summary:
        //     The data value will be represented as a ReinforcementAreaPerUnitLength.
        ReinforcementAreaPerUnitLength = 205,
        //
        // Summary:
        //     The data value will be represented as a ReinforcementCover.
        ReinforcementCover = 206,
        //
        // Summary:
        //     The data value will be represented as a ReinforcementSpacing.
        ReinforcementSpacing = 207,
        //
        // Summary:
        //     The data value will be represented as a Rotation.
        Rotation = 208,
        //
        // Summary:
        //     The data value will be represented as a SectionArea.
        SectionArea = 209,
        //
        // Summary:
        //     The data value will be represented as a SectionDimension.
        SectionDimension = 210,
        //
        // Summary:
        //     The data value will be represented as a SectionModulus.
        SectionModulus = 211,
        //
        // Summary:
        //     The data value will be represented as a SectionProperty.
        SectionProperty = 212,
        //
        // Summary:
        //     The data value will be represented as a StructuralVelocity.
        StructuralVelocity = 213,
        //
        // Summary:
        //     The data value will be represented as a WarpingConstant.
        WarpingConstant = 214,
        //
        // Summary:
        //     The data value will be represented as a Weight.
        Weight = 215,
        //
        // Summary:
        //     The data value will be represented as a WeightPerUnitLength.
        WeightPerUnitLength = 216,
        //
        // Summary:
        //     The data value will be represented as a HVACThermalConductivity.
        HVACThermalConductivity = 217,
        //
        // Summary:
        //     The data value will be represented as a HVACSpecificHeat.
        HVACSpecificHeat = 218,
        //
        // Summary:
        //     The data value will be represented as a HVACSpecificHeatOfVaporization.
        HVACSpecificHeatOfVaporization = 219,
        //
        // Summary:
        //     The data value will be represented as a HVACPermeability.
        HVACPermeability = 220,
        //
        // Summary:
        //     The data value will be represented as a ElectricalResistivity.
        ElectricalResistivity = 221,
        //
        // Summary:
        //     The data value will be represented as a MassDensity.
        MassDensity = 222,
        //
        // Summary:
        //     The data value will be represented as a MassPerUnitArea.
        MassPerUnitArea = 223,
        //
        // Summary:
        //     The value of this parameter will be a Pipe Dimension
        PipeDimension = 224,
        //
        // Summary:
        //     The value of this parameter will be the Pipe Mass
        PipeMass = 225,
        //
        // Summary:
        //     The value of this parameter will be the Pipe Mass per Unit Length
        PipeMassPerUnitLength = 226,
        //
        // Summary:
        //     The data value will be represented as a HVACTemperatureDifference.
        HVACTemperatureDifference = 227,
        //
        // Summary:
        //     The data value will be represented as a PipingTemperatureDifference.
        PipingTemperatureDifference = 228,
        //
        // Summary:
        //     The data value will be represented as an ElectricalTemperatureDifference.
        ElectricalTemperatureDifference = 229,
        //
        // Summary:
        //     The data value will be represented as a TimeInterval
        TimeInterval = 230,
        //
        // Summary:
        //     The data value will be represented as a Speed
        Speed = 231,
        //
        // Summary:
        //     The data value will be represented as infrastructure stationing
        Stationing = 232
    }
}
