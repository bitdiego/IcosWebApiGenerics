using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Utils
{
    public static class ErrorCodes
    {
        public static Dictionary<int, string> GeneralErrors = new Dictionary<int, string>
        {
            { 1, "$V0$ is mandatory for variable $GRP$"},
            { 2, "$V0$ has a wrong ISODATE format. Found value: $V1$"},
            { 3, "$V0$ is outside a valid range"},
            { 4, "$V0$ and $V1$/$V2$ are mutually exclusive"},
            { 5, "$V0$ and $V1$ must be both submitted"},
            { 6, "$V0$ must be greater than $V1$"},
            { 7, "Not found $V0$ value in list $V1$ for group $GRP$"},
            { 8, "Negative number not allowed for variable $V0$. Found $V1$"},
            { 9, "You have to submit at least one variable"},
            {10,"Error: $V0$ string does not have correct format: It must be CP_## or SP-I_## or SP-II_##-## where ## range from 01 to 20. Found value: $V1$"},
            {48,"Error: integer number expected; found $V0$ for variable $V1$"},
            {49,"Error: decimal number expected; found $V0$ for variable $V1$"},
            {86,"Error: entered plot ID not found in GRP_PLOT"},
            {118,"Error: Serial number wrong format. Correct format: $V0$, where X is a digit. Found value: $V1$"},
            {200,"Error; Missing $V0$, $V1$ and $V2$ variables. Please, enter either $V0$ or $V1$/$V2$"},
            {201,"Error; Found $V0$ together with $V1$ and/or $V2$ variables. Please, enter either $V0$ or $V1$/$V2$"},
            {202,"Error; DATE_END is missing. Please, enter either DATE or DATE_START / DATE_END"},
            {203,"Error; DATE_START is missing. Please, enter DATE_START value"},
            {204,"Error; DATE_END must be greater than DATE_START"},
        };

        public static Dictionary<int, string> GrpLocationErrors = new Dictionary<int, string>
        {
           // { 1, "LOCATION_DATE is mandatory"},
           // { 2, "LOCATION_LAT is mandatory"},
          //  { 3, "LOCATION_LONG is mandatory"}, 
           // { 4, "LOCATION_DATE has a wrong ISODATE format. Found value: $V0$"},
            { 5, "LOCATION_LONG must be between -180 and 180. Found value: $V0$"},
            { 6, "LOCATION_LAT must be between -90 and 90. Found value: $V0$"}
        };

        public static Dictionary<int, string> GrpUtcErrors = new Dictionary<int, string>
        {
            { 1, "UTC_OFFSET is out of allowed values. Found value: $V0$"},
           // { 2, "UTC_OFFSET_DATE_START has a wrong ISODATE format. Found value: $V0$"}
        };

        public static Dictionary<int, string> GrpLandOwnerErrors = new Dictionary<int, string>
        {
           // { 1, "LAND_OWNERSHIP value $V0$ not found in list of allowed values."},
            //{ 2, "LAND_DATE has a wrong ISODATE format. Found value: $V0$"}
        };

        public static Dictionary<int, string> GrpTowerErrors = new Dictionary<int, string>
        {
            //{ 1, "TOWER_DATE is mandatory."},
            //{ 2, "TOWER_DATE has a wrong ISODATE format. Found value: $V0$"},
           // { 3, "TOWER_TYPE value $V0$ not found in list of allowed values."},
           // { 4, "TOWER_ACCESS value $V0$ not found in list of allowed values."},
           // { 5, "TOWER_POWER value $V0$ not found in list of allowed values."},
           // { 6, "TOWER_DATATRAN value $V0$ not found in list of allowed values."}
        };

        public static Dictionary<int, string> GrpClimateAvgErrors = new Dictionary<int, string>
        {
            //{ 1, "MAC_DATE is mandatory."},
           // { 2, "MAC_DATE has a wrong ISODATE format. Found value: $V0$"}
        };

        public static Dictionary<int, string> GrpDisturbanceErrors = new Dictionary<int, string>
        {
           // { 1, "DM_DATE has a wrong ISODATE format. Found value: $V0$"},
           // { 2, "DM_DATE_START has a wrong ISODATE format. Found value: $V0$"},
          //  { 3, "DM_DATE_END has a wrong ISODATE format. Found value: $V0$"},
           // { 4, "DM_DATE and DM_DATE_START/DM_DATE_END are mutually exclusive"},
           // { 5, "DM_DATE_START and DM_DATE_END must be both submitted"},
          //  { 6, "DM_DATE_END must be greater than DM_DATE_START"}
        };

        public static Dictionary<int, string> GrpSamplingSchemeErrors = new Dictionary<int, string>
        {
            {1,"Mismatch between PLOT_ID and PLOT_TYPE: expected $V0$, found $V1$"},
            {2,@"Error: for GRP_PLOT you have to submit either PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR or PLOT_LOCATION_LAT <---> PLOT_LOCATION_LONG."+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {3,"Error: PLOT_REFERENCE_POINT is allowed only for SP-II sampling points"},
            {4,"Error: PLOT_REFERENCE_POINT is not allowed when PLOT_LOCATION_LAT and PLOT_LOCATION_LONG are submitted"},
            {5,"Error: PLOT_REFERENCE_POINT is mandatory for SP-II sampling points when PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR are submitted"},
            {6,"Error: PLOT_ID is mandatory for SP-II sampling points when PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR are submitted"},
        };

        public static Dictionary<int, string> GrpFlsmErrors = new Dictionary<int, string>
        {
            {1,"Error: FLSM_SPP and FLSM_PTYPE are mutually exclusive for GRP_FLSM"},
            {2,"Error: entered FLSM_PLOT_ID not found in GRP_PLOT"},
            {3,"Error: entered FLSM_PLOT_ID not found in GRP_PLOT"},
        };

        public static Dictionary<int, string> GrpSosmErrors = new Dictionary<int, string>
        {
            {1,"Error: entered SOSM_PLOT_ID not found in GRP_PLOT"},
            {2,"Error: SOSM_SAMPLE_MAT organic options (O, Oi, Oa, Oe) are not allowed when SOSM_PLOT_ID is of type SP-I"},
            {3,"Error: SOSM_UD and SOSM_SD must be submitted together when SOSM_SAMPLE_MAT is M"},
            {4,"Error: SOSM_AREA and SOSM_VOLUME must be submitted together when SOSM_SAMPLE_MAT is M and SOSM_PLOT_ID is of type SP-II"},
            {5,"Error: SOSM_AREA, SOSM_VOLUME and SOSM_THICKNESS must be submitted together when SOSM_SAMPLE_MAT is in (O, Oi, Oa, Oe) and SOSM_PLOT_ID is of type SP-II"},
            {6,"Error: CP_XX not allowed for SOSM_PLOT_ID"},
            {7,"Error: Wrong format for SOSM_SAMPLE_ID: must be in the format CC-Xxx_YYYYMMDD_NN_AA_BB when SOSM_PLOT_ID is of type SP-I and SOSM_SAMPLE_MAT is M"},
            {8,"Error: Wrong format for SOSM_SAMPLE_ID: must be in the format CC-Xxx_YYYYMMDD_NN-MM_AA_BB when SOSM_PLOT_ID is of type SP-II and SOSM_SAMPLE_MAT is M"},
            {9,"Error: Wrong format for SOSM_SAMPLE_ID: must be in the format CC-Xxx_YYYYMMDD_NN-MM_(O|Oa|Oe|Oi)_BB when SOSM_PLOT_ID is of type SP-II and SOSM_SAMPLE_MAT is in (O, Oi, Oa, Oe)"},
        };

        

        public static Dictionary<int, string> GrpDhpErrors = new Dictionary<int, string>
        {
            {1,"Error: DHP_OC_COL cannot be less than DHP_OC_ROW"}
        };

        public static Dictionary<int, string> GrpGaiErrors = new Dictionary<int, string>
        {
            {58,@"Error: for GRP_PLOT you have to submit either PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR or PLOT_LOCATION_LAT <---> PLOT_LOCATION_LONG."+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {63,"Error: GAI_SPP is mandatory for Mires Ecosystem and Visual estimation and Modified VGA GAI_METHODs"},
            {64,"Error: Visual estimation and Modified VGA GAI_METHODs can be used only for Mires Ecosystem"},
            {65,"Error: GAI_PLOT_TYPE is mandatory when GAI_METHOD is Accupar for Grassland Ecosystem"},
            {66,"Error: DHP GAI_METHOD is allowed only for Forest Ecosystem"},
            {67,"Error: GAI_SPP can be reported only when GAI_PTYPE is Crop for Cropland Ecosystem when GAI_METHOD is Destructive"},
            {68,"Error: GAI_PTYPE for Cropland Ecosystem must be Weed or Crop when GAI_METHOD is Destructive"},
            {69,"Error: GAI_LOCATION is required for Cropland Ecosystem when GAI_METHOD is Destructive"},
            {70,"Error: GAI_PTYPE is required for Cropland Ecosystem when GAI_METHOD is Destructive"},
            {71,"Error: GAI_PTYPE Crop and Weed are not allowed for Grassland Ecosystem when GAI_METHOD is Destructive"},
            {72,"Error: GAI_PLOT_TYPE is mandatory for Grassland Ecosystem when GAI_METHOD is Destructive"},
            {73,"Error: GAI_AREA is mandatory for Grassland or Cropland Ecosystem when GAI_METHOD is Destructive"},
            {74,"Error: GAI_DHP_ID, GAI_DHP_SLOPE, GAI_DHP_ASPECT are mandatory for Forest Ecosystem when GAI_METHOD is DHP"},
            {75,"Error: GAI_DHP_EASTWARD_DIST<--->GAI_DHP_NORTHWARD_DIST and GAI_DHP_DISTANCE_POLAR<---->GAI_DHP_ANGLE_POLAR are mutually exclusive and are mandatory for forest ecosystems when GAI_METHOD is DHP and GAI_PLOT is CP"},
            {76,"Error: GAI_DHP_EASTWARD_DIST<--->GAI_DHP_NORTHWARD_DIST and GAI_DHP_DISTANCE_POLAR<---->GAI_DHP_ANGLE_POLAR are mutually exclusive and are possible for forest ecosystems when GAI_METHOD is DHP and GAI_PLOT is CP"},
            {77,"Error: GAI is mandatory for Visual estimation, Modified VGA or Spectral reflectance GAI_METHODs"},
            {78,"Error: GAI is mandatory for Grassland or Cropland ecosystem and GAI_METHOD is Destructive"},
            {80,"Error: GAI_METHOD Accupar is allowed only for Grassland or Cropland ecosystems"},
            {81,"Error: GAI_CEPT_EASTWARD_DIST<--->GAI_CEPT_NORTHWARD_DIST and GAI_CEPT_DISTANCE_POLAR<---->GAI_CEPT_ANGLE_POLAR are mutually exclusive and are mandatory for forest ecosystems when GAI_METHOD is SUNSCAN and GAI_PLOT is CP"},
            {82,"Error: GAI_CEPT_ID is mandatory for forest ecosystems when GAI_METHOD is SUNSCAN and GAI_PLOT is SP"},
            {83,"Error: GAI_PLOT_TYPE is mandatory for Grassland ecosystems when GAI_METHOD is SUNSCAN"},
            {84,"Error: SUNSCAN GAI_METHOD is allowed only for Forest, Grassland and Cropland ecosystems"},
            {4,"Error: GAI_DHP_EASTWARD_DIST<--->GAI_DHP_NORTHWARD_DIST or GAI_DHP_DISTANCE_POLAR<---->GAI_DHP_ANGLE_POLAR are mandatory and mutually exclusive for forest ecosystems when GAI_METHOD is DHP and GAI_PLOT is CP"},

        };

        public static Dictionary<int, string> GrpSppsErrors = new Dictionary<int, string>
        {
            {2,@"Error: for GRP_SPPS you have to submit either SPPS_LOCATION_LAT <---> SPPS_LOCATION_LON, or SPPS_LOCATION_DIST <---> SPPS_LOCATION_ANG"+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {90,"Error: Either (SPPS_LOCATION, SPPS_LOCATION_LAT, SPPS_LOCATION_LON) or (SPPS_LOCATION, SPPS_LOCATION_DIST, SPPS_LOCATION_ANG) must  be submitted together for Inaccessible Mires"},
            {91,"Error: SPPS_PTYPE is allowed only for CP SPPS_PLOT and for Mires"},
        };

        public static Dictionary<int, string> GrpTreeErrors = new Dictionary<int, string>
        {
            {2,@"Error: for GRP_TREE you have to submit either TREE_EASTWARD_DIST<--->TREE_NORTHWARD_DIST, or TREE_DISTANCE_POLAR<--->TREE_ANGLE_POLAR."+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {91,"Error: SPPS_PTYPE is allowed only for CP SPPS_PLOT and for Mires"},
        };

        public static Dictionary<int, string> GrpAgbErrors = new Dictionary<int, string>
        {
            {106, "Error: for GRP_AGB and Cropland Ecosysten the following variables are all mandatory:"+Environment.NewLine+
                  "AGB, AGB_LOCATION, AGB_AREA, AGB_PHEN, AGB_PTYPE"},
            {107, "Error: for GRP_AGB and Grassland Ecosysten the following variables are all mandatory:"+Environment.NewLine+
                "AGB, AGB_AREA, AGB_PHEN, AGB_PLOT_TYPE"},
            {108, "Error: AGB_PTYPE is mandatory for GRP_AGB and Forest Ecosystem"},
            {109, "Error: AGB_LOCATION is mandatory for Forest Ecosystem whe AGB_PTYPE = Moss"},
            {110, "Error: AGB and AGB_NPP_MOSS are mutually exclusive for GRP_AGB for Forest Ecosystem when AGB_PTYPE = Moss"},
            {111, "Error: AGB_PHEN is mandatory for Forest Ecosystem whe AGB_PTYPE = Moss and AGB is provided (not allowed if AGB_NPP_MOSS is submitted)"},
            {112, "Error: for GRP_AGB and Forest Ecosysten and for AGB_PTYPE=Sapling, the following variables are all mandatory:"+Environment.NewLine+
                          "AGB, AGB_LOCATION, AGB_AREA, AGB_SPP, AGB_PTYPE"},
            {113, "Error: for GRP_AGB and Forest Ecosysten and for AGB_PTYPE in (Ferns, Herb, Shrub), the following variables are all mandatory:"+Environment.NewLine+
                          "AGB, AGB_LOCATION, AGB_AREA, AGB_PHEN, AGB_PTYPE"},
            {114, "Error: for GRP_AGB and Mires Ecosysten, AGB_SPP is mandatory"},
            {115, "Error: AGB and AGB_NPP_MOSS are mutually exclusive for GRP_AGB for Mires Ecosystem"},
            {116, "Error: AGB_ORGAN is allowed in case of AGB_PTYPE is on woody species for Mires Ecosystem"},
        };

        public static Dictionary<int, string> GrpWtdPntErrors = new Dictionary<int, string>
        {
            {2,@"Error: for GRP_WTDPNT you have to submit either WTDPNT_EASTWARD_DIST<--->WTDPNT_NORTHWARD_DIST, or WTDPNT_DISTANCE_POLAR <--->WTD_ANGLE_POLAR."+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {137,"Error: entered WTDPNT_PLOT not found in GRP_PLOT"},
            {138,"Error: either WTDPNT_PLOT or WTDPNT_VARMAP must be submitted"},
        };

        public static Dictionary<int, string> GrpD_SnowErrors = new Dictionary<int, string>
        {
            {2,@"Error: for GRP_WTDPNT you have to submit either D_SNOW_EASTWARD_DIST<--->D_SNOW_NORTHWARD_DIST, or D_SNOW_DISTANCE_POLAR <--->D_SNOW_ANGLE_POLAR."+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {140,"Error: either D_SNOW_PLOT or D_SNOW_VARMAP must be submitted"},
        };

        public static Dictionary<int, string> GrpLitterPntErrors = new Dictionary<int, string>
        {
            {85,@"ERROR - Cell $CELL$; LITTERPNT_EASTWARD_DIST<--->LITTERPNT_NORTHWARD_DIST, or LITTERPNT_DISTANCE_POLAR <---> LITTERPNT_ANGLE_POLAR 
                 are possible only for forests if LITTERPNT_TYPE is non-woody, not possible for all the other cases"},
            {128,"Error: entered LITTERPNT_PLOT not found in GRP_PLOT"},
            {129,"Error: LITTERPNT and LITTERPNT_AREA are mandatory for Grassland ecosystem"},
            {130,"Error: for GRP_LITTERPNT and Cropland Ecosysten and for LITTERPNT_TYPE=Natural, the following variables are all mandatory:"+Environment.NewLine+
                 "LITTERPNT, LITTERPNT_ID, LITTERPNT_AREA, LITTERPNT_FRACTION"},
            {131,"Error: LITTERPNT_TYPE is not allowed for Grassland and Cropland Ecosystems"},
            {132,"Error: for GRP_LITTERPNT and Cropland Ecosysten and for LITTERPNT_TYPE=Residual, the following variables are all mandatory:"+Environment.NewLine+
                 "LITTERPNT, LITTERPNT_AREA, LITTERPNT_FRACTION"},
            {133,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Coarse, the following variables are all mandatory:"+Environment.NewLine+
                 "LITTERPNT_COARSE_DIAM, LITTERPNT_COARSE_LENGTH, LITTERPNT_COARSE_ANGLE, LITTERPNT_COARSE_DECAY, LITTERPNT_ID"},
            {134,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Fine-woody, the following variables are all mandatory:"+Environment.NewLine+
                 "LITTERPNT_AREA, LITTERPNT"},
            {135,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Non-woody, the following variables are all mandatory:"+Environment.NewLine+
                 "LITTERPNT_AREA, LITTERPNT, LITTERPNT_ID, LITTERPNT_FRACTION"},
            {136,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Non-woody and LITTERPNT_FRACTION=Leaves, the following variables are all mandatory:"+Environment.NewLine+
                 "LITTERPNT_AREA, LITTERPNT, LITTERPNT_ID, LITTERPNT_FRACTION, LITTERPNT_SPP"},
        };

        public static Dictionary<int, string> GrpAllomErrors = new Dictionary<int, string>
        {
            {1,@"Error: for GRP_ALLOM and Cropland Ecosysten the following variables are all mandatory:"+Environment.NewLine+
                  "ALLOM_DBH, ALLOM_HEIGHT, ALLOM_SPP, ALLOM_STEM_BIOM, ALLOM_BRANCHES_BIOM"},
        };

        public static Dictionary<int, string> GrpInstErrors = new Dictionary<int, string>
        {
            {34,"Entered instrument has not been found. Please enter a Purchase INST_FACTORY value first."},
            {35,"Error: found an INST_FACTORY date prior to Purchase date."},
            {79,"Entered instrument does not have a Purchase entry for INST_FACTORY variable or operation date is prior to instrument purchase date"},
            {123,"Entered instrument is already registered with Purchase factory operation"},
        };

        public static Dictionary<int, string> GrpEcErrors = new Dictionary<int, string>
        {
            {34,"Entered instrument has not been found in GRP_INST. Please insert item with Purchase INST_FACTORY first."},
            {35,"Error: operation date results prior to Purchase date of entered sensor."},
            {223, "Error: Trying to install Instrument $V0$, serial $V1$ which is already installed"},
            {224, @"Error: Trying to install Instrument $V0$, serial $V1$ on date $V2$ which is already installed and was not removed first.
                    Please choose another $OP$ operation or check operation dates"},
            {225, "Error: Trying to perform operation $OP$ on an instrument which does not result to be installed. Please choose another EC_TYPE operation or check operation dates"},
            {226, "Error: Trying to remove Instrument $V0$, serial $V1$ which has been already removed and not reinstalled"},
        };

        /*
        public static Dictionary<int, string> ErrorsList = new Dictionary<int, string>
        {
            {1,"Missing required fields"},
            {2,"Mismatch between PLOT_ID and PLOT_TYPE"},
            {3,"Invalid PLOT_ID format"},
            {4,"Error: PLOT_REFERENCE_POINT is not allowed when PLOT_LOCATION_LAT and PLOT_LOCATION_LONG are submitted"},
            {5,"Error: PLOT_REFERENCE_POINT is allowed only for SP-II sampling points"},
            {6,"Error: PLOT_REFERENCE_POINT is mandatory for SP-II sampling points when PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR are submitted"},
            {7,"Error: please enter either _DATE or _DATE_START / _DATE_END"},
            {8,"Error: please enter either _DATE_END must be greater than _DATE_START"},
            {10,"Error: You have to submit at least one variable"},
            {11,"Error: DM_SURF_MEAS_PRECISION must be submitted together with DM_SURF"},
            {12,"Error: Team member with entered email does not exist or TEAM_MEMBER_WORKEND value is not correct related to TEAM_MEMBER_DATE"},
            {13,"Error: There is already a Manager registered for this site" },
            {14,"Error: TEAM_MEMBER_WORKEND date is greater than TEAM_MEMBER_DATE" },
            {15,"Error: TEAM_MEMBER_DATE must be TEAM_MEMBER_WORKEND + 1 day when trying to change site Manager" },
            {16,"Error: Entered Team member results alrady registered for this site" },
            {34,"Entered instrument has not been found. Please enter a Purchase INST_FACTORY value first."},
            {35,"Error: found an INST_FACTORY date prior to Purchase date."},
            {50,"Error: entered $V0$ is missing in GRP_PLOT group"},
            {58,@"Error: for GRP_PLOT you have to submit either PLOT_EASTWARD_DIST <---> PLOT_NORTHWARD_DIST, or PLOT_DISTANCE_POLAR <---> PLOT_ANGLE_POLAR or PLOT_LOCATION_LAT <---> PLOT_LOCATION_LONG."+
                  Environment.NewLine + "These couples of information are mutually exclusive"},
            {63,"Error: GAI_SPP is mandatory for Mires Ecosystem and Visual estimation and Modified VGA GAI_METHODs"},
            {64,"Error: Visual estimation and Modified VGA GAI_METHODs can be used only for Mires Ecosystem"},
            {65,"Error: GAI_PLOT_TYPE is mandatory when GAI_METHOD is Accupar for Grassland Ecosystem"},
            {66,"Error: DHP GAI_METHOD is allowed only for Forest Ecosystem"},
            {67,"Error: GAI_SPP can be reported only when GAI_PTYPE is Crop for Cropland Ecosystem when GAI_METHOD is Destructive"},
            {68,"Error: GAI_PTYPE for Cropland Ecosystem must be Weed or Crop when GAI_METHOD is Destructive"},
            {69,"Error: GAI_LOCATION is required for Cropland Ecosystem when GAI_METHOD is Destructive"},
            {70,"Error: GAI_PTYPE is required for Cropland Ecosystem when GAI_METHOD is Destructive"},
            {71,"Error: GAI_PTYPE Crop and Weed are not allowed for Grassland Ecosystem when GAI_METHOD is Destructive"},
            {72,"Error: GAI_PLOT_TYPE is mandatory for Grassland Ecosystem when GAI_METHOD is Destructive"},
            {73,"Error: GAI_AREA is mandatory for Grassland or Cropland Ecosystem when GAI_METHOD is Destructive"},
            {74,"Error: GAI_DHP_ID, GAI_DHP_SLOPE, GAI_DHP_ASPECT are mandatory for Forest Ecosystem when GAI_METHOD is DHP"},
            {75,"Error: GAI_DHP_EASTWARD_DIST<--->GAI_DHP_NORTHWARD_DIST and GAI_DHP_DISTANCE_POLAR<---->GAI_DHP_ANGLE_POLAR are mutually exclusive and are mandatory for forest ecosystems when GAI_METHOD is DHP and GAI_PLOT is CP"},
            {76,"Error: GAI_DHP_EASTWARD_DIST<--->GAI_DHP_NORTHWARD_DIST and GAI_DHP_DISTANCE_POLAR<---->GAI_DHP_ANGLE_POLAR are mutually exclusive and are possible for forest ecosystems when GAI_METHOD is DHP and GAI_PLOT is CP"},
            {77,"Error: GAI is mandatory for Visual estimation, Modified VGA or Spectral reflectance GAI_METHODs"},
            {78,"Error: GAI is mandatory for Grassland or Cropland ecosystem and GAI_METHOD is Destructive"},
            {79,"Entered instrument does not have a Purchase entry for INST_FACTORY variable or operation date is prior to instrument purchase date"},
            {80,"Error: GAI_METHOD Accupar is allowed only for Grassland or Cropland ecosystems"},
            {81,"Error: GAI_CEPT_EASTWARD_DIST<--->GAI_CEPT_NORTHWARD_DIST and GAI_CEPT_DISTANCE_POLAR<---->GAI_CEPT_ANGLE_POLAR are mutually exclusive and are mandatory for forest ecosystems when GAI_METHOD is SUNSCAN and GAI_PLOT is CP"},
            {82,"Error: GAI_CEPT_ID is mandatory for forest ecosystems when GAI_METHOD is SUNSCAN and GAI_PLOT is SP"},
            {83,"Error: GAI_PLOT_TYPE is mandatory for Grassland ecosystems when GAI_METHOD is SUNSCAN"},
            {84,"Error: SUNSCAN GAI_METHOD is allowed only for Forest, Grassland and Cropland ecosystems"},
            {85,"Error: CEPT_FIRST and CEPT_LAST must be submitted together for Grassland ecosystem"},
            {86,"Error: entered TREE_PLOT not found in GRP_PLOT"},
            {87,"Error: TREE_ID is mandatory for CP or Outside_CP tree plots; it must be unique in a given CP"},
            {88,"Error: TREE_DBH, TREE_SPP and TREE_STATUS are all mandatory Forest ecosystem"},
            {89,"Error: entered SPPS_PLOT not found in GRP_PLOT"},
            {90,"Error: Either (SPP_LOCATION, SPP_LOCATION_LAT, SPP_LOCATION_LON) or (SPP_LOCATION, SPP_LOCATION_DIST, SPP_LOCATION_ANG) must  be submitted together for Inaccessible Mires"},
            {91,"Error: SPPS_PTYPE is allowed only for CP SPPS_PLOT and for Mires"},
            {92,"Error: entered SOSM_PLOT_ID not found in GRP_PLOT"},
            {93,"Error: SOSM_SAMPLE_MAT organic options (O, Oi, Oa, Oe) are not allowed when SOSM_PLOT_ID is of type SP-I"},
            {94,"Error: SOSM_UD and SOSM_SD must be submitted together when SOSM_SAMPLE_MAT is M"},
            {95,"Error: SOSM_AREA and SOSM_VOLUME must be submitted together when SOSM_SAMPLE_MAT is M and SOSM_PLOT_ID is of type SP-II"},
            {96,"Error: SOSM_AREA, SOSM_VOLUME and SOSM_THICKNESS must be submitted together when SOSM_SAMPLE_MAT is in (O, Oi, Oa, Oe) and SOSM_PLOT_ID is of type SP-II"},
            {97,"Error: CP_XX not allowed for SOSM_PLOT_ID"},
            {98,"Error: Wrong format for SOSM_SAMPLE_ID: must be in the format CC-Xxx_YYYYMMDD_NN_AA_BB when SOSM_PLOT_ID is of type SP-I and SOSM_SAMPLE_MAT is M"},
            {99,"Error: Wrong format for SOSM_SAMPLE_ID: must be in the format CC-Xxx_YYYYMMDD_NN-MM_AA_BB when SOSM_PLOT_ID is of type SP-II and SOSM_SAMPLE_MAT is M"},
            {100,"Error: Wrong format for SOSM_SAMPLE_ID: must be in the format CC-Xxx_YYYYMMDD_NN-MM_(O|Oa|Oe|Oi)_BB when SOSM_PLOT_ID is of type SP-II and SOSM_SAMPLE_MAT is in (O, Oi, Oa, Oe)"},
            {101,"Error: FLSM_SAMPLE_TYPE variable required for GRP_FLSM"},
            {102,"Error: FLSM_SAMPLE_ID variable required for GRP_FLSM"},
            {103,"Error: FLSM_SPP and FLSM_PTYPE are mutually exclusive for GRP_FLSM"},
            {104,"Error: entered FLSM_PLOT_ID not found in GRP_PLOT"},
            {105,"Error: entered AGB_PLOT not found in GRP_PLOT"},
            {106, "Error: for GRP_AGB and Cropland Ecosysten the following variables are all mandatory:"+Environment.NewLine+
                          "AGB, AGB_LOCATION, AGB_AREA, AGB_PHEN, AGB_PTYPE"},
            {107, "Error: for GRP_AGB and Grassland Ecosysten the following variables are all mandatory:"+Environment.NewLine+
                          "AGB, AGB_AREA, AGB_PHEN, AGB_PLOT_TYPE"},
            {108, "Error: AGB_PTYPE is mandatory for GRP_AGB and Forest Ecosystem"},
            {109, "Error: AGB_LOCATION is mandatory for Forest Ecosystem whe AGB_PTYPE = Moss"},
            {110, "Error: AGB and AGB_NPP_MOSS are mutually exclusive for GRP_AGB for Forest Ecosystem when AGB_PTYPE = Moss"},
            {111, "Error: AGB_PHEN is mandatory for Forest Ecosystem whe AGB_PTYPE = Moss and AGB is provided (not allowed if AGB_NPP_MOSS is submitted)"},
            {112, "Error: for GRP_AGB and Forest Ecosysten and for AGB_PTYPE=Sapling, the following variables are all mandatory:"+Environment.NewLine+
                          "AGB, AGB_LOCATION, AGB_AREA, AGB_SPP, AGB_PTYPE"},
            {113, "Error: for GRP_AGB and Forest Ecosysten and for AGB_PTYPE in (Ferns, Herb, Shrub), the following variables are all mandatory:"+Environment.NewLine+
                          "AGB, AGB_LOCATION, AGB_AREA, AGB_PHEN, AGB_PTYPE"},
            {114, "Error: for GRP_AGB and Mires Ecosysten, AGB_SPP is mandatory"},
            {115, "Error: AGB and AGB_NPP_MOSS are mutually exclusive for GRP_AGB for Mires Ecosystem"},
            {116, "Error: AGB_ORGAN is allowed in case of AGB_PTYPE is on woody species for Mires Ecosystem"},
            {118,"Serial number wrong format. Correct format: $V0$, where X is a digit. Found value: $V1$"},
            {123,"Entered instrument is already registered with Purchase factory operation"},
            {126,"Error: entered logger id does not match with Logger ID registered for this site in GRP_LOGGER group"},
            {127,"Error: The logger id=$V0$ and file id=$V1$ is already registered for $V2$ type in GRP_FILE"},
            {128,"Error: entered LITTERPNT_PLOT not found in GRP_PLOT"},
            {129,"Error: LITTERPNT and LITTERPNT_AREA are mandatory for Grassland ecosystem"},
            {130,"Error: for GRP_LITTERPNT and Cropland Ecosysten and for LITTERPNT_TYPE=Natural, the following variables are all mandatory:"+Environment.NewLine+
                          "LITTERPNT, LITTERPNT_ID, LITTERPNT_AREA, LITTERPNT_FRACTION"},
            {131,"Error: LITTERPNT_TYPE is not allowed for Grassland and Cropland Ecosystems"},
            {132,"Error: for GRP_LITTERPNT and Cropland Ecosysten and for LITTERPNT_TYPE=Residual, the following variables are all mandatory:"+Environment.NewLine+
                          "LITTERPNT, LITTERPNT_AREA, LITTERPNT_FRACTION"},
            {133,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Coarse, the following variables are all mandatory:"+Environment.NewLine+
                          "LITTERPNT_COARSE_DIAM, LITTERPNT_COARSE_LENGTH, LITTERPNT_COARSE_ANGLE, LITTERPNT_COARSE_DECAY, LITTERPNT_ID"},
            {134,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Fine-woody, the following variables are all mandatory:"+Environment.NewLine+
                          "LITTERPNT_AREA, LITTERPNT"},
            {135,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Non-woody, the following variables are all mandatory:"+Environment.NewLine+
                          "LITTERPNT_AREA, LITTERPNT, LITTERPNT_ID, LITTERPNT_FRACTION"},
            {136,"Error: for GRP_LITTERPNT and Forest Ecosysten and for LITTERPNT_TYPE=Non-woody and LITTERPNT_FRACTION=Leaves, the following variables are all mandatory:"+Environment.NewLine+
                          "LITTERPNT_AREA, LITTERPNT, LITTERPNT_ID, LITTERPNT_FRACTION, LITTERPNT_SPP"},
            {137,"Error: entered WTDPNT_PLOT not found in GRP_PLOT"},
            {138,"Error: either WTDPNT_PLOT or WTDPNT_VARMAP must be submitted"},
            {139,"Error: entered D_SNOW_PLOT not found in GRP_PLOT"},
            {140,"Error: either D_SNOW_PLOT or D_SNOW_VARMAP must be submitted"},
            {200,"Missing $V0$, $V1$ and $V2$ variables. Please, enter either $V0$ or $V1$/$V2$"},
            {201,"Found $V0$ together with $V1$ and/or $V2$ variables. Please, enter either $V0$ or $V1$/$V2$"},
            {219, "Error: You have to specify ony date for $V0$ operation"},
            {220, "Error: for Installation operation EC_HEIGHT,EC_EASTWARD_DIST and EC_NORTHWARD_DIST are mandatory"},
            {221, "Error: for Installation operation EC_SA_HEAT and EC_SA_OFFSET_N are mandatory for SA-Gill senosrs"},
            {222, "Error: You have to specify all the variables EC_SAMPLING_INT, EC_LOGGER, EC_FILE for instrument $V0$, serial $V1$ if any of these is entered"},
            {223, "Error: Trying to install Instrument $V0$, serial $V1$ which is already installed"},
            {224, "Error: Trying to install Instrument $V0$, serial $V1$ on date $V2$ which is already installed and was not removed first. Please choose another $OP$ operation or check operation dates"},
            {225, "Error: Trying to perform operation $OP$ on an instrument which does not result to be installed. Please choose another EC_TYPE operation or check operation dates"},
            {226, "Error: Trying to remove Instrument $V0$, serial $V1$ which has been already removed and not reinstalled"},
            {227, "Error: EC_GA_CAL_CO2_OFFSET and EC_GA_CAL_CO2_REF must be submitted together for Field Calibration / Field Calibration Check operations"},
            {234, "Error: .csv extension not allowed for binary files"},
            {235, "Error: .bin extension not allowed for ASCII files"},
            {236, "Error: FILE_HEAD_VARS for ASCII files must be specified if FILE_HEAD_NUM is entered"},
            {237, "Error: FILE_HEAD_NUM for ASCII files must be specified if FILE_HEAD_VARS is entered"},
            {238, "Error: FILE_HEAD_VARS cannot be greater than FILE_HEAD_NUM"},
            {239, "Error: FILE_HEAD_TYPE must be different from FILE_HEAD_VARS"},
            {240, "Error: FILE_HEAD_VARS and FILE_HEAD_TYPE for binary files must be specified if FILE_HEAD_NUM is entered"},
            {241, "Error: FILE_HEAD_NUM and FILE_HEAD_TYPE for binary files must be specified if FILE_HEAD_VARS is entered"},
            {242, "Error: FILE_HEAD_NUM and FILE_HEAD_VARS for binary files must be specified if FILE_HEAD_TYPE is entered"},
            {243, "Error: FILE_HEAD_TYPE cannot be greater than FILE_HEAD_NUM"},
            {244, "Error: FILE_HEAD_TYPE cannot be greater than FILE_HEAD_VARS"},
            {245, "Error: FILE_FORMAT and FILE_EXT must be submitted together"},
            {246, "Error: You have to submit FILE_FORMAT if FILE_HEAD_NUM, FILE_HEAD_VARS or FILE_HEAD_TYPE are entered"},
            {250, "Error: Instrument is missing or results to be removed in EC group"},
            {251, "Error: EC_SYS date must be greater than installation dates of SA* and GA* sensors"},
            {252, "Error: Invalid format for BM_VARIABLE_H_V_R. Please enter <VAR_NAME>_X_Y_Z"},
            {253, "Error: Entered variable not found in BM_VARIABLE_H_V_R list"},
            {254, "Error: Entered BM_MODEL, for Installation operation, does not result to be removed first"},
            {255, "Error: Entered BM_MODEL, for $OP$ operation, does not result to be installed first"},
            {256, "Error: BM_HEIGHT, BM_EASTWARD_DIST and BM_NORTHWARD_DIST are mandatory for Installation operation"},
            {257, "Error: BM_VARIABLE_H_V_R, BM_SAMPLING_INT, BM_LOGGER and BM_FILE are all mandatory for Installation operation if one them is entered"},
            {258, "Error: BM_VARIABLE_H_V_R, BM_SAMPLING_INT, BM_LOGGER and BM_FILE are all mandatory for Variable map operation"},
            {259, "Error: BM_LOGGER and BM_FILE not found in GRP_FILE for BM file types"},
            {260, "Error: The entered BM_FILE and BM_LOGGER values are already mapped to at least one variable with a sampling interval different from the one that you are trying to insert. Please note that the sampling interval must be the same for all the variables in a file"},
            {261, "Error: entered BM_VARIABLE_H_V_R results already mapped on a different instrument"},
            {262, "Error: there is a Removal or Installation operation with BM_DATE following the submitted date"},
            {300, "Error: for GA Installation the following variables are all mandatory:"+Environment.NewLine+
                          "STO_GA_MODEL,STO_GA_SN,STO_GA_VARIABLE,STO_LOGGER,STO_FILE,"+Environment.NewLine+"STO_GA_PROF_ID,STO_GA_SAMPLING_INT,STO_DATE,STO_CONFIG,STO_TYPE"},
            {301, "Error: for GA Installation, closed path sensor the following variables are all mandatory:"+Environment.NewLine+
                          "STO_GA_TUBE_LENGTH,STO_GA_TUBE_DIAM,STO_GA_TUBE_MAT,STO_GA_TUBE_THERM"},
            {302, "Error: for GA Installation, open path sensor the following variables are not allowed:"+Environment.NewLine+
                          "STO_GA_TUBE_LENGTH,STO_GA_TUBE_DIAM,STO_GA_TUBE_MAT,STO_GA_TUBE_THERM"},
            {303, "Error: STO_GA_AZIM_MOUNT and STO_GA_VERT_MOUNT mandatory only if STO_CONFIG=separate and the IRGA is an open path"},
            {304, "Error: STO_GA_FLOW_RATE possible only for closed path IRGAs"},
            {305, "Error: STO_GA_AZIM_MOUNT and STO_GA_VERT_MOUNT are all mandatory if STO_CONFIG=separate and the IRGA is an open path"},
            {306, "Error: STO_GA_AZIM_MOUNT and STO_GA_VERT_MOUNT are not allowed if STO_CONFIG is different from separate"},
            {307, "Error: Entered STO_GA_PROF_ID was not found or has been removed. Please insert related STO_PROF_ID variable"},
            {308, "Error: The same STO_GA_PROF_ID can be used for different GA in case of STO_CONFIG == 'sequential', but it must be unique in case of STO_CONFIG == 'separate'"},
            {309, "Error: for GA Removal the following variables are all mandatory:"+Environment.NewLine+
                          "STO_GA_MODEL,STO_GA_SN,STO_DATE"},
            {310, "Error: entered GA model was not found or has not been GA installed before"},
            {311, "Error: For Field calibration, Field calibration check and Firmware update operations STO_GA_CAL_VARIABLE,STO_GA_CAL_VALUE,STO_GA_CAL_REF must be all submitted together"},
            {312, "Error: for Level Installation the following variables are all mandatory:"+Environment.NewLine+
                          "STO_CONFIG, STO_PROF_ID, STO_PROF_LEVEL, STO_PROF_HEIGHT, STO_PROF_EASTWARD_DIST,STO_PROF_NORTHWARD_DIST"+Environment.NewLine+
                            "STO_PROF_BUFFER_VOL,STO_PROF_TUBE_LENGTH,STO_PROF_TUBE_DIAM, STO_PROF_TUBE_THERM, STO_PROF_TUBE_MAT "},
            {313, "Error: For Level removal STO_DATE and STO_PROF_ID variables are mandatory"},
            {314, "Error: entered STO_PROF_ID was not found or level has already been removed"},
            {315, "Error: entered STO_PROF_ID has already been submitted"},
            {316, "Error: for Disturbance, Field Cleaning, Parts Substitution, Filter change, Tube change operations STO_PROF_ID is mandatory when one or more other STO_PROF_* variables are submitted"},
            {317, "Error: STO_PROF_SAMPLING_TIME is possible only when STO_CONFIG is sequential"},
            {318, "Error: for Variable map the following variables are all mandatory:"+Environment.NewLine+
                          "STO_GA_VARIABLE, STO_LOGGER, STO_FILE, STO_GA_MODEL, STO_GA_SN, STO_GA_SAMPLING_INT, STO_DATE"},
            {319, "Error: STO_GA_MODEL and STO_GA_SN must be submitted together"},
            {320, "Error: STO_GA_MODEL and STO_GA_SN must be submitted together"},
            {321, "Error: STO_DATE variable not allowed for Connection failure operation"},
            {322, "Error: STO_DATE_START and STO_DATE_END must be submitted together for Connection failure operation"},
            {323, "Error: trying to install a GA sensor that has not been removed before"},
            {324, "Error: trying to remove a GA sensor that has not been installed before"},
            {325, "Error: trying to install a Level that has not been removed before"},
            {326, "Error: trying to remove a Level that has not been installed before"},
            {327, "Error: there is a GA Removal or GA Installation operation with STO_DATE following the submitted date"},
            {328, "Error: there is a Level Removal or Level Installation operation with STO_DATE following the submitted date"},
            {329, "Error: for Maintenance operation either STO_PROF_* variables or STO_GA_* must be submitted"},
            {330, "Error: for Maintenance operation STO_PROF_SAMPLING_TIME possible only if STO_CONFIG=sequential"},
            {331, "Error: for Maintenance operation STO_PROF_LEVEL mandatory if STO_PROF_BUFFER_FLOWRATE is reported (must be 0 for STO_CONFIG=simultaneous) and if STO_PROF_TUBE_THREAT is reported"},
            {332, "Error: for Maintenance operation STO_PROF_BUFFER_FLOWRATE must be 0 for STO_CONFIG=simultaneous"},
            {333, "Error: for Maintenance operation STO_GA_FLOW_RATE and STO_GA_TUBE_THERM possible only for closed path IRGAs"},
            {334, "Error: STO_LOGGER and STO_FILE not found in GRP_FILE for STO file types"},
            {335, "Error: found a different STO_GA_SAMPLING_INT with this STO_FILE value"},
            {336, "Error: trying to perform operation on a sensor that has not been installed before"},
            {337, "Error: trying to perform an installation on a sensor that has not been removed before"},
            {338, "Error: there is a Removal or Installation operation with INSTMAN_DATE following the submitted date"},
            {339, "Error: entered INSTMAN_VARIABLE_H_V_R results already mapped on a different instrument"},
            {340, "Error: INSTMAN_HEIGHT, INSTMAN_EASTWARD_DIST and INSTMAN_NORTHWARD_DIST are all mandatory for Installation operation"},
            {341, "Error: INSTMAN_VARIABLE_H_V_R and INSTMAN_SAMPLING_INT must be submitted together for Installation operation"},
            {342, "Error: INSTMAN_VARIABLE_H_V_R and INSTMAN_SAMPLING_INT are mandatory for Variable map operation"},
            {343, "Error: INTSTPAIR sensor not found in GRP_INSTMAN"},
            {344, "Error: INSTPAIR_MODEL_1 and INSTPAIR_MODEL_2 must be different sensor types"},
            {345, "Error: INSTPAIR_HEIGHT_SEP, INSTPAIR_EASTWARD_SEP and INSTPAIR_NORTHWARD_SEP are all mandatory for GRP_INSTPAIR"}

        };*/
    }
}
