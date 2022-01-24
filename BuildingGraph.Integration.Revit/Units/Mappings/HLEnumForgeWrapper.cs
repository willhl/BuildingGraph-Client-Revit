using System;
using Autodesk.Revit.DB;


namespace BuildingGraph.Integration.Revit
{
    /*
    public class HLEnumForgeWrapper<hlenum, native> where hlenum : System.Enum where native : System.Enum
    {
#if REVIT2022

        public HLUnitType(ForgeTypeId forgeTypeId)
        {
            AsForgeTypedId = forgeTypeId;
        }

        public ForgeTypeId AsForgeTypedId { get; private set; }


        public static bool operator ==(HLUnitType lhs, HLUnitTypeEnum rhs)
        {

            //Convert to ForgeType?
            return false;
        }

        public static bool operator !=(HLUnitType lhs, HLUnitTypeEnum rhs)
        {

            //Convert to ForgeType?
            return false;
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

        HLUnitTypeEnum asEnum;

#else
        public HLEnumForgeWrapper(native enumValue)
        {
            AsEnum = enumValue;
        }

        public native AsEnum { get; private set; }


        public static bool operator ==(HLEnumForgeWrapper<hlenum, native> lhs, hlenum rhs)
        {
            return Convert.ToInt32(lhs.AsEnum) == Convert.ToInt32(rhs);
        }

        public static bool operator !=(HLEnumForgeWrapper<hlenum, native> lhs, hlenum rhs)
        {
            return Convert.ToInt32(lhs.AsEnum) != Convert.ToInt32(rhs);
        }

        public static bool operator ==(HLEnumForgeWrapper<hlenum, native> lhs, HLEnumForgeWrapper<hlenum, native> rhs)
        {
            return Convert.ToInt32(lhs.AsEnum) == Convert.ToInt32(rhs.AsEnum);
        }

        public static bool operator !=(HLEnumForgeWrapper<hlenum, native> lhs, HLEnumForgeWrapper<hlenum, native> rhs)
        {
            return Convert.ToInt32(lhs.AsEnum) != Convert.ToInt32(rhs.AsEnum);
        }

        public override int GetHashCode()
        {
            return AsEnum.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLEnumForgeWrapper<hlenum, native>)
            {
                return (other as HLEnumForgeWrapper<hlenum, native>).AsEnum.Equals(AsEnum);
            }
            else if (other is HLUnitSymbolTypeEnum || other is UnitSymbolType)
            {
                return Convert.ToInt32(AsEnum) == Convert.ToInt32((UnitSymbolType)other);
            }

            return false;
        }
#endif

    }
    */
}
