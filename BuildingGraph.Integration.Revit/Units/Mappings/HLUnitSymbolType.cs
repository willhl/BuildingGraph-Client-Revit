using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;


namespace BuildingGraph.Integration.Revit
{

    /// <summary>
    /// UnitSymbolType == SymbolTypeId
    /// </summary>
    public class HLUnitSymbolType
    {
        
#if REVIT2022

        public HLUnitSymbolType(ForgeTypeId forgeTypeId)
        {
            AsForgeTypedId = forgeTypeId;
        }

        public ForgeTypeId AsForgeTypedId { get; private set; }


        public static bool operator ==(HLUnitSymbolType lhs, HLUnitSymbolTypeEnum rhs)
        {
            var hlut = HLConvert.ToHLSymbolUnitType(rhs);
            return lhs == hlut;
        }

        public static bool operator !=(HLUnitSymbolType lhs, HLUnitSymbolTypeEnum rhs)
        {
            var hlut = HLConvert.ToHLSymbolUnitType(rhs);
            return lhs != hlut;
        }

        public static bool operator ==(HLUnitSymbolType lhs, HLUnitSymbolType rhs)
        {
            return lhs.AsForgeTypedId == rhs.AsForgeTypedId;
        }

        public static bool operator !=(HLUnitSymbolType lhs, HLUnitSymbolType rhs)
        {
            return lhs.AsForgeTypedId != rhs.AsForgeTypedId;
        }

        public override int GetHashCode()
        {
            return AsForgeTypedId == null ? base.GetHashCode() : AsForgeTypedId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLUnitSymbolType)
            {
                return (other as HLUnitSymbolType).AsForgeTypedId == AsForgeTypedId;
            }
            return false;
        }

        HLUnitTypeEnum asEnum;

#else
        public HLUnitSymbolType(UnitSymbolType enumValue)
        {
            AsEnum = enumValue;
        }

        public UnitSymbolType AsEnum { get; private set; }


        public static bool operator ==(HLUnitSymbolType lhs, HLUnitSymbolTypeEnum rhs)
        {
           
            return lhs.AsEnum == (UnitSymbolType)(int)rhs;
        }

        public static bool operator !=(HLUnitSymbolType lhs, HLUnitSymbolTypeEnum rhs)
        {
            return lhs.AsEnum != (UnitSymbolType)(int)rhs; ;
        }



        public static bool operator ==(HLUnitSymbolType lhs, HLUnitSymbolType rhs)
        {
            return lhs.AsEnum == rhs.AsEnum;
        }

        public static bool operator !=(HLUnitSymbolType lhs, HLUnitSymbolType rhs)
        {
            return lhs.AsEnum != rhs.AsEnum;
        }

        public override int GetHashCode()
        {
            return AsEnum.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is HLUnitSymbolType)
            {
                return (other as HLUnitSymbolType).AsEnum == AsEnum;
            }
            else if (other is HLUnitSymbolTypeEnum || other is UnitSymbolType)
            {

                return AsEnum == (UnitSymbolType)(int)other;
            }

            return false;
        }
#endif

    }

    public static class HLUnitSymbolTypeExtentions
    {

        public static HLUnitSymbolType HLGetUnitSymbol(this FormatOptions options)
        {

#if REVIT2022
            return new HLUnitSymbolType(options.GetSymbolTypeId());
#else
            return new HLUnitSymbolType(options.UnitSymbol);
#endif

        }

    }

}
