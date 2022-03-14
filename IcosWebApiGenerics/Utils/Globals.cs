using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Utils
{
    public static class Globals
    {
        static int[] allowedOutside = { 10, 17, 19, 21, 22 };
        static List<int> notSP_II_Valid = new List<int>() { 21, 22 };

        public static string taReg = @"^[a-zA-Z]{2}\-[a-zA-Z0-9]{3}_(TA).[a-zA-Z]{3}$";
        public static string eaReg = @"[a-zA-Z]{2}\-[a-zA-Z0-9]{3}_(EA)([0][1-9]|[1][0-9]|20).[a-zA-Z]{3}$";
        public static string cpReg = @"^(CP)_([0][1-9]|[1][0-9]|[2][0-9]|[3][0-9]|[4][0-9]|50)$";
        public static string sp1Reg = @"^(SP-I)_([0][1-9]|[1][0-9]|20)$";
        public static string sp2Reg = @"^(SP-II)_([0][1-9]|[1][0-9]|20)-([0][1-9]|[1][0-9]|20)$";
        public static string varMapReg = @"^([a-zA-Z0-9_]{1,})_([1-9]|[1-9][0-9]|100)_([1-9]|[1-9][0-9]|100)_([1-9]|[1-9][0-9]|100)$";
        public static string spiSosmM = @"^[a-zA-Z]{2}\-[a-zA-Z0-9]{3}_[0-9]{8}_([0][1-9]|[1][0-9]|20)_([0-9]|[1-9][0-9]|100)_([0-9]|[1-9][0-9]|100)$";//BE-Lcr_20170608_04_30_60
        public static string spiiSosmM = @"^[a-zA-Z]{2}\-[a-zA-Z0-9]{3}_[0-9]{8}_([0][1-9]|[1][0-9]|20)-([0][1-9]|[1][0-9]|20)_([0-9]|[1-9][0-9]|100)_([0-9]|[1-9][0-9]|100)$";//BE-Lcr_20170608_01-05_30_60
        public static string spiSosmOrganic = @"^[a-zA-Z]{2}\-[a-zA-Z0-9]{3}_[0-9]{8}_([0][1-9]|[1][0-9]|20)_(O|Oa|Oe|Oi)_([0-9]|[1-9][0-9]|100)$";
        public static string spiiSosmOrganic = @"^[a-zA-Z]{2}\-[a-zA-Z0-9]{3}_[0-9]{8}_([0][1-9]|[1][0-9]|20)-([0][1-9]|[1][0-9]|20)_(O|Oa|Oe|Oi)_((\d{0,2}(\.\d{1,2})?|100(\.00?)?))$";
        public static string wtdPattern = @"^(WTD)_([1-9]|[1][0-9]|20)_([1-9]|[1][0-9]|20)_([1-9]|[1][0-9]|20)$";
        public static string dsnowPattern = @"^(D_SNOW)_([1-9]|[1][0-9]|20)_([1-9]|[1][0-9]|20)_([1-9]|[1][0-9]|20)$";

        public static readonly decimal MIN_UTC_OFFSET_VAL = -12;
        public static readonly decimal MAX_UTC_OFFSET_VAL = 14;
        public static readonly decimal MAX_LONGITUDE_VALUE = 180;
        public static readonly decimal MAX_LATITUDE_VALUE = 90;
        public static readonly decimal MIN_ALLOWED_TEMPERATURE = -273.15M;
        public static readonly decimal MAX_PERCENT = 100;
        public enum Groups
        {
            GRP_HEADER = 1,
            GRP_TEAM,
            GRP_LOCATION,
            GRP_UTC_OFFSET,
            GRP_LAND_OWNERSHIP,
            GRP_TOWER,
            GRP_CLIM_AVG,
            GRP_DM,
            GRP_PLOT,
            GRP_FLSM,
            GRP_SOSM,
            GRP_DHP,
            GRP_GAI,
            GRP_CEPT,
            GRP_BULKH,
            GRP_SPPS,
            GRP_TREE,
            GRP_AGB,
            GRP_LITTERPNT,
            GRP_ALLOM,
            GRP_WTDPNT,
            GRP_D_SNOW,
            GRP_INST = 2000,
            GRP_LOGGER,
            GRP_FILE,
            GRP_EC,
            GRP_ECSYS,
            GRP_BM,
            GRP_STO,
            GRP_ECWEXCL = 2014,
            GRP_CHEMICAL_DATA = 2999,
            GRP_SPP_ASS,
            GRP_LAI,
            GRP_BIOMASS,
            GRP_HEIGHTC,
            GRP_SA,
            GRP_DBH,
            GRP_BASAL_AREA,
            GRP_TREES_NUM,
            GRP_ROOT_DEPTH,
            GRP_PHEN_EVENT_TYPE,
            GRP_IGBP,
            GRP_RESEARCH_TOPIC,
            GRP_SITE_FUNDING,
            GRP_URL,
            GRP_ACKNOWLEDGEMENT,
            GRP_FLUX_MEASUREMENTS,
            GRP_SITE_CHAR1,
            GRP_SITE_CHAR2,
            GRP_SITE_CHAR3,
            GRP_SITE_CHAR4,
            GRP_SITE_CHAR5,
            GRP_SOIL_CHEM,
            GRP_SOIL_STOCK,
            GRP_SOIL_TEX,
            GRP_PFCURVE,
            GRP_WTD_ASS,
            GRP_SWC,
            GRP_SOIL_WRB_GROUP,
            GRP_SOIL_ORDER,
            GRP_SOIL_CLASSIFICATION,
            GRP_SOIL_SERIES,
            GRP_SOIL_DEPTH,
            GRP_LITTER_ASS,
            GRP_SITE_DESC,
            GRP_SITE_NAME,
            GRP_SITE_ICOS_CLASS,
            GRP_CLIMATE_KOEPPEN,
            GRP_REFERENCE_PAPER,
            GRP_VAR_INFO = 4000,
            GRP_VAR_AGG,
            GRP_INSTMAN = 5000,
            GRP_INSTPAIR,
            GRP_ICOS,
            GRP_FUNDING,
            GRP_NETWORK
        }
        public enum CvIndexes
        {
            ASPECT = 1,
            DIR,
            DIST_MGMT,
            FLUX_METHOD,
            FLUX_OPERATIONS,
            FLUX_VARIABLE,
            IGBP,
            LAND_OWNERSHIP,
            NETWORK,
            REFERENCE_USAGE,
            TEAM_ROLE = 11,
            TERRAIN,
            TOWER_POWER,
            TOWER_TYPE,
            PROFILE_ZERO_REF = 22,
            SOIL_CLASS_TAXON = 25,
            SOIL_GROUP,
            SOIL_ORDER,
            UNIT_SWC,
            YES_NO,
            SA_WIND_FORMAT = 31,
            GA_CP_THERMAL = 33,
            INST_ASPIRATION,
            INST_HEAT,
            //VAR_CODE=40,
            CLIMATE_KOEPPEN = 42,
            TEAM_EXP,
            TEAM_PERC,
            TEAM_CONTR,
            PLOTYPE,
            PLOTREF,
            TOWER_ACCESS,
            TOWER_DATATRAN,
            DM_AGRICULTURE,
            DM_ENCROACH,
            DM_EXT_WEATHER,
            DM_FERT_M,
            DM_FERT_O,
            DM_FIRE,
            DM_FORESTRY,
            DM_GRAZE,
            DM_INS_PATH,
            DM_PESTICIDE,
            DM_PLANTING,
            DM_TILL,
            DM_WATER,
            DM_GENERAL,
            INST_MODEL,
            INST_FACTORY,
            FILE_FORMAT,
            FILE_TYPE,
            EC_TYPE,
            EC_YN,
            SA_ALIGN,
            SA_FORMAT,
            BM_TYPE,
            VAR_CODE,
            BM_YN,
            BM_SHIELDING,
            BM_ASPIRATION,
            STO_VAR,
            STO_CONFIG,
            STO_TUBEMAT,
            STO_TUBETHERM,
            STO_TYPE,
            EC_INST,
            LOGGER_MODEL,
            BM_MODEL,
            STO_INST,
            CAMERA,
            LENS,
            FILE_EXT,
            FILE_COMPR,
            FILE_MISSING,
            FILE_TIME,
            GAIMETHOD,
            GAIPTYPE,
            PLOTTYPE,
            PHEN,
            TWSP,
            SAMPLEMAT,
            TREE_STATUS,
            SPPPTYPE,
            AGBPTYPE,
            AGBORGAN,
            LITTYPE,
            LITFRAC,
            LAI_METHOD,
            LAI_TYPE,
            LAI_CANOPY_TYPE,
            BIOM_ORGAN,
            BIOM_ORGAN_PHEN,
            PHEN_EVENT,
            PHEN_STATUS,
            UNIT_BIOMASS,
            UNIT_SPP,
            VEGTYPE,
            STATISTIC,
            STATISTIC_TYPE,
            VAR_UNIT,
            AGG_STATISTIC,
            LITTER_UNIT,
            LITTER_ORGAN,
            INSTMAN_TYPE,
            INSTMAN_MODEL,
            GAI_DHP_QC,
            SITE_ICOS_CLASS,
            GAI_DHP_QC_EXT,
            VEG_STATUS,
            LIFESTAGE,
            GA_CP_MATERIAL,
            PREVALENCE,
            STATISTIC_METHOD,
            ACTION,
            FLSM_STYPE,
            FLSM_PTYPE
        };

        /*58,@"ERROR - Column $CELL$; for GRP_PLOT you have to submit either PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR or PLOT_LOCATION_LAT <---> PLOT_LOCATION_LONG."+
                  Environment.NewLine + "These couples of information are mutually exclusive"}*/
        public enum ErrorValidationCodes
        {
            MISSING_REQUIRED_FIELD = 1,
            PLOT_ID_PLOT_TYPE_MISMATCH,
            PLOT_ID_INVALID_FORMAT,
            PLOT_REF_POINT_ERROR,
            PLOT_REF_SP_II_ALLOWED,
            PLOT_REF_SP_II_MISSING,
            DATES_CONSTRAINTS_ERROR,
            DATE_START_DATE_END_ERROR,
            NO_VALUE_SUBMITTED = 10,
            DM_SURF_DM_SURF_PRECISION,
            TEAM_MEMBER_WE_NOT_FOUND,
            PI_MANAGER_ALREADY_REGISTERED,
            MANAGER_WORKEND_DATE_ERROR,
            MANAGER_WORKEND_DATE_ERROR_PLUS_1,
            TEAM_MEMBER_ALREADY_REGISTERED,
            NOT_IN_GRP_INST = 34,
            INST_PURCHASE_DATE_GREATER_THAN_INST_OP_DATE,
            PLOT_MISSING = 50,
            INVALID_COORDINATE_SYSTEM = 58,
            GAI_SPP_MISSING_MIRES = 63,
            GAI_ONLY_MIRES = 64,
            GAI_PLOT_TYPE_ACCUPAR_GRASSLAND_MANDATORY = 65,
            GAI_DHP_ONLY_FOREST = 66,
            GAI_SPP_CROP_CROPLAND = 67,
            GAI_PTYPE_WEED_CROP_INVALID_CROPLAND = 68,
            GAI_LOCATION_MISSING_CROPLAND = 69,
            GAI_PTYPE_MISSING_CROPLAND = 70,
            GAI_PTYPE_WEED_CROP_INVALID_GRASSLAND = 71,
            GAI_PLOT_TYPE_MISSING_GRASSLAND = 72,
            GAI_AREA_MISSING_GRASSLAND = 73,
            GAI_DHP_MANDATORY_MISSING,
            GAI_DHP_MISSING_COORDINATES,
            GAI_DHP_MISSING_POSSIBLE_COORDINATES,
            GAI_MISSING_SPECTRAL_REF,
            GAI_MISSING_GRASSLAND,
            GRP_INST_NOT_PURCHASE = 79,
            GAI_ACCUPAR,
            GAI_CEPT_MANDATORY_MISSING,
            GAI_CEPT_ID_MANDATORY_MISSING,
            GAI_PLOT_TYPE_SUNSCAN,
            GAI_SUNSCAN_ECOSYSTEMS,
            CEPT_FIRST_LAST,
            PLOT_ID_NOT_FOUND,
            TREE_ID_MANDATORY_CP,
            TREE_MANDATORY_VARIABLES,
            SPPS_PLOT_NOT_FOUND,
            SPPS_LOCATION_BOUND,
            SPPS_PTYPE_CP_ALLOWED,
            SOSM_PLOT_ID_NOT_FOUND,
            SOSM_SAMPLE_MAT_O_NOT_ALLOWED,
            SOSM_UD_LD,
            SOSM_AREA_VOLUME,
            SOSM_THICKNESS_AREA_VOLUME,
            SOSM_CP_NOT_ALLOWED,
            SOSM_INVALID_SPI_M,
            SOSM_INVALID_SPII_M,
            SOSM_INVALID_SPII_O,
            FLSM_SAMPLE_TYPE_REQUIRED,
            FLSM_SAMPLE_ID_REQUIRED,
            FLSM_SPP_PTYPE_XOR,
            FLSM_PLOT_ID_NOT_FOUND,
            AGB_PLOT_NOT_FOUND,
            AGB_CROPLAND_MANDATORY,
            AGB_GRASSLAND_MANDATORY,
            AGB_FOREST_AGB_PTYPE_MANDATORY,
            AGB_FOREST_AGB_LOCATION_MANDATORY,
            AGB_FOREST_AGB_XOR_AGB_NPP_MOSS,
            AGB_FOREST_AGB_PHEN_MANDATORY,
            AGB_FOREST_SAPLING_MANDATORY,
            AGB_FOREST_FHS_MANDATORY,
            AGB_MIRES_SPP,
            AGB_MIRES_AGB_XOR_AGB_NPP_MOSS,
            AGB_MIRES_AGB_ORGAN,
            WRONG_SERIALNUMBER_FORMAT = 118,
            GRP_INST_ALREADY_PURCHASED = 123,
            LOGGER_ID_NOT_REGISTERED = 126,
            FILE_ID_LOGGER_ID_ALREADY_REGISTERED = 127,
            LITTERPNT_PLOT_NOT_FOUND,
            LITTERPNT_AREA_GRASSLAND_MANDATORY,
            LITTERPNT_CROP_NATURAL_MANDATORY,
            LITTERPNT_CROP_RESIDUAL_MANDATORY,
            LITTERPNT_LITTERTYPE_NOT_ALLOWED,
            LITTERPNT_FOREST_COARSE_MANDATORY,
            LITTERPNT_FOREST_FINE_WOODY_MANDATORY,
            LITTERPNT_FOREST_NON_WOODY_MANDATORY,
            LITTERPNT_FOREST_SPP_MANDATORY,
            WTDPNT_PLOT_NOT_FOUND,
            WTDPNT_PLOT_VARMAP,
            D_SNOW_PLOT_NOT_FOUND,
            D_SNOW_PLOT_VARMAP,
            MISSING_VARIABLES = 200,
            XOR_VARIABLES,
            INSTALLATION_ONLY_DATE = 219,
            EC_MANDATORY_MISSING,
            EC_SA_MANDATORY_MISSING,
            SAMPLING_LOGGER_FILE_ERROR = 222,
            INST_ALREADY_INSTALLED,
            INST_ALREADY_INSTALLED_NOT_REMOVED,
            INST_NOT_VALID_OPERATION,
            INST_ALREADY_REMOVED,
            EC_GA_FIELD_CALIBRATION,
            CSV_NOT_ALLOWED_FOR_BINARY = 234,
            BIN_NOT_ALLOWED_FOR_ASCII,
            FILE_HEAD_VARS_MISSING,
            FILE_HEAD_NUM_MISSING,
            FILE_HEAD_VARS_GT_FILE_HEAD_NUM,
            FILE_HEAD_TYPE_EQ_FILE_HEAD_VARS,
            FILE_HEAD_VARS_TYPE_MISSING_BINARY,
            FILE_HEAD_NUM_TYPE_MISSING_BINARY,
            FILE_HEAD_NUM_VARS_MISSING_BINARY,
            FILE_HEAD_TYPE_GT_FILE_HEAD_NUM,
            FILE_HEAD_TYPE_GT_FILE_HEAD_VARS,
            FILE_FORMAT_FILEXT,
            FIE_FORMAT_HEAD_VARS_TYPE,
            SA_GA_MISSING_IN_EC_GROUP = 250,
            ECSYS_DATE_WRONG,
            BM_VARIABLE_H_V_R_WRONG_FORMAT,
            BM_VARIABLE_H_V_R_NOT_FOUND,
            BM_SENSOR_NOT_REMOVED,
            BM_SENSOR_NOT_INSTALLED,
            BM_INSTALLATION_HEIGHT_EAST_NORTH_MANDATORY,
            BM_INSTALLATION_VAR_SAMPLING_LOGGER_FILE,
            BM_VARMAP_SAMPLING_FILE_LOGGER_MANDATORY,
            BM_LOGGER_FILE_MISSING_IN_GRP_FILE,
            BM_DIFFERENT_SAMPLING_INT,
            BM_VAR_MAPPED_DIFFERENT_INST,
            BM_SENSOR_FOLLOWING_OPERATION,
            STO_GA_INSTALLATION_MANDATORY = 300,
            STO_GA_INSTALLATION_CP_TUBE_MANDATORY,
            STO_GA_INSTALLATION_OP_TUBE_NOT_ALLOWED,
            STO_GA_MOUNT_CP_NOT_ALLOWED,
            STO_GA_FLOW_RATE_OP_NOT_ALLOWED,
            STO_GA_MOUNT_OP_ERROR,
            STO_GA_MOUNT_OP_NOT_SEPARATE_ERROR,
            STO_GA_PROF_ID_NOT_FOUND_OR_REMOVED,
            STO_GA_PROF_ID_NOT_SEQUENTIAL_ERROR,
            STO_GA_REMOVAL_MANDATORY_ERROR,
            STO_GA_REMOVAL_GA_NOT_FOUND,
            STO_GA_CAL_BOUND,
            STO_LEVEL_INSTALLATION_MANDATORY,
            STO_LEVEL_REMOVAL_MANDATORY_ERROR,
            STO_LEVEL_REMOVAL_PROF_ID_NOT_FOUND,
            STO_PROF_LEVEL_ALREADY_SUBMITTED,
            STO_ALL_PROF_VARS_MANDATORY,
            STO_SEQ_SAMPLING_INT,
            STO_VARIABLE_MAP_MANDATORY,
            STO_FIELD_FIRM_MANDATORY_ERROR,
            STO_GA_MODEL_SN_ERROR,
            STO_DATE_NOT_ALLOWED,
            STO_DATE_START_END_ERROR,
            STO_SENSOR_NOT_REMOVED,
            STO_SENSOR_NOT_INSTALLED,
            STO_LEVEL_NOT_INSTALLED,
            STO_LEVEL_NOT_REMOVED,
            STO_SENSOR_FOLLOWING_OPERATION,
            STO_LEVEL_FOLLOWING_OPERATION,
            STO_GA_PROF_NOT_ALLOWED,
            STO_MAINTENANCE_PROF_SAMPLING_INT,
            STO_MAINTENANCE_PROF_LEVEL_NULL,
            STO_MAINTENANCE_BUFFER_FR_NON_ZERO,
            STO_GA_FLOW_TUBE_ERROR,
            STO_LOGGER_FILE_MISSING_IN_GRP_FILE,
            STO_DIFFERENT_SAMPLING_INT,
            INSTMAN_SENSOR_NOT_INSTALLED,
            INSTMAN_SENSOR_NOT_REMOVED,
            INSTMAN_SENSOR_FOLLOWING_OPERATION,
            INSTMAN_VAR_MAPPED_DIFFERENT_INST,
            INSTMAN_INSTALLATION_HEIGHT_EAST_NORTH_MANDATORY,
            INSTMAN_INSTALLATION_VAR_SAMPLING,
            INSTMAN_VARMAP_SAMPLING,
            INSTPAIR_NOT_IN_GRP_INSTMAN,
            INSTPAIR_SAME_SENSORS,
            INSTPAIR_SEP_MANDATORY
        };


        //DIEGO:: consider to move in ValidationUtils class
        public static bool DatesCompare(string _dateMin, string _dateMax)
        {
            //min should be <= max: if >, return true (error)
            return (String.Compare(_dateMin, _dateMax, true) > 0);
        }

        public static int IsValidCoordinateSystem<T>(params T[] coordinates)
        {
            int nCoo = coordinates.ToList().Count(v => v != null);
            if (nCoo != 2) return 1;

            int index1 = coordinates.ToList().FindIndex(v => v != null);
            int index2 = coordinates.ToList().FindLastIndex(v => v != null);
            if (((index2 - index1) != 1) || (index1 % 2 != 0)) return 1;
            return 0; // index1 / 2; 
        }

        public static bool IsValidPlotId(string plotId, int groupId)
        {
            bool isMatch = true;
            try
            {
                Match match;

                if (plotId.ToLower().StartsWith("cp"))
                {
                    match = Regex.Match(plotId, Globals.cpReg, RegexOptions.IgnoreCase);
                    isMatch = match.Success;
                }
                else if (plotId.ToLower().StartsWith("sp-i_"))
                {
                    match = Regex.Match(plotId, Globals.sp1Reg, RegexOptions.IgnoreCase);
                    isMatch = match.Success;
                }
                else if (plotId.ToLower().StartsWith("sp-ii"))
                {
                    if (notSP_II_Valid.Any(id => id == groupId))
                    {
                        isMatch = false;
                    }
                    else
                    {
                        match = Regex.Match(plotId, Globals.sp2Reg, RegexOptions.IgnoreCase);
                        isMatch = match.Success;
                    }
                }
                else
                {
                    if (String.Compare(plotId, "outside_cp", true) == 0)
                    {
                        //check for which groups 'outside_cp' is allowed
                        if (allowedOutside.Contains(groupId))
                        {
                            isMatch = true;
                        }
                    }
                    else
                    {
                        isMatch = false;
                    }
                }
            }
            catch (Exception dd)
            {
                isMatch = false;
            }

            return isMatch;
        }

       /* public static void FormatError(ref string err, params string[] list)
        {
            for (int i = 0; i < list.Length; i += 2)
            {
                err = err.Replace(list[i], list[i + 1]);
            }
        }*/
    }
}
