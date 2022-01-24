

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    //needs to be an object not enum
    //so any conversion is always handled in the extension methods only, so,
    //not this rv2022 > to hl enum > () > rv2020, but this rv2022 > () > rv2022 (no conversion)
    //conversion should only takes place where explicit enum values are checked in the upstream methods.

    /// <summary>
    /// Enumerated by SpecTypeId in Revit 2022, UnitType in earlier versions
    /// </summary>
    public class HLUnitType
    {
#if REVIT2022

        public HLUnitType(ForgeTypeId forgeTypeId)
        {
            AsForgeTypedId = forgeTypeId;
        }

        public ForgeTypeId AsForgeTypedId { get; private set; }


        public static bool operator ==(HLUnitType lhs, HLUnitTypeEnum rhs)
        {
            var hlut = HLConvert.ToHLUnitType(rhs);
            return lhs == hlut;
        }

        public static bool operator !=(HLUnitType lhs, HLUnitTypeEnum rhs)
        {
            var hlut = HLConvert.ToHLUnitType(rhs);
            return lhs != hlut;
        }

        public static bool operator ==(HLUnitType lhs, HLUnitType rhs)
        {
            return lhs.AsForgeTypedId == rhs.AsForgeTypedId;
        }

        public static bool operator !=(HLUnitType lhs, HLUnitType rhs)
        {
            return lhs.AsForgeTypedId != rhs.AsForgeTypedId;
        }

        public override int GetHashCode()
        {
            return AsForgeTypedId == null ? base.GetHashCode() : AsForgeTypedId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLUnitType)
            {
                return (other as HLUnitType).AsForgeTypedId == AsForgeTypedId;
            }
            return false;
        }

#else
        public HLUnitType(UnitType enumValue)
        {
            AsEnum = enumValue;
        }

        public UnitType AsEnum { get; private set; }


        public static bool operator ==(HLUnitType lhs, HLUnitTypeEnum rhs)
        {
            return lhs.AsEnum == (UnitType)(int)rhs;
        }

        public static bool operator !=(HLUnitType lhs, HLUnitTypeEnum rhs)
        {
            return lhs.AsEnum != (UnitType)(int)rhs; ;
        }



        public static bool operator ==(HLUnitType lhs, HLUnitType rhs)
        {
            return lhs.AsEnum == rhs.AsEnum;
        }

        public static bool operator !=(HLUnitType lhs, HLUnitType rhs)
        {
            return lhs.AsEnum != rhs.AsEnum;
        }

        public override int GetHashCode()
        {
            return AsEnum.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLUnitType)
            {
                return (other as HLUnitType).AsEnum == AsEnum;
            }
            else if (other is HLUnitTypeEnum || other is UnitType)
            {

                return AsEnum == (UnitType)(int)other;
            }

            return false;
        }
#endif
    }


    public static class HLUnitTypeExtensions
    {
        public static HLUnitType HLGetUnitType(this Definition def)
        {

#if REVIT2022
            return new HLUnitType(def.GetGroupTypeId());
#else
            return new HLUnitType(def.UnitType);
#endif
        }

        public static HLUnitType HLGetUnitType(this ScheduleField field)
        {

#if REVIT2022
            return new HLUnitType(field.GetSpecTypeId());

#else
            return new HLUnitType(field.UnitType);
#endif

        }

        public static FormatOptions HLGetFormatOptions(this Units options, HLUnitType unitType)
        {

#if REVIT2022
            return options.GetFormatOptions(unitType.AsForgeTypedId);
#else
            return options.GetFormatOptions(unitType.AsEnum);
#endif
        }


    }


}
