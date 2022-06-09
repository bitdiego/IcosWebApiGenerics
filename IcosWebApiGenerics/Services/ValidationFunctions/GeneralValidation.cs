using IcosWebApiGenerics.Data;
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

        public static int ItemInSamplingPointGroupAsync(string modelPlotId, string modelDate, int siteId, IcosDbContext db)
        {
            if (String.Compare(modelPlotId, "Outside_CP", true) == 0) return 0;
            var item = db.GRP_PLOT.Where(plot => plot.SiteId == siteId && plot.DataStatus == 0 &&
                                                String.Compare(plot.PLOT_ID, modelPlotId) == 0 &&
                                                String.Compare(plot.PLOT_DATE, modelDate) <= 0).FirstOrDefault();
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
            if (value == null)
            {
                return 1;
            }
            return 0;
        }

    }
}
