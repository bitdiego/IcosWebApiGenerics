using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public class GrpValidator
    {
        private static Response response = null;
        private static string Err = "";
        private static int errorCode = 0;

        public static Response GetResponse()
        {
            if (response == null)
            {
                response = new Response();
            }
            return response;
        }
        public static Response ValidateLocationResponse(GRP_LOCATION location)
        {
            MissingDate(location.LOCATION_DATE, "LOCATION_DATE", "GRP_LOCATION");
            IsoDateCheck(location.LOCATION_DATE, "LOCATION_DATE");
            
            if (location.LOCATION_LAT > Globals.MAX_LATITUDE_VALUE || location.LOCATION_LAT < -Globals.MAX_LATITUDE_VALUE)
            {
                errorCode = 5;
                response.Code += errorCode;
                Err = ErrorCodes.GrpLocationErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$",  location.LOCATION_LAT.ToString());
                response.Messages.Add("LOCATION_LAT", ErrorCodes.GrpLocationErrors[errorCode]);
            }
            if (location.LOCATION_LONG > Globals.MAX_LATITUDE_VALUE|| location.LOCATION_LONG < -Globals.MAX_LATITUDE_VALUE)
            {
                errorCode = 6;
                response.Code += errorCode;
                Err = ErrorCodes.GrpLocationErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$", location.LOCATION_LAT.ToString());
                response.Messages.Add("LOCATION_LONG", Err);
            }
            return response;
        }

        public static Response ValidateUtcResponse(GRP_UTC_OFFSET utc)
        {
            if (!String.IsNullOrEmpty(utc.UTC_OFFSET_DATE_START))
            {
                IsoDateCheck(utc.UTC_OFFSET_DATE_START, "UTC_OFFSET_DATE_START");
            }
            if (Decimal.Compare(utc.UTC_OFFSET, Globals.MIN_UTC_OFFSET_VAL) < 0 || Decimal.Compare(utc.UTC_OFFSET, Globals.MAX_UTC_OFFSET_VAL) > 0)
            {
                errorCode = 1;
                response.Code += errorCode;
                Err = ErrorCodes.GrpUtcErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$", utc.UTC_OFFSET.ToString());
                response.Messages.Add("UTC_OFFSET", Err);
            }
            return response;
        }

        public static Response ValidateLandOwnerResponse(GRP_LAND_OWNERSHIP land, IcosDbContext db)
        {
            if (!String.IsNullOrEmpty(land.LAND_DATE))
            {
                IsoDateCheck(land.LAND_DATE, "LAND_DATE");
            }
            ItemInBadmList("LAND_OWNERSHIP", land.LAND_OWNERSHIP, "GRP_LAND_OWNERSHIP", (int)Globals.CvIndexes.LAND_OWNERSHIP, db);
            return response;
        }

        public static Response ValidateTowerResponse(GRP_TOWER tower, IcosDbContext db)
        {
            MissingDate(tower.TOWER_DATE, "TOWER_DATE", "GRP_TOWER");
            IsoDateCheck(tower.TOWER_DATE, "TOWER_DATE");
            ItemInBadmList("TOWER_TYPE", tower.TOWER_TYPE, "GRP_TOWER", (int)Globals.CvIndexes.TOWER_TYPE, db);
            ItemInBadmList("TOWER_ACCES", tower.TOWER_ACCESS, "GRP_TOWER", (int)Globals.CvIndexes.TOWER_ACCESS, db);
            ItemInBadmList("TOWER_POWER", tower.TOWER_POWER, "GRP_TOWER", (int)Globals.CvIndexes.TOWER_POWER, db);
            ItemInBadmList("TOWER_DATATRAN", tower.TOWER_DATATRAN, "GRP_TOWER", (int)Globals.CvIndexes.TOWER_DATATRAN, db);
            return response;
        }

        public static Response ValidateClimateAvgResponse(GRP_CLIM_AVG climateAvg)
        {
            MissingDate(climateAvg.MAC_DATE, "MAC_DATE", "GRP_CLIM_AVG");
            IsoDateCheck(climateAvg.MAC_DATE, "MAC_DATE");
            //check if MAP, MAR, MAS, MAC_YEARS must only be positive
            return response;
        }
        public static Response ValidateDistManResponse(GRP_DM distMan, IcosDbContext db)
        {
            IsoDateCheck(distMan.DM_DATE, "DM_DATE");
            IsoDateCheck(distMan.DM_DATE_START, "DM_DATE_START");
            IsoDateCheck(distMan.DM_DATE_END, "DM_DATE_END");
            if(!String.IsNullOrEmpty(distMan.DM_DATE)&& !String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                //{ 4, "$V0$ and $V1$/$V2$ are mutually exclusive"},
                errorCode = 4;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$", distMan.DM_DATE, "$V1$", distMan.DM_DATE_START, "$V2$", distMan.DM_DATE_END);
            }
            if ( !String.IsNullOrEmpty(distMan.DM_DATE_START) && !String.IsNullOrEmpty(distMan.DM_DATE_END))
            {
                if(String.Compare(distMan.DM_DATE_START, distMan.DM_DATE_END) > 0)
                {
                    //{ 6, "$V0$ must be greater than $V1$"},
                    errorCode = 6;
                    response.Code += errorCode;
                    Err = ErrorCodes.GeneralErrors[errorCode];
                    Globals.FormatError(ref Err, "$V0$", distMan.DM_DATE_END, "$V1$", distMan.DM_DATE_START);
                }
            }
            ItemInBadmList("DM_ENCROACH", distMan.DM_ENCROACH, "GRP_DM", (int)Globals.CvIndexes.DM_ENCROACH, db);
            ItemInBadmList("DM_AGRICULTURE", distMan.DM_AGRICULTURE, "GRP_DM", (int)Globals.CvIndexes.DM_AGRICULTURE, db);
            ItemInBadmList("DM_EXT_WEATHER", distMan.DM_EXT_WEATHER, "GRP_DM", (int)Globals.CvIndexes.DM_EXT_WEATHER, db);
            ItemInBadmList("DM_FERT_M", distMan.DM_FERT_M, "GRP_DM", (int)Globals.CvIndexes.DM_FERT_M, db);
            ItemInBadmList("DM_FERT_O", distMan.DM_FERT_M, "DM_FERT_O", (int)Globals.CvIndexes.DM_FERT_O, db); 
            ItemInBadmList("DM_FIRE", distMan.DM_FERT_M, "DM_FIRE", (int)Globals.CvIndexes.DM_FIRE, db);
            ItemInBadmList("DM_FORESTRY", distMan.DM_FERT_M, "DM_FORESTRY", (int)Globals.CvIndexes.DM_FORESTRY, db);
            ItemInBadmList("DM_GRAZE", distMan.DM_FERT_M, "DM_GRAZE", (int)Globals.CvIndexes.DM_GRAZE, db);
            ItemInBadmList("DM_INS_PATH", distMan.DM_FERT_M, "DM_INS_PATH", (int)Globals.CvIndexes.DM_INS_PATH, db);
            ItemInBadmList("DM_PESTICIDE", distMan.DM_FERT_M, "DM_PESTICIDE", (int)Globals.CvIndexes.DM_PESTICIDE, db);
            ItemInBadmList("DM_PLANTING", distMan.DM_FERT_M, "DM_PLANTING", (int)Globals.CvIndexes.DM_PLANTING, db);
            ItemInBadmList("DM_TILL", distMan.DM_FERT_M, "DM_TILL", (int)Globals.CvIndexes.DM_TILL, db); 
            ItemInBadmList("DM_WATER", distMan.DM_FERT_M, "DM_WATER", (int)Globals.CvIndexes.DM_WATER, db);
            ItemInBadmList("DM_GENERAL", distMan.DM_FERT_M, "DM_GENERAL", (int)Globals.CvIndexes.DM_GENERAL, db);

            return response;
        }

        /////////////////////////////////
        ///

        private static void ItemInBadmList(string name, string value, string groupName, int cvIndex, IcosDbContext db)
        {
            if (String.IsNullOrEmpty(value)) return;
            string bmList = db.BADMList.Where(item => item.cv_index == cvIndex).Select(x => x.vocabulary).FirstOrDefault();
            var res = db.BADMList.Where(item => item.cv_index == cvIndex)
                                       .Any(item => (String.Compare(item.shortname, value, true) == 0));
            if (!res)
            {
                errorCode = 7;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$", value, "$V1$", bmList, "$GRP$", groupName);
                response.Messages.Add(name, Err);
            }
            //return res;
        }

        private static void MissingDate(string dateValue, string name, string groupName)
        {
            if (String.IsNullOrEmpty(dateValue))
            {
                errorCode = 1;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$", name, "$GRP$", groupName);
                response.Messages.Add(name, Err);
            }
        }

        private static void IsoDateCheck(string dateValue, string name)
        {
            errorCode = ValidateIsoDate(dateValue);
            if (errorCode > 0)
            {
                errorCode = 2;
                response.Code += errorCode;
                Err = ErrorCodes.GeneralErrors[errorCode];
                Globals.FormatError(ref Err, "$V0$", name, "$V1$", dateValue);
                response.Messages.Add(name, Err);
            }
        }

        private static int ValidateIsoDate(string input)
        {
            int currentYear = DateTime.Now.Year;

            string numReg = "^[0-9]+$";
            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if (input == "")
            {
                return 0;
            }
            if ((input.Length) % 2 > 0 || input.Length > 12)
            {
                return 2;
            }
            Match match = Regex.Match(input, numReg, RegexOptions.IgnoreCase);
            if (!match.Success) return 2;
            else
            {
                string subYear = input.Substring(0, 4);
                string subMonth = "";
                string subDay = "";
                string subHour = "";
                string subMins = "";
                ////////////////
                int iYear = 0;
                iYear = int.Parse(subYear);

                var iMonth = 0;
                var iDay = 0;
                var iHour = 0;
                var iMins = 0;
                if (iYear < 1800 || iYear > (currentYear + 2))
                {
                    return 3;
                }
                if (input.Length > 4)
                {
                    subMonth = input.Substring(4, 2);
                    iMonth = int.Parse(subMonth);
                    if (iMonth < 1 || iMonth > 12)
                    {
                        return 2;
                    }
                }
                if (input.Length > 6)
                {
                    subDay = input.Substring(6, 2);
                    iDay = int.Parse(subDay);
                    if (iMonth != 2)
                    {

                        if (iDay > daysInMonth[iMonth - 1])
                        {
                            return 2;
                        }

                    }
                    else
                    {
                        if (isLeap(iYear))
                        {
                            if (iDay > 29)
                            {
                                return 2;
                            }
                        }
                        else
                        {
                            if (iDay > 28)
                            {
                                return 2;
                            }
                        }
                    }
                }
                if (input.Length > 8)
                {
                    subHour = input.Substring(8, 2);
                    iHour = int.Parse(subHour);
                    if (iHour >= 24)
                    {
                        return 2;
                    }
                }
                if (input.Length > 10)
                {
                    subMins = input.Substring(10, 2);
                    iMins = int.Parse(subMins);
                    if (iMins > 59)
                    {
                        return 2;
                    }
                }

            }
            return 0;
        }

        private static bool isLeap(int yy)
        {
            if ((yy % 400 == 0 || yy % 100 != 0) && (yy % 4 == 0))
            {
                return true;

            }
            else
            {
                return false;
            }

        }
    }
}
