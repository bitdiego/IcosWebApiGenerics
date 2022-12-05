using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public static class GeneralValidation
    {
        public static bool CountBoundedProps<T>(T model, int bound, params string[] vars)
        {
            Type myType = model.GetType();
            IList<PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());

            var subList = props.Where(item => vars.Contains(item.Name)).ToList();

            var countValue = subList.Count(item => item.GetValue(model, null) != null);

            return (countValue == 0) || (countValue == bound);
        }

        public static bool XORNull<T>(string obja, string objb) where T : IComparable<T>
        {
            if (String.IsNullOrEmpty(obja) && !String.IsNullOrEmpty(objb))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(obja) && String.IsNullOrEmpty(objb))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidPattern(string pattern, string regex)
        {
            Match match = Regex.Match(pattern, regex);
            return match.Success;
        }

        public static bool IsValidPlotString(string plot, int group)
        {
            int[] allowedOutside = { (int)Globals.Groups.GRP_FLSM, (int)Globals.Groups.GRP_TREE, (int)Globals.Groups.GRP_LITTERPNT, 
                                 (int)Globals.Groups.GRP_WTDPNT, (int)Globals.Groups.GRP_D_SNOW };
            List<int> notSP_II_Valid = new List<int>() { (int)Globals.Groups.GRP_WTDPNT, (int)Globals.Groups.GRP_D_SNOW };
            bool isMatch = true;
            try
            {
                Match match;

                if (plot.ToLower().StartsWith("cp"))
                {
                    match = Regex.Match(plot, Globals.cpReg, RegexOptions.IgnoreCase);
                    isMatch = match.Success;
                }
                else if (plot.ToLower().StartsWith("sp-i_"))
                {
                    match = Regex.Match(plot, Globals.sp1Reg, RegexOptions.IgnoreCase);
                    isMatch = match.Success;
                }
                else if (plot.ToLower().StartsWith("sp-ii"))
                {
                    if (notSP_II_Valid.Any(id => id == group))
                    {
                        isMatch = false;
                    }
                    else
                    {
                        match = Regex.Match(plot, Globals.sp2Reg, RegexOptions.IgnoreCase);
                        isMatch = match.Success;
                    }
                }
                else
                {
                    if (String.Compare(plot, "outside_cp", true) == 0)
                    {
                        //check for which groups 'outside_cp' is allowed
                        if (allowedOutside.Contains(group))
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

        public static async Task<int> ItemInSamplingPointGroupAsync(string modelPlotId, string modelDate, int siteId, IcosDbContext db)
        {
            if (String.Compare(modelPlotId, "Outside_CP", true) == 0) return 0;
            var item = await db.GRP_PLOT.Where(plot => plot.SiteId == siteId && plot.DataStatus == 0 &&
                                                String.Compare(plot.PLOT_ID, modelPlotId) == 0 &&
                                                String.Compare(plot.PLOT_DATE, modelDate) <= 0).FirstOrDefaultAsync();
            var cippa = db.GRP_PLOT.Where(plot => plot.SiteId == siteId && plot.DataStatus == 0 &&
                                                String.Compare(plot.PLOT_ID, modelPlotId) == 0 &&
                                                String.Compare(plot.PLOT_DATE, modelDate) <= 0).ToQueryString();
            if (item == null)
            {
                return (int)Globals.ErrorValidationCodes.PLOT_ID_NOT_FOUND;
            }
            return 0;
        }

        public static async Task<int> ItemInBadmListAsync(string value, int cvIndex, IcosDbContext db)
        {
            string bmList = await db.BADMList.Where(item => item.cv_index == cvIndex).Select(x => x.vocabulary).FirstOrDefaultAsync();

            var res = await db.BADMList.Where(item => item.cv_index == cvIndex)
                                        .AnyAsync(item => item.shortname== value);
            if (!res)
            {
                return 7;
            }
            return 0;
        }

        public static async Task<int> IsInControlledVocabulary(string value, int cvIndex, IcosDbContext _context)
        {
            var item = await _context.BADMList.Where(bm => bm.cv_index == cvIndex && bm.shortname == value).FirstOrDefaultAsync();
            if (item == null) return 7;
            if (String.Compare(item.shortname, value, false) != 0)
            {
                //raise warn string...
                //WarningString += Environment.NewLine + "Warning: case differences in entered item. Found " + v.Value + " instead of " + item + " in cell " + v.Cell;
                //WarningString += ". Item value will be corrected";
                value = item.shortname;
            }

            return 0;
        }

        public static bool IsAnyPropNotNull<T>(T model, params string[] vars)
        {
            Type myType = model.GetType();
            IList<PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());
            var subList = props.Where(item => vars.Contains(item.Name)).ToList();
            var isAnyVAlue = subList.Any(item => item.GetValue(model, null) != null);

            return isAnyVAlue;
        }

        public static bool FindMandatoryNull<T>(T model, params string[] vars)
        {
            Type myType = model.GetType();
            IList<PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());

            var subList = props.Where(item => vars.Contains(item.Name)).ToList();

            var isAnyVAlue = subList.Any(item => item.GetValue(model, null) == null);

            return isAnyVAlue;
        }

        public static int MissingMandatoryData<T>(T value, string name, string groupName)
        {
            int res = 0;
            if (value == null)
            {
                return 1;
            }
            else
            {
                var tt = typeof(T);
                switch (tt.Name.ToLower())
                {
                    case "int32":
                        int iVal = Convert.ToInt32(value);
                        if (iVal < -9998) res = 1;
                        break;
                    case "decimal":
                        decimal dVal= Convert.ToDecimal(value);
                        if (dVal < -9998) res = 1;
                        break;
                    case "string":
                        break;
                }
               
            }
            return res;
        }

        /*public static void ManageMandatoryData(IEnumerable<Variable> varList, Response response)
        {
            foreach(var variable in varList)
            {
                var value = variable.Value;
                if (value == null)
                {
                    //return 1;
                    response.Code += 1;
                    response.FormatError(ErrorCodes.GeneralErrors[1], variable.Name, "$V0$", variable.Name, "$GRP$", "getGroupByIdgroupofVar()");
                }
                else
                {
                    var tt = variable.Unit;
                    int res=0;
                    switch (tt)
                    {
                        case 3:
                        case 4:
                            int iVal = Convert.ToInt32(value);
                            if (iVal < -9998) res = 1;
                            break;
                        case 2:
                            decimal dVal = Convert.ToDecimal(value);
                            if (dVal < -9998) res = 1;
                            break;
                        case 1:
                            break;
                    }
                    if (res > 0)
                    {
                        response.Code += res;
                        response.FormatError(ErrorCodes.GeneralErrors[1], variable.Name, "$V0$", variable.Name, "$GRP$", "getGroupByIdgroupofVar()");
                    }
                    
                }
            }
        }
        */
        public static bool DecimalValueInRange(decimal min, decimal max, decimal value)
        {
            return value >= min && value <= max;
        }

    }
}
