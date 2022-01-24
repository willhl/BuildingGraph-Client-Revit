

using Autodesk.Revit.DB;

namespace BuildingGraph.Integration.Revit
{
    /// <summary>
    /// Enumerated by UnitTypeId in Revit 2022, DisplayUnitType in earlier versions
    /// </summary>
    public class HLDisplayUnitType
    {

#if REVIT2022
        public HLDisplayUnitType(ForgeTypeId forgeTypeId)
        {
            AsForgeTypedId = forgeTypeId;
        }

        public ForgeTypeId AsForgeTypedId { get; private set; }

        public static bool operator ==(HLDisplayUnitType lhs, HLDisplayUnitTypeEnum rhs)
        {
            var hlut = HLConvert.ToHLDisplayUnitType(rhs);
            return lhs == hlut;
        }

        public static bool operator !=(HLDisplayUnitType lhs, HLDisplayUnitTypeEnum rhs)
        {
            var hlut = HLConvert.ToHLDisplayUnitType(rhs);
            return lhs != hlut;
        }


        public static bool operator ==(HLDisplayUnitType lhs, HLDisplayUnitType rhs)
        {
            return lhs.AsForgeTypedId == rhs.AsForgeTypedId;
        }

        public static bool operator !=(HLDisplayUnitType lhs, HLDisplayUnitType rhs)
        {
            return lhs.AsForgeTypedId != rhs.AsForgeTypedId;
        }

        public override int GetHashCode()
        {
            return AsForgeTypedId == null ? base.GetHashCode() : AsForgeTypedId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLDisplayUnitType)
            {
                return (other as HLDisplayUnitType).AsForgeTypedId == AsForgeTypedId;
            }
            return false;
        }

#else

        public HLDisplayUnitType(DisplayUnitType enumValue)
        {
            AsEnum = enumValue;
        }

        public DisplayUnitType AsEnum { get; private set; }


        public static bool operator ==(HLDisplayUnitType lhs, HLDisplayUnitTypeEnum rhs)
        {
            return lhs.AsEnum == (DisplayUnitType)(int)rhs;
        }

        public static bool operator !=(HLDisplayUnitType lhs, HLDisplayUnitTypeEnum rhs)
        {
            return lhs.AsEnum != (DisplayUnitType)(int)rhs; ;
        }


        public static bool operator ==(HLDisplayUnitType lhs, HLDisplayUnitType rhs)
        {
            return lhs.AsEnum == rhs.AsEnum;
        }

        public static bool operator !=(HLDisplayUnitType lhs, HLDisplayUnitType rhs)
        {
            return lhs.AsEnum != rhs.AsEnum;
        }

        public override int GetHashCode()
        {
            return AsEnum.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLDisplayUnitType)
            {
                return (other as HLDisplayUnitType).AsEnum == AsEnum;
            }
            else if (other is HLDisplayUnitTypeEnum || other is DisplayUnitType)
            {

                return AsEnum == (DisplayUnitType)(int)other;
            }


            return false;
        }

#endif

    }

    public static class HLDisplayUnitTypeExtentions 
    {

        public static HLDisplayUnitType HLGetDisplayUnitType(this Parameter param)
        {

#if REVIT2022
            return new HLDisplayUnitType(param.GetUnitTypeId());
#else
            return new HLDisplayUnitType(param.DisplayUnitType);
#endif

        }

        public static HLDisplayUnitType HLGetDisplayUnitType(this FamilyParameter param)
        {

#if REVIT2022
            return new HLDisplayUnitType(param.GetUnitTypeId());
#else
            return new HLDisplayUnitType(param.DisplayUnitType);
#endif

        }


        public static HLDisplayUnitType HLGetDisplayUnits(this FormatOptions options)
        {

#if REVIT2022
            return new HLDisplayUnitType(options.GetUnitTypeId());
#else
            return new HLDisplayUnitType(options.DisplayUnits);
#endif

        }

    }


}
