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

        public static HLUnitSymbolType ToHLSymbolUnitType(HLUnitSymbolTypeEnum unitType)
        {

#if REVIT2022
            var forgeType = ToForgeTypeId(unitType);
            return new HLUnitSymbolType(forgeType);
#else 

            return new HLUnitSymbolType((UnitSymbolType)(int)unitType);
#endif

        }


#if REVIT2022

        static Dictionary<HLUnitSymbolTypeEnum, ForgeTypeId> ForgeSymbolTypeMap;

        /// <summary>
        /// To be used when checking for expected or known legacy UnitType from Revit 2021 and earlier.
        /// Generally, for Revit 2022+, it recommended to obtain the ForgeTypeID
        /// using Revit's provided methods if you don't need to check 
        /// for equality with the known enums.
        /// </summary>
        /// <param name="unitType">The HLUnitSymbolTypeEnum enumeration to lookup SymbolTypeId</param>
        /// <returns></returns>
        public static ForgeTypeId ToForgeTypeId(HLUnitSymbolTypeEnum displaySymbolType)
        {

            //Some of these had no obvious mapping

            if (ForgeSymbolTypeMap == null)
            {
                ForgeSymbolTypeMap = new Dictionary<HLUnitSymbolTypeEnum, ForgeTypeId>();


                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_NONE, new ForgeTypeId());

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_PER_FT_S, SymbolTypeId.LbMassPerFtDashS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM_PER_FT_S, SymbolTypeId.LbmPerFtDashS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGM, SymbolTypeId.KgfDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TM, SymbolTypeId.TfDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGM_PER_M, SymbolTypeId.KgfDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGM_PER_SQ_M, SymbolTypeId.KgfPerMSup2);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_FORCE_AS_MASS_TONS, SymbolTypeId.UsTonnesMassTons);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_FORCE_AS_MASS_T, SymbolTypeId.UsTonnesMassT);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_FORCE_AS_MASS_ST, SymbolTypeId.UsTonnesMassSt);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_L_PER_M, SymbolTypeId.LPerMin);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_US_SURVEY_FT, SymbolTypeId.Ft);


                //all these should be fine

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_ACRES, SymbolTypeId.Acres);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_AMPERE, SymbolTypeId.Ampere);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CUSTOM, SymbolTypeId.Custom);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M, SymbolTypeId.Meter);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM, SymbolTypeId.Cm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM, SymbolTypeId.Mm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LF, SymbolTypeId.Lf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FOOT_SINGLE_QUOTE, SymbolTypeId.FootSingleQuote);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT, SymbolTypeId.Ft);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_INCH_DOUBLE_QUOTE, SymbolTypeId.InchDoubleQuote);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN, SymbolTypeId.In);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_HECTARES, SymbolTypeId.Hectare);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CY, SymbolTypeId.Cy);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SF, SymbolTypeId.Sf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_SUP_2, SymbolTypeId.FtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_CARET_2, SymbolTypeId.FtCaret2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_SUP_2, SymbolTypeId.MSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_CARET_2, SymbolTypeId.MCaret2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CF, SymbolTypeId.Cf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_SUP_3, SymbolTypeId.FtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_CARET_3, SymbolTypeId.FtCaret3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_SUP_3, SymbolTypeId.MSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_CARET_3, SymbolTypeId.MCaret3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_SYMBOL, SymbolTypeId.Degree);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PERCENT_SIGN, SymbolTypeId.Percent);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_SUP_2, SymbolTypeId.InSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_CARET_2, SymbolTypeId.InCaret2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_SUP_2, SymbolTypeId.CmSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_CARET_2, SymbolTypeId.CmCaret2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_SUP_2, SymbolTypeId.MmSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_CARET_2, SymbolTypeId.MmCaret2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_SUP_3, SymbolTypeId.InSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_CARET_3, SymbolTypeId.InCaret3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_SUP_3, SymbolTypeId.CmSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_CARET_3, SymbolTypeId.CmCaret3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_SUP_3, SymbolTypeId.MmSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_CARET_3, SymbolTypeId.MmCaret3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_L, SymbolTypeId.Liter);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GAL, SymbolTypeId.Gal);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KG_PER_CU_M, SymbolTypeId.KgPerMSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_MASS_PER_CU_FT, SymbolTypeId.LbMassPerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM_PER_CU_FT, SymbolTypeId.LbmPerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_MASS_PER_CU_IN, SymbolTypeId.LbMassPerInSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM_PER_CU_IN, SymbolTypeId.LbmPerInSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU, SymbolTypeId.Btu);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CAL, SymbolTypeId.Cal);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KCAL, SymbolTypeId.Kcal);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_JOULE, SymbolTypeId.Joule);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KWH, SymbolTypeId.KWh);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_THERM, SymbolTypeId.Therm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_WG_PER_100FT, SymbolTypeId.InDashWgPer100ft);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATT, SymbolTypeId.Watt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KILOWATT, SymbolTypeId.KW);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_S, SymbolTypeId.BtuPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_H, SymbolTypeId.BtuPerH);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CAL_PER_S, SymbolTypeId.CalPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KCAL_PER_S, SymbolTypeId.KcalPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATT_PER_SQ_FT, SymbolTypeId.WPerFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATT_PER_SQ_M, SymbolTypeId.WPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_WG, SymbolTypeId.InDashWg);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PASCAL, SymbolTypeId.Pa);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PASCAL_PER_M, SymbolTypeId.PaPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KILOPASCAL, SymbolTypeId.KPa);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MEGAPASCAL, SymbolTypeId.MPa);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PSI, SymbolTypeId.Psi);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_PER_SQ_IN, SymbolTypeId.LbForcePerInSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PSIG, SymbolTypeId.Psig);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PSIA, SymbolTypeId.Psia);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF_PER_SQ_IN, SymbolTypeId.LbfPerInSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_HG, SymbolTypeId.InHg);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_HG, SymbolTypeId.MmHg);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_ATM, SymbolTypeId.Atm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BAR, SymbolTypeId.Bar);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_F, SymbolTypeId.DegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_C, SymbolTypeId.DegreeC);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KELVIN, SymbolTypeId.Kelvin);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_R, SymbolTypeId.DegreeR);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_PER_MIN, SymbolTypeId.FtPerMin);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FPM, SymbolTypeId.Fpm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_PER_S, SymbolTypeId.MPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_PER_MIN, SymbolTypeId.CmPerMin);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CU_FT_PER_MIN, SymbolTypeId.FtSup3PerH);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CFM, SymbolTypeId.Cfm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_L_PER_S, SymbolTypeId.LPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LPS, SymbolTypeId.Lps);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CU_M_PER_S, SymbolTypeId.MSup3PerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CMS, SymbolTypeId.Cms);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CU_M_PER_H, SymbolTypeId.MSup3PerH);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CMH, SymbolTypeId.Cmh);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GAL_PER_MIN, SymbolTypeId.GalPerMin);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GPM, SymbolTypeId.Gpm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USGPM, SymbolTypeId.Usgpm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GAL_PER_H, SymbolTypeId.GalPerH);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GPH, SymbolTypeId.Gph);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USGPH, SymbolTypeId.Usgph);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KILOAMPERE, SymbolTypeId.KA);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MILLIAMPERE, SymbolTypeId.MA);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_VOLT, SymbolTypeId.Volt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KILOVOLT, SymbolTypeId.KV);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MILLIVOLT, SymbolTypeId.MV);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_HZ, SymbolTypeId.Hz);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CPS, SymbolTypeId.Cps);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LX, SymbolTypeId.Lx);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FC, SymbolTypeId.Fc);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FTC, SymbolTypeId.Ftc);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FL, SymbolTypeId.FL);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FL_LOWERCASE, SymbolTypeId.FlLowercase);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FTL, SymbolTypeId.FtL);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CD_PER_SQ_M, SymbolTypeId.CdPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CD, SymbolTypeId.Cd);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LM, SymbolTypeId.Lm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_VOLTAMPERE, SymbolTypeId.VA);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KILOVOLTAMPERE, SymbolTypeId.KVA);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_HP, SymbolTypeId.Hp);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_N, SymbolTypeId.Newton);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DA_N, SymbolTypeId.DaN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N, SymbolTypeId.KN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_N, SymbolTypeId.MN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP, SymbolTypeId.Kip);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGF, SymbolTypeId.Kgf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TF, SymbolTypeId.Tf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE, SymbolTypeId.LbForce);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF, SymbolTypeId.Lbf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_N_PER_M, SymbolTypeId.NPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DA_N_PER_M, SymbolTypeId.DaNPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_PER_M, SymbolTypeId.KNPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_N_PER_M, SymbolTypeId.MNPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_PER_FT, SymbolTypeId.KipPerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGF_PER_M, SymbolTypeId.KgfPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TF_PER_M, SymbolTypeId.TfPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_PER_FT, SymbolTypeId.LbForcePerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF_PER_FT, SymbolTypeId.LbfPerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_N_PER_M_SUP_2, SymbolTypeId.NPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DA_N_PER_M_SUP_2, SymbolTypeId.DaNPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_PER_M_SUP_2, SymbolTypeId.KNPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_N_PER_M_SUP_2, SymbolTypeId.MNPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KSF, SymbolTypeId.Ksf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_PER_SQ_FT, SymbolTypeId.KipPerFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGF_PER_M_SUP_2, SymbolTypeId.KgfPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TF_PER_M_SUP_2, SymbolTypeId.TfPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PSF, SymbolTypeId.Psf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_PER_SQ_FT, SymbolTypeId.LbForcePerFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF_PER_SQ_FT, SymbolTypeId.LbfPerFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_N_DASH_M, SymbolTypeId.NDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DA_N_DASH_M, SymbolTypeId.DaNDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_DASH_M, SymbolTypeId.KNDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_N_DASH_M, SymbolTypeId.MNDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_DASH_FT, SymbolTypeId.KipDashFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGF_DASH_M, SymbolTypeId.KgfDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TF_DASH_M, SymbolTypeId.TfDashM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_DASH_FT, SymbolTypeId.LbForceDashFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF_DASH_FT, SymbolTypeId.LbfDashFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_PER_K_N, SymbolTypeId.MPerKN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_PER_KIP, SymbolTypeId.FtPerKip);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_SUP_2_PER_K_N, SymbolTypeId.MSup2PerKN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_SUP_2_PER_KIP, SymbolTypeId.FtSup2PerKip);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_SUP_3_PER_K_N, SymbolTypeId.MSup3PerKN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_SUP_3_PER_KIP, SymbolTypeId.FtSup3PerKip);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_INV_K_N, SymbolTypeId.InvKN);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_INV_KIP, SymbolTypeId.InvKip);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FTH2O_PER_100FT, SymbolTypeId.FtH2OPer100ft);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_OF_WATER_PER_100FT, SymbolTypeId.FtOfWaterPer100ft);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FEET_OF_WATER_PER_100FT, SymbolTypeId.FeetOfWaterPer100ft);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FTH2O, SymbolTypeId.FtH2O);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_OF_WATER, SymbolTypeId.FtOfWater);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FEET_OF_WATER, SymbolTypeId.FeetOfWater);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PA_S, SymbolTypeId.PaDashS);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CP, SymbolTypeId.CP);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_PER_S, SymbolTypeId.FtPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FPS, SymbolTypeId.Fps);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KSI, SymbolTypeId.Ksi);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_PER_SQ_IN, SymbolTypeId.KipPerInSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KN_PER_M_SUP_3, SymbolTypeId.KNPerMSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_PER_CU_FT, SymbolTypeId.LbForcePerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF_PER_CU_FT, SymbolTypeId.LbfPerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_PER_IN_SUP_3, SymbolTypeId.KipPerInSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_INV_DEGREE_F, SymbolTypeId.InvDegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_INV_DEGREE_C, SymbolTypeId.InvDegreeC);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_N_DASH_M_PER_M, SymbolTypeId.NDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DA_N_DASH_M_PER_M, SymbolTypeId.DaNDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_DASH_M_PER_M, SymbolTypeId.KNDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_N_DASH_M_PER_M, SymbolTypeId.MNDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_DASH_FT_PER_FT, SymbolTypeId.KipDashFtPerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KGF_DASH_M_PER_M, SymbolTypeId.KgfDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TF_DASH_M_PER_M, SymbolTypeId.TfDashMPerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_DASH_FT_PER_FT, SymbolTypeId.LbForceDashFtPerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBF_DASH_FT_PER_FT, SymbolTypeId.LbfDashFtPerFt);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_FORCE_PER_FT_H, SymbolTypeId.LbMassPerFtDashH);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM_PER_FT_H, SymbolTypeId.LbmPerFtDashH);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIPS_PER_IN, SymbolTypeId.KipPerIn);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIPS_PER_CU_FT, SymbolTypeId.KipPerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_FT_PER_DEGREE, SymbolTypeId.KipDashFtPerDegree);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_M_PER_DEGREE, SymbolTypeId.KNDashMPerDegree);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KIP_FT_PER_DEGREE_PER_FT, SymbolTypeId.KipDashFtPerDegreePerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_M_PER_DEGREE_PER_M, SymbolTypeId.KNDashMPerDegreePerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATT_PER_SQ_M_K, SymbolTypeId.WPerMSup2K);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_H_SQ_FT_DEGREE_F, SymbolTypeId.BtuPerHFtSup2DegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CFM_PER_SQ_FT, SymbolTypeId.CfmPerFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CFM_PER_SF, SymbolTypeId.CfmPerSf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LPS_PER_SQ_M, SymbolTypeId.LpsPerMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_L_PER_S_SQ_M, SymbolTypeId.LPerSMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_COLON_10, SymbolTypeId.Colon10);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_COLON_12, SymbolTypeId.Colon12);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SLOPE_DEGREE_SYMBOL, SymbolTypeId.SlopeDegree);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATT_PER_CU_FT, SymbolTypeId.WPerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATT_PER_CU_M, SymbolTypeId.WPerMSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_H_SQ_FT, SymbolTypeId.BtuPerHFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_H_CU_FT, SymbolTypeId.BtuPerHFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TON, SymbolTypeId.Ton);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_TON_OF_REFRIGERATION, SymbolTypeId.TonOfRefrigeration);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CFM_PER_CU_FT, SymbolTypeId.CfmPerFtSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CFM_PER_CF, SymbolTypeId.CfmPerCf);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_L_PER_S_CU_M, SymbolTypeId.LPerSMSup3);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CFM_PER_TON, SymbolTypeId.CfmPerTon);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_L_PER_S_KW, SymbolTypeId.LPerSKw);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_FT_PER_TON, SymbolTypeId.FtSup2PerTon);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SF_PER_TON, SymbolTypeId.SfPerTon);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_M_PER_KW, SymbolTypeId.MSup2PerKw);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DOLLAR, SymbolTypeId.UsDollar);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_EURO_SUFFIX, SymbolTypeId.EuroSuffix);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_EURO_PREFIX, SymbolTypeId.EuroPrefix);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_POUND, SymbolTypeId.UkPound);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_YEN, SymbolTypeId.Yen);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CHINESE_HONG_KONG_SAR, SymbolTypeId.ChineseHongKongDollar);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WON, SymbolTypeId.Won);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SHEQEL, SymbolTypeId.Shekel);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DONG, SymbolTypeId.Dong);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BAHT, SymbolTypeId.Baht);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KRONER, SymbolTypeId.Krone);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LM_PER_W, SymbolTypeId.LmPerW);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SF_PER_MBH, SymbolTypeId.SfPerMbh);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SF_PER_KBTU_PER_H, SymbolTypeId.SfHPerKbtu);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_FT_PER_MBH, SymbolTypeId.FtSup2PerMbh);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_FT_PER_KBTU_PER_H, SymbolTypeId.FtSup2HPerKbtu);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_PER_CM_SUP_2, SymbolTypeId.KNPerCmSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_N_PER_MM_SUP_2, SymbolTypeId.NPerMmSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_K_N_PER_MM_SUP_2, SymbolTypeId.KNPerMmSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_ONE_COLON, SymbolTypeId.OneColon);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_H_SQ_FT_DEGREE_F_PER_BTU, SymbolTypeId.HFtSup2DegreeFPerBtu);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_M_K_PER_WATT, SymbolTypeId.MSup2KPerW);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_F, SymbolTypeId.BtuPerDegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_J_PER_KELVIN, SymbolTypeId.JPerK);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KJ_PER_KELVIN, SymbolTypeId.KJPerK);




                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_MASS, SymbolTypeId.LbMass);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM, SymbolTypeId.Lbm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_PER_SQ_S, SymbolTypeId.MPerSSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KM_PER_SQ_S, SymbolTypeId.KmPerSSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_PER_SQ_S, SymbolTypeId.InPerSSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_PER_SQ_S, SymbolTypeId.FtPerSSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MI_PER_SQ_S, SymbolTypeId.MiPerSSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_SUP_4, SymbolTypeId.FtSup4);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_SUP_4, SymbolTypeId.InSup4);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_SUP_4, SymbolTypeId.MmSup4);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_SUP_4, SymbolTypeId.CmSup4);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_SUP_4, SymbolTypeId.MSup4);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_FT_SUP_6, SymbolTypeId.FtSup6);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_IN_SUP_6, SymbolTypeId.InSup6);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MM_SUP_6, SymbolTypeId.MmSup6);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_CM_SUP_6, SymbolTypeId.CmSup6);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_M_SUP_6, SymbolTypeId.MSup6);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_FT_PER_FT, SymbolTypeId.FtSup2PerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_IN_PER_FT, SymbolTypeId.InSup2PerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_MM_PER_M, SymbolTypeId.MmSup2PerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_CM_PER_M, SymbolTypeId.CmSup2PerM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_SQ_M_PER_M, SymbolTypeId.MSup2PerM);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LB_MASS_PER_FT, SymbolTypeId.LbMassPerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM_PER_FT, SymbolTypeId.LbmPerFt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_RAD, SymbolTypeId.Rad);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GRAD, SymbolTypeId.Grad);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_RAD_PER_S, SymbolTypeId.RadPerS);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MS, SymbolTypeId.Ms);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_S, SymbolTypeId.Second);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MIN, SymbolTypeId.Min);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_H, SymbolTypeId.Hour);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KM_PER_H, SymbolTypeId.KmPerH);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_MI_PER_H, SymbolTypeId.Mph);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KJ, SymbolTypeId.KJ);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LBM_PER_SQ_FT, SymbolTypeId.LbMassPerFtSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_WATTS_PER_METER_KELVIN, SymbolTypeId.WPerMK);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_J_PER_G_CELSIUS, SymbolTypeId.JPerGDegreeC);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_J_PER_G, SymbolTypeId.JPerG);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_NG_PER_PA_S_SQ_M, SymbolTypeId.NgPerPaSMSup2);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_OHM_M, SymbolTypeId.OhmM);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_H_FT_DEGREE_F, SymbolTypeId.BtuPerHFtDegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_LB_DEGREE_F, SymbolTypeId.BtuPerLbDegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_BTU_PER_LB, SymbolTypeId.BtuPerLb);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_GR_PER_H_SQ_FT_IN_HG, SymbolTypeId.GrPerHFtSup2InHg);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_PER_MILLE_SIGN, SymbolTypeId.PerMille);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DM, SymbolTypeId.Dm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_J_PER_KG_CELSIUS, SymbolTypeId.JPerKgDegreeC);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_UM_PER_M_C, SymbolTypeId.UmPerMDegreeC);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_UIN_PER_IN_F, SymbolTypeId.UinPerInDegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_MASS_TONS, SymbolTypeId.UsTonnesMassTons);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_MASS_T, SymbolTypeId.UsTonnesMassT);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_MASS_ST, SymbolTypeId.UsTonnesMassSt);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_FORCE_TONSF, SymbolTypeId.UsTonnesForceTons);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_USTONNES_FORCE_STF, SymbolTypeId.UsTonnesForceSt);

                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_LPM, SymbolTypeId.Lpm);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_F_DIFFERENCE, SymbolTypeId.DegreeFInterval);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DELTA_DEGREE_F, SymbolTypeId.DeltaDegreeF);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_C_DIFFERENCE, SymbolTypeId.DegreeCInterval);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DELTA_DEGREE_C, SymbolTypeId.DeltaDegreeC);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_KELVIN_DIFFERENCE, SymbolTypeId.KelvinInterval);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DELTA_KELVIN, SymbolTypeId.DeltaK);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DEGREE_R_DIFFERENCE, SymbolTypeId.DegreeRInterval);
                ForgeSymbolTypeMap.Add(HLUnitSymbolTypeEnum.UST_DELTA_DEGREE_R, SymbolTypeId.DeltaDegreeR);

            }

            if (ForgeSymbolTypeMap.ContainsKey(displaySymbolType)) return ForgeSymbolTypeMap[displaySymbolType];

            throw new NoForgeTypeFoundException("No type found for: " + displaySymbolType.ToString());

        }
#endif


    }
}
