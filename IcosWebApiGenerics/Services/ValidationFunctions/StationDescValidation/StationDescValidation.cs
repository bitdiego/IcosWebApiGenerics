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
            if (location.LOCATION_LONG > Globals.MAX_LONGITUDE_VALUE || location.LOCATION_LONG < -Globals.MAX_LONGITUDE_VALUE)
            {
                errorCode = 5;
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GrpLocationErrors[errorCode], "LOCATION_LONG", "$V0$", location.LOCATION_LONG.ToString());
            }
            //return response;

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
            if (!String.IsNullOrEmpty(land.LAND_DATE))
            {
                errorCode = DatesValidator.IsoDateCheck(land.LAND_DATE, "LAND_DATE");
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_DATE", "$V0$", "LAND_DATE", "$V1$", land.LAND_DATE);
                }
            }

            if (!String.IsNullOrEmpty(land.LAND_OWNERSHIP))
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(land.LAND_OWNERSHIP, (int)Globals.CvIndexes.LAND_OWNERSHIP, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LAND_OWNERSHIP", "$V0$", land.LAND_OWNERSHIP, "$V1$", "LAND_OWNERSHIP", "$GRP$", "GRP_LAND_OWNERSHIP");
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
            await GeneralValidation.ItemInBadmListAsync(tower.TOWER_TYPE, (int)Globals.CvIndexes.TOWER_TYPE, db);
            await GeneralValidation.ItemInBadmListAsync(tower.TOWER_ACCESS, (int)Globals.CvIndexes.TOWER_ACCESS, db);
            await GeneralValidation.ItemInBadmListAsync(tower.TOWER_POWER, (int)Globals.CvIndexes.TOWER_POWER, db);
            await GeneralValidation.ItemInBadmListAsync(tower.TOWER_DATATRAN, (int)Globals.CvIndexes.TOWER_DATATRAN, db);
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
            DatesValidator.IsoDateCheck(distMan.DM_DATE, "DM_DATE");
            DatesValidator.IsoDateCheck(distMan.DM_DATE_START, "DM_DATE_START");
            DatesValidator.IsoDateCheck(distMan.DM_DATE_END, "DM_DATE_END");
            if (!String.IsNullOrEmpty(distMan.DM_DATE) && !String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                errorCode = 4;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                response.FormatError("DM_DATE", "$V0$", distMan.DM_DATE, "$V1$", distMan.DM_DATE_START, "$V2$", distMan.DM_DATE_END);
                //Globals.FormatError(ref Err, "$V0$", distMan.DM_DATE, "$V1$", distMan.DM_DATE_START, "$V2$", distMan.DM_DATE_END);
            }
            if (!String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                if (String.Compare(distMan.DM_DATE_START, distMan.DM_DATE_END) > 0)
                {
                    errorCode = 6;
                    response.Code += errorCode;
                    Err = ErrorCodes.GeneralErrors[errorCode];
                    response.FormatError("DM_DATE_START", "$V0$", distMan.DM_DATE_END, "$V1$", distMan.DM_DATE_START);
                    //Globals.FormatError(ref Err, "$V0$", distMan.DM_DATE_END, "$V1$", distMan.DM_DATE_START);
                }
            }
            /*
            GeneralValidation.ItemInBadmList("DM_ENCROACH", distMan.DM_ENCROACH, "GRP_DM", (int)Globals.CvIndexes.DM_ENCROACH, db);
            GeneralValidation.ItemInBadmList("DM_AGRICULTURE", distMan.DM_AGRICULTURE, "GRP_DM", (int)Globals.CvIndexes.DM_AGRICULTURE, db);
            GeneralValidation.ItemInBadmList("DM_EXT_WEATHER", distMan.DM_EXT_WEATHER, "GRP_DM", (int)Globals.CvIndexes.DM_EXT_WEATHER, db);
            GeneralValidation.ItemInBadmList("DM_FERT_M", distMan.DM_FERT_M, "GRP_DM", (int)Globals.CvIndexes.DM_FERT_M, db);
            GeneralValidation.ItemInBadmList("DM_FERT_O", distMan.DM_FERT_M, "DM_FERT_O", (int)Globals.CvIndexes.DM_FERT_O, db); 
            GeneralValidation.ItemInBadmList("DM_FIRE", distMan.DM_FERT_M, "DM_FIRE", (int)Globals.CvIndexes.DM_FIRE, db);
            GeneralValidation.ItemInBadmList("DM_FORESTRY", distMan.DM_FERT_M, "DM_FORESTRY", (int)Globals.CvIndexes.DM_FORESTRY, db);
            GeneralValidation.ItemInBadmList("DM_GRAZE", distMan.DM_FERT_M, "DM_GRAZE", (int)Globals.CvIndexes.DM_GRAZE, db);
            GeneralValidation.ItemInBadmList("DM_INS_PATH", distMan.DM_FERT_M, "DM_INS_PATH", (int)Globals.CvIndexes.DM_INS_PATH, db);
            GeneralValidation.ItemInBadmList("DM_PESTICIDE", distMan.DM_FERT_M, "DM_PESTICIDE", (int)Globals.CvIndexes.DM_PESTICIDE, db);
            GeneralValidation.ItemInBadmList("DM_PLANTING", distMan.DM_FERT_M, "DM_PLANTING", (int)Globals.CvIndexes.DM_PLANTING, db);
            GeneralValidation.ItemInBadmList("DM_TILL", distMan.DM_FERT_M, "DM_TILL", (int)Globals.CvIndexes.DM_TILL, db); 
            GeneralValidation.ItemInBadmList("DM_WATER", distMan.DM_FERT_M, "DM_WATER", (int)Globals.CvIndexes.DM_WATER, db);
            GeneralValidation.ItemInBadmList("DM_GENERAL", distMan.DM_FERT_M, "DM_GENERAL", (int)Globals.CvIndexes.DM_GENERAL, db);
            */
            if (!GeneralValidation.IsAnyPropNotNull<GRP_DM>(distMan))
            {
                errorCode = 9;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
            }

            return response;
        }
    }
}
