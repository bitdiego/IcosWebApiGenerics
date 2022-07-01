using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.StationDescValidation
{
    public class StationDescValidation
    {
        private static string Err = "";
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

       
        //OK
        public static /*Response*/ void ValidateLocationResponse(GRP_LOCATION location, Response response)
        {
            //Not mandatory
            errorCode = GeneralValidation.MissingMandatoryData<string>(location.LOCATION_DATE, "LOCATION_DATE", "GRP_LOCATION");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOCATION_DATE", "$V0$", "LOCATION_DATE", "$GRP$", "GRP_LOCATION");
            }
            errorCode = DatesValidator.IsoDateCheck(location.LOCATION_DATE, "LOCATION_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOCATION_DATE", "$V0$", "LOCATION_DATE", "$V1$", location.LOCATION_DATE);
            }

            if (location.LOCATION_LAT > Globals.MAX_LATITUDE_VALUE || location.LOCATION_LAT < -Globals.MAX_LATITUDE_VALUE)
            {
                errorCode = 6;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpLocationErrors[errorCode], "LOCATION_LAT", "$V0$", location.LOCATION_LAT.ToString());
            }
            if(!GeneralValidation.DecimalValueInRange(- Globals.MAX_LONGITUDE_VALUE, Globals.MAX_LONGITUDE_VALUE, location.LOCATION_LONG))
            {
                errorCode = 5;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpLocationErrors[errorCode], "LOCATION_LONG", "$V0$", location.LOCATION_LONG.ToString());
            }
            if (!GeneralValidation.DecimalValueInRange(-Globals.MAX_LATITUDE_VALUE, Globals.MAX_LATITUDE_VALUE, location.LOCATION_LAT))
            {
                errorCode = 5;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpLocationErrors[errorCode], "LOCATION_LAT", "$V0$", location.LOCATION_LAT.ToString());
            }
        }

        //OK
        public static void ValidateUtcResponse(GRP_UTC_OFFSET utc, Response response)
        {
            errorCode = DatesValidator.IsoDateCheck(utc.UTC_OFFSET_DATE_START, "UTC_OFFSET_DATE_START");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "UTC_OFFSET_DATE_START", "$V0$", "UTC_OFFSET_DATE_START", "$V1$", utc.UTC_OFFSET_DATE_START);
            }
            if (Decimal.Compare(utc.UTC_OFFSET, Globals.MIN_UTC_OFFSET_VAL) < 0 || Decimal.Compare(utc.UTC_OFFSET, Globals.MAX_UTC_OFFSET_VAL) > 0)
            {
                errorCode = 1;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpUtcErrors[errorCode], "UTC_OFFSET", "$V0$", utc.UTC_OFFSET.ToString());
            }
            //return response;
        }

        //OK
        public static async Task ValidateLandOwnerResponseAsync(GRP_LAND_OWNERSHIP land, IcosDbContext db, Response response)
        {
            //Not mandatory
            if (!String.IsNullOrEmpty(land.LAND_DATE))
            {
                errorCode = DatesValidator.IsoDateCheck(land.LAND_DATE, "LAND_DATE");
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_DATE", "$V0$", "LAND_DATE", "$V1$", land.LAND_DATE);
                }
            }

            //return response;
        }


        public static async Task ValidateTowerResponseAsync(GRP_TOWER tower, IcosDbContext db, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<string>(tower.TOWER_DATE, "TOWER_DATE", "GRP_TOWER");
            if (errorCode > 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "TOWER_DATE", "$V0$", "TOWER_DATE", "$GRP$", "GRP_TOWER");
            }
            DatesValidator.IsoDateCheck(tower.TOWER_DATE, "TOWER_DATE");
            //return response;
        }


        //to do
        public static void ValidateClimateAvgResponse(GRP_CLIM_AVG climateAvg, Response response)
        {
            if(GeneralValidation.IsAnyPropNotNull<GRP_CLIM_AVG>(climateAvg, "MAP", "MAT", "MAS", "MAR", "MAC_YEARS"))
            {
                errorCode= GeneralValidation.MissingMandatoryData<string>(climateAvg.MAC_DATE, "MAC_DATE", "GRP_CLIM_AVG");
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "MAC_DATE", "$V0$", "MAC_DATE", "$GRP$", "GRP_CLIM_AVG");
                }

                errorCode= DatesValidator.IsoDateCheck(climateAvg.MAC_DATE, "MAC_DATE");
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "MAC_DATE", "$V0$", "MAC_DATE", "$V1$", climateAvg.MAC_DATE);
                }

            }
            //check if MAP, MAR, MAS, MAC_YEARS must only be positive
            //return response;
        }

        //to do
        public static Response ValidateDistManResponse(GRP_DM distMan, IcosDbContext db, Response response)
        {
            if (!String.IsNullOrEmpty(distMan.DM_DATE) && !String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                errorCode = 4;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DM_DATE", "$V0$", "DM_DATE", "$V1$", "DM_DATE_START", "$V2$", "DM_DATE_END");
            }
            if (!String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                if (String.Compare(distMan.DM_DATE_START, distMan.DM_DATE_END) > 0)
                {
                    errorCode = 6;
                    response.Code += errorCode;
                    Err = ErrorCodes.GeneralErrors[errorCode];
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DM_DATE_START", "$V0$", distMan.DM_DATE_END, "$V1$", distMan.DM_DATE_START);
                 }
            }
            
            if (!GeneralValidation.IsAnyPropNotNull<GRP_DM>(distMan, "DM_AGRICULTURE","DM_ENCROACH","DM_EXT_WEATHER","DM_FERT_M","DM_FERT_O","DM_FIRE",
                                                                        "DM_FORESTRY","DM_GRAZE","DM_INS_PATH", "DM_PESTICIDE",
                                                                        "DM_PLANTING","DM_TILL","DM_WATER","DM_GENERAL","DM_SURF","DM_SURF_MEAS_PRECISION"))
            {
                errorCode = 9;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "DM_AGRICULTURE");
            }

            return response;
        }
    }
}
