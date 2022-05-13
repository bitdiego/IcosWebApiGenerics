using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public static class NumericValidation
    {
        public static bool IsValidNumber(this string s)
        {
            decimal d;
            var res = decimal.TryParse(s, out d); 
            //Regex regex = new Regex(@"^-?\d*\.{0,1}\d+$");
            //var res = regex.IsMatch(s);
            return res;
        }

        public static bool IsValidPositiveNumber(this string s)
        {
            decimal d;
            var res = decimal.TryParse(s, out d);
            if (res)
            {
                res = d >= 0;
            }
            //Regex regex = new Regex(@"^-?\d*\.{0,1}\d+$");
            //var res = regex.IsMatch(s);
            return res;
        }

        public static bool IsValidInteger(this string s)
        {
            int num;
            bool success = int.TryParse(s, out num);
            return success;
        }

        public static bool IsValidPositiveInteger(this string s)
        {
            int num;
            bool success = int.TryParse(s, out num);
            if (success)
            {
                success = num >= 0;
            }
            return success;
        }

        public static bool IsDecimalNumberInRange(this string s, decimal min, decimal max)
        {
            decimal num;
            bool success = decimal.TryParse(s, out num);
            if (success)
            {
                success = (num >= min && num <= max);
            }
            return success;
        }

        public static bool IsIntegerNumberInRange(this string s, int min, int max)
        {
            int num;
            bool success = int.TryParse(s, out num);
            if (success)
            {
                success = (num >= min && num <= max);
            }
            return success;
        }
    }
}
