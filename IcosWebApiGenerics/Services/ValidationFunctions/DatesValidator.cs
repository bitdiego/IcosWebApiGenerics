using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public class DatesValidator
    {
        public static int IsoDateCheck(string dateValue, string name)
        {
            int errorCode = 0;
            if (String.IsNullOrEmpty(dateValue))
            {
                return errorCode;
            }
            errorCode = ValidateIsoDate(dateValue);
            if (errorCode > 0)
            {
                return 2;
            }
            return errorCode;
        }

        public static int ValidateIsoDate(string input)
        {
            int currentYear = DateTime.Now.Year;

            string numReg = "^[0-9]+$";
            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

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

        public static int IsoDateCompare(string date, string dateStart, string dateEnd)
        {
            //if(date==null)
            if (String.IsNullOrEmpty(date) && String.IsNullOrEmpty(dateStart) && String.IsNullOrEmpty(dateEnd))
            {
                return 200;
            }
            if (!String.IsNullOrEmpty(date) && (!String.IsNullOrEmpty(dateStart) || !String.IsNullOrEmpty(dateEnd)))
            {
                return 201;
            }
            if (!String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(dateStart) && !String.IsNullOrEmpty(dateEnd))
            {
                return 201;
            }
            if (String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(dateStart) && String.IsNullOrEmpty(dateEnd))
            {
                return 202;
            }
            if (String.IsNullOrEmpty(date) && String.IsNullOrEmpty(dateStart) && !String.IsNullOrEmpty(dateEnd))
            {
                return 203;
            }
            if (String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(dateStart) && !String.IsNullOrEmpty(dateEnd))
            {
                if (String.Compare(dateStart, dateEnd) > 0)
                {
                    return 204;
                }
            }
            if (!String.IsNullOrEmpty(date) && (!String.IsNullOrEmpty(dateStart) || !String.IsNullOrEmpty(dateEnd)))
            {
                return 201;
            }
            return 0;
        }
    }
}
