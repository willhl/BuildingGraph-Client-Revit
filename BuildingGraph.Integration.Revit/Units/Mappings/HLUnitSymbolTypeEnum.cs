﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGraph.Integration.Revit
{ 
    public enum HLUnitSymbolTypeEnum
    {
        UST_CUSTOM = -1,
        UST_NONE = 0,
        UST_M = 1,
        UST_CM = 101,
        UST_MM = 201,
        UST_LF = 301,
        UST_FOOT_SINGLE_QUOTE = 302,
        UST_FT = 303,
        UST_INCH_DOUBLE_QUOTE = 601,
        UST_IN = 602,
        UST_ACRES = 701,
        UST_HECTARES = 801,
        UST_CY = 1001,
        UST_SF = 1101,
        UST_FT_SUP_2 = 1102,
        UST_FT_CARET_2 = 1103,
        UST_M_SUP_2 = 1201,
        UST_M_CARET_2 = 1202,
        UST_CF = 1301,
        UST_FT_SUP_3 = 1302,
        UST_FT_CARET_3 = 1303,
        UST_M_SUP_3 = 1401,
        UST_M_CARET_3 = 1402,
        UST_DEGREE_SYMBOL = 1501,
        UST_PERCENT_SIGN = 1901,
        UST_IN_SUP_2 = 2001,
        UST_IN_CARET_2 = 2002,
        UST_CM_SUP_2 = 2101,
        UST_CM_CARET_2 = 2102,
        UST_MM_SUP_2 = 2201,
        UST_MM_CARET_2 = 2202,
        UST_IN_SUP_3 = 2301,
        UST_IN_CARET_3 = 2302,
        UST_CM_SUP_3 = 2401,
        UST_CM_CARET_3 = 2402,
        UST_MM_SUP_3 = 2501,
        UST_MM_CARET_3 = 2502,
        UST_L = 2601,
        UST_GAL = 2701,
        UST_KG_PER_CU_M = 2801,
        UST_LB_MASS_PER_CU_FT = 2901,
        UST_LBM_PER_CU_FT = 2902,
        UST_LB_MASS_PER_CU_IN = 3001,
        UST_LBM_PER_CU_IN = 3002,
        UST_BTU = 3101,
        UST_CAL = 3201,
        UST_KCAL = 3301,
        UST_JOULE = 3401,
        UST_KWH = 3501,
        UST_THERM = 3601,
        UST_IN_WG_PER_100FT = 3701,
        UST_PASCAL_PER_M = 3801,
        UST_WATT = 3901,
        UST_KILOWATT = 4001,
        UST_BTU_PER_S = 4101,
        UST_BTU_PER_H = 4201,
        UST_CAL_PER_S = 4301,
        UST_KCAL_PER_S = 4401,
        UST_WATT_PER_SQ_FT = 4501,
        UST_WATT_PER_SQ_M = 4601,
        UST_IN_WG = 4701,
        UST_PASCAL = 4801,
        UST_KILOPASCAL = 4901,
        UST_MEGAPASCAL = 5001,
        UST_PSI = 5101,
        UST_LB_FORCE_PER_SQ_IN = 5102,
        UST_PSIG = 5103,
        UST_PSIA = 5104,
        UST_LBF_PER_SQ_IN = 5105,
        UST_IN_HG = 5201,
        UST_MM_HG = 5301,
        UST_ATM = 5401,
        UST_BAR = 5501,
        UST_DEGREE_F = 5601,
        UST_DEGREE_C = 5701,
        UST_KELVIN = 5801,
        UST_DEGREE_R = 5901,
        UST_FT_PER_MIN = 6001,
        UST_FPM = 6002,
        UST_M_PER_S = 6101,
        UST_CM_PER_MIN = 6201,
        UST_CU_FT_PER_MIN = 6301,
        UST_CFM = 6302,
        UST_L_PER_S = 6401,
        UST_LPS = 6402,
        UST_CU_M_PER_S = 6501,
        UST_CMS = 6502,
        UST_CU_M_PER_H = 6601,
        UST_CMH = 6602,
        UST_GAL_PER_MIN = 6701,
        UST_GPM = 6702,
        UST_USGPM = 6703,
        UST_GAL_PER_H = 6801,
        UST_GPH = 6802,
        UST_USGPH = 6803,
        UST_AMPERE = 6901,
        UST_KILOAMPERE = 7001,
        UST_MILLIAMPERE = 7101,
        UST_VOLT = 7201,
        UST_KILOVOLT = 7301,
        UST_MILLIVOLT = 7401,
        UST_HZ = 7501,
        UST_CPS = 7601,
        UST_LX = 7701,
        UST_FC = 7801,
        UST_FTC = 7802,
        UST_FL = 7901,
        UST_FL_LOWERCASE = 7902,
        UST_FTL = 7903,
        UST_CD_PER_SQ_M = 8001,
        UST_CD = 8101,
        UST_LM = 8301,
        UST_VOLTAMPERE = 8401,
        UST_KILOVOLTAMPERE = 8501,
        UST_HP = 8601,
        UST_N = 8701,
        UST_DA_N = 8801,
        UST_K_N = 8901,
        UST_M_N = 9001,
        UST_KIP = 9101,
        UST_KGF = 9201,
        UST_TF = 9301,
        UST_LB_FORCE = 9401,
        UST_LBF = 9402,
        UST_N_PER_M = 9501,
        UST_DA_N_PER_M = 9601,
        UST_K_N_PER_M = 9701,
        UST_M_N_PER_M = 9801,
        UST_KIP_PER_FT = 9901,
        UST_KGF_PER_M = 10001,
        UST_TF_PER_M = 10101,
        UST_LB_FORCE_PER_FT = 10201,
        UST_LBF_PER_FT = 10202,
        UST_N_PER_M_SUP_2 = 10301,
        UST_DA_N_PER_M_SUP_2 = 10401,
        UST_K_N_PER_M_SUP_2 = 10501,
        UST_M_N_PER_M_SUP_2 = 10601,
        UST_KSF = 10701,
        UST_KIP_PER_SQ_FT = 10702,
        UST_KGF_PER_M_SUP_2 = 10801,
        UST_TF_PER_M_SUP_2 = 10901,
        UST_PSF = 11001,
        UST_LB_FORCE_PER_SQ_FT = 11002,
        UST_LBF_PER_SQ_FT = 11003,
        UST_N_DASH_M = 11101,
        UST_DA_N_DASH_M = 11201,
        UST_K_N_DASH_M = 11301,
        UST_M_N_DASH_M = 11401,
        UST_KIP_DASH_FT = 11501,
        UST_KGF_DASH_M = 11601,
        UST_TF_DASH_M = 11701,
        UST_LB_FORCE_DASH_FT = 11801,
        UST_LBF_DASH_FT = 11802,
        UST_M_PER_K_N = 11901,
        UST_FT_PER_KIP = 12001,
        UST_M_SUP_2_PER_K_N = 12101,
        UST_FT_SUP_2_PER_KIP = 12201,
        UST_M_SUP_3_PER_K_N = 12301,
        UST_FT_SUP_3_PER_KIP = 12401,
        UST_INV_K_N = 12501,
        UST_INV_KIP = 12601,
        UST_FTH2O_PER_100FT = 12701,
        UST_FT_OF_WATER_PER_100FT = 12702,
        UST_FEET_OF_WATER_PER_100FT = 12703,
        UST_FTH2O = 12801,
        UST_FT_OF_WATER = 12802,
        UST_FEET_OF_WATER = 12803,
        UST_PA_S = 12901,
        UST_LB_FORCE_PER_FT_S = 13001,
        UST_LBM_PER_FT_S = 13002,
        UST_CP = 13101,
        UST_FT_PER_S = 13201,
        UST_FPS = 13202,
        UST_KSI = 13301,
        UST_KIP_PER_SQ_IN = 13302,
        UST_KN_PER_M_SUP_3 = 13401,
        UST_LB_FORCE_PER_CU_FT = 13501,
        UST_LBF_PER_CU_FT = 13502,
        UST_KIP_PER_IN_SUP_3 = 13601,
        UST_INV_DEGREE_F = 13701,
        UST_INV_DEGREE_C = 13801,
        UST_N_DASH_M_PER_M = 13901,
        UST_DA_N_DASH_M_PER_M = 14001,
        UST_K_N_DASH_M_PER_M = 14101,
        UST_M_N_DASH_M_PER_M = 14201,
        UST_KIP_DASH_FT_PER_FT = 14301,
        UST_KGF_DASH_M_PER_M = 14401,
        UST_TF_DASH_M_PER_M = 14501,
        UST_LB_FORCE_DASH_FT_PER_FT = 14601,
        UST_LBF_DASH_FT_PER_FT = 14602,
        UST_LB_FORCE_PER_FT_H = 14701,
        UST_LBM_PER_FT_H = 14702,
        UST_KIPS_PER_IN = 14801,
        UST_KIPS_PER_CU_FT = 14901,
        UST_KIP_FT_PER_DEGREE = 15001,
        UST_K_N_M_PER_DEGREE = 15101,
        UST_KIP_FT_PER_DEGREE_PER_FT = 15201,
        UST_K_N_M_PER_DEGREE_PER_M = 15301,
        UST_WATT_PER_SQ_M_K = 15401,
        UST_BTU_PER_H_SQ_FT_DEGREE_F = 15501,
        UST_CFM_PER_SQ_FT = 15601,
        UST_CFM_PER_SF = 15602,
        UST_LPS_PER_SQ_M = 15701,
        UST_L_PER_S_SQ_M = 15702,
        UST_COLON_10 = 15801,
        UST_COLON_12 = 15901,
        UST_SLOPE_DEGREE_SYMBOL = 16001,
        UST_WATT_PER_CU_FT = 16401,
        UST_WATT_PER_CU_M = 16501,
        UST_BTU_PER_H_SQ_FT = 16601,
        UST_BTU_PER_H_CU_FT = 16701,
        UST_TON = 16801,
        UST_TON_OF_REFRIGERATION = 16802,
        UST_CFM_PER_CU_FT = 16901,
        UST_CFM_PER_CF = 16902,
        UST_L_PER_S_CU_M = 17001,
        UST_CFM_PER_TON = 17101,
        UST_L_PER_S_KW = 17201,
        UST_SQ_FT_PER_TON = 17301,
        UST_SF_PER_TON = 17302,
        UST_SQ_M_PER_KW = 17401,
        UST_DOLLAR = 17501,
        UST_EURO_SUFFIX = 17502,
        UST_EURO_PREFIX = 17503,
        UST_POUND = 17504,
        UST_YEN = 17505,
        UST_CHINESE_HONG_KONG_SAR = 17506,
        UST_WON = 17507,
        UST_SHEQEL = 17508,
        UST_DONG = 17509,
        UST_BAHT = 17510,
        UST_KRONER = 17511,
        UST_LM_PER_W = 17601,
        UST_SF_PER_MBH = 17701,
        UST_SF_PER_KBTU_PER_H = 17702,
        UST_SQ_FT_PER_MBH = 17703,
        UST_SQ_FT_PER_KBTU_PER_H = 17704,
        UST_K_N_PER_CM_SUP_2 = 17801,
        UST_N_PER_MM_SUP_2 = 17901,
        UST_K_N_PER_MM_SUP_2 = 18001,
        UST_ONE_COLON = 18201,
        UST_H_SQ_FT_DEGREE_F_PER_BTU = 18401,
        UST_SQ_M_K_PER_WATT = 18501,
        UST_BTU_PER_F = 18601,
        UST_J_PER_KELVIN = 18701,
        UST_KJ_PER_KELVIN = 18801,
        UST_KGM = 18901,
        UST_TM = 19001,
        UST_LB_MASS = 19101,
        UST_LBM = 19102,
        UST_M_PER_SQ_S = 19201,
        UST_KM_PER_SQ_S = 19301,
        UST_IN_PER_SQ_S = 19401,
        UST_FT_PER_SQ_S = 19501,
        UST_MI_PER_SQ_S = 19601,
        UST_FT_SUP_4 = 19701,
        UST_IN_SUP_4 = 19801,
        UST_MM_SUP_4 = 19901,
        UST_CM_SUP_4 = 20001,
        UST_M_SUP_4 = 20101,
        UST_FT_SUP_6 = 20201,
        UST_IN_SUP_6 = 20301,
        UST_MM_SUP_6 = 20401,
        UST_CM_SUP_6 = 20501,
        UST_M_SUP_6 = 20601,
        UST_SQ_FT_PER_FT = 20701,
        UST_SQ_IN_PER_FT = 20801,
        UST_SQ_MM_PER_M = 20901,
        UST_SQ_CM_PER_M = 21001,
        UST_SQ_M_PER_M = 21101,
        UST_KGM_PER_M = 21201,
        UST_LB_MASS_PER_FT = 21301,
        UST_LBM_PER_FT = 21302,
        UST_RAD = 21401,
        UST_GRAD = 21501,
        UST_RAD_PER_S = 21601,
        UST_MS = 21701,
        UST_S = 21801,
        UST_MIN = 21901,
        UST_H = 22001,
        UST_KM_PER_H = 22101,
        UST_MI_PER_H = 22201,
        UST_KJ = 22301,
        UST_KGM_PER_SQ_M = 22401,
        UST_LBM_PER_SQ_FT = 22501,
        UST_WATTS_PER_METER_KELVIN = 22601,
        UST_J_PER_G_CELSIUS = 22701,
        UST_J_PER_G = 22801,
        UST_NG_PER_PA_S_SQ_M = 22901,
        UST_OHM_M = 23001,
        UST_BTU_PER_H_FT_DEGREE_F = 23101,
        UST_BTU_PER_LB_DEGREE_F = 23201,
        UST_BTU_PER_LB = 23301,
        UST_GR_PER_H_SQ_FT_IN_HG = 23401,
        UST_PER_MILLE_SIGN = 23501,
        UST_DM = 23601,
        UST_J_PER_KG_CELSIUS = 23701,
        UST_UM_PER_M_C = 23801,
        UST_UIN_PER_IN_F = 23901,
        UST_USTONNES_MASS_TONS = 24001,
        UST_USTONNES_MASS_T = 24002,
        UST_USTONNES_MASS_ST = 24003,
        UST_USTONNES_FORCE_TONSF = 24101,
        UST_USTONNES_FORCE_STF = 24102,
        UST_USTONNES_FORCE_AS_MASS_TONS = 24103,
        UST_USTONNES_FORCE_AS_MASS_T = 24104,
        UST_USTONNES_FORCE_AS_MASS_ST = 24105,
        UST_L_PER_M = 24201,
        UST_LPM = 24202,
        UST_DEGREE_F_DIFFERENCE = 24301,
        UST_DELTA_DEGREE_F = 24302,
        UST_DEGREE_C_DIFFERENCE = 24401,
        UST_DELTA_DEGREE_C = 24402,
        UST_KELVIN_DIFFERENCE = 24501,
        UST_DELTA_KELVIN = 24502,
        UST_DEGREE_R_DIFFERENCE = 24601,
        UST_DELTA_DEGREE_R = 24602,
        UST_US_SURVEY_FT = 60501
    }
}
