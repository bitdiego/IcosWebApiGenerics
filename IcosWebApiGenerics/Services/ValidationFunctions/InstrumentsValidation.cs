using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions
{
    public class InstrumentsValidation
    {
        public static int SerialNumberCheck(string instModel, string instSn)
        {
            if (!Globals.regulars.ContainsKey(instModel.ToLower())) return 0;
            string instReg = Globals.regulars[instModel.ToLower()];
            Match instMatch = Regex.Match(instSn, instReg, RegexOptions.IgnoreCase);
            if (!instMatch.Success)
            {
                return (int)Globals.ErrorValidationCodes.WRONG_SERIALNUMBER_FORMAT;
            }
            return 0;
        }
        public static async Task<int> LastExpectedOpByDateAsync(GRP_INST inst, IcosDbContext db)
        {

            if (String.Compare(inst.INST_MODEL, "purchase", true) == 0)
            {
                if (await db.GRP_INST.AnyAsync(xinst => xinst.INST_FACTORY.ToLower() == "purchase" && String.Compare(xinst.INST_MODEL, inst.INST_MODEL, true) == 0
                                                    && String.Compare(xinst.INST_SN, inst.INST_SN, true) == 0 && xinst.SiteId == inst.SiteId && xinst.DataStatus == 0))
                {
                    return (int)Globals.ErrorValidationCodes.GRP_INST_ALREADY_PURCHASED;
                }
            }
            else
            {
                var item = await db.GRP_INST.FirstOrDefaultAsync(xinst => (string.Compare(xinst.INST_FACTORY, "purchase", true) == 0)
                                                                        && (string.Compare(xinst.INST_MODEL, inst.INST_MODEL, true) == 0)
                                                                        && (string.Compare(xinst.INST_SN, inst.INST_SN, true) == 0) && xinst.SiteId == inst.SiteId);
                if (item == null)
                {
                    return (int)Globals.ErrorValidationCodes.GRP_INST_NOT_PURCHASE;
                }
                else
                {
                    if (String.Compare(inst.INST_DATE, item.INST_DATE) < 0)
                    {
                        return (int)Globals.ErrorValidationCodes.GRP_INST_NOT_PURCHASE;
                    }
                }
            }

            return 0;
        }

        public async Task<int> LastExpectedOpByDateAsync(GRP_EC model, int siteId, IcosDbContext _context)
        {
            int result = 0;
            var modelEcType = model.EC_TYPE;
            GRP_EC lastRecord = null;
            string cDate = String.IsNullOrEmpty(model.EC_DATE) ? model.EC_DATE_START : model.EC_DATE;
            //is there any previous sensor??
            var isAnyEcModel = await _context.GRP_EC.AnyAsync(ec => ec.SiteId == siteId);
            if (!isAnyEcModel)
            {
                if (String.Compare(model.EC_TYPE, "installation", true) != 0)
                {
                    return (int)Globals.ErrorValidationCodes.INST_NOT_VALID_OPERATION;
                }
            }
            else
            {
                try
                {
                    bool bDateCompareResult = false;

                    var pp = await _context.GRP_EC.Where(item => item.EC_MODEL == model.EC_MODEL && item.EC_SN == model.EC_SN
                    && (item.EC_TYPE == "Removal" || item.EC_TYPE == "Installation") && item.SiteId == siteId)
                    .OrderByDescending(ec => ec.EC_DATE).ToListAsync();

                    lastRecord = pp.FirstOrDefault(item => String.Compare(item.EC_DATE, cDate) < 0);

                    if (lastRecord == null)
                    {
                        lastRecord = pp.FirstOrDefault(item => String.Compare(item.EC_DATE, cDate) > 0);
                    }
                    if (lastRecord == null)
                    {
                        if (String.Compare(model.EC_TYPE, "installation", true) != 0)
                        {
                            return (int)Globals.ErrorValidationCodes.INST_NOT_VALID_OPERATION;
                        }
                    }
                    else
                    {
                        string lastRecordLastOp = lastRecord.EC_TYPE;
                        if (model.EC_TYPE.ToLower() == "installation")
                        {
                            //if model.ec_type==installation && lastRecordLastOp==installation -> error
                            if (lastRecordLastOp.ToLower() == "installation")
                                return (int)Globals.ErrorValidationCodes.INST_ALREADY_INSTALLED;
                            else
                            {
                                //model.ec_type == installation && lastRecordLastOp == removal: check dates
                                //model.ec_date (installation) should be > removal date
                                bDateCompareResult = Globals.DatesCompare(lastRecord.EC_DATE, model.EC_DATE);
                            }
                        }
                        else if (model.EC_TYPE.ToLower() == "removal")
                        {
                            //if model.ec_type==removal && lastRecordLastOp==removal -> error
                            if (lastRecordLastOp.ToLower() == "removal")
                                return (int)Globals.ErrorValidationCodes.INST_ALREADY_REMOVED;
                            else
                            {
                                //model.ec_type==removal && lastRecordLastOp==installation: check dates
                                //model.ec_date (removal) should be > installation date
                                bDateCompareResult = Globals.DatesCompare(lastRecord.EC_DATE, model.EC_DATE);
                            }
                        }
                        else
                        {
                            //if model.ec_type!=installation && model.ec_type!=removal && lastRecordLastOp!=installation -> error
                            if (lastRecordLastOp.ToLower() == "removal")
                                return (int)Globals.ErrorValidationCodes.INST_ALREADY_REMOVED;
                            else
                            {
                                //lastRecordOP is installation: ok
                                //model.ec_type is calibration, maintenance, comment etc etc: check dates
                                //_dateOp shoud be >= lastRecordOP install date
                                string _dateOp = (String.IsNullOrEmpty(model.EC_DATE)) ? model.EC_DATE_START : model.EC_DATE;
                                bDateCompareResult = Globals.DatesCompare(lastRecord.EC_DATE, _dateOp);
                            }
                        }
                        if (bDateCompareResult)
                        {
                            return (int)Globals.ErrorValidationCodes.INST_NOT_VALID_OPERATION;
                        }
                    }

                }
                catch (Exception e)
                {

                }
            }
            switch (modelEcType.ToLower())
            {
                case "installation":
                    //last EC_TYPE must be removal, or no record related to this model / sn 

                    if (lastRecord != null)
                    {
                        if (String.Compare(lastRecord.EC_TYPE, "installation", true) == 0)
                        {
                            result = (int)Globals.ErrorValidationCodes.INST_ALREADY_INSTALLED;
                        }
                        else
                        {
                            if (String.Compare(lastRecord.EC_DATE, model.EC_DATE, true) > 0)
                            {
                                result = (int)Globals.ErrorValidationCodes.INST_PURCHASE_DATE_GREATER_THAN_INST_OP_DATE;
                            }
                        }
                    }

                    //must have ONLY EC_DATE
                    if (!String.IsNullOrEmpty(model.EC_DATE_START) || !String.IsNullOrEmpty(model.EC_DATE_END))
                    {
                        result = (int)Globals.ErrorValidationCodes.INSTALLATION_ONLY_DATE;
                    }
                    //must have EC_HEIGHT,EC_EASTWARD_DIST,EC_NORTHWARD_DIST, [EC_SA_HEAT,EC_SA_OFFSET_N if SA_Gill.... ]
                    if (model.EC_HEIGHT == null || model.EC_NORTHWARD_DIST == null || model.EC_EASTWARD_DIST == null)
                    {
                        result = (int)Globals.ErrorValidationCodes.EC_MANDATORY_MISSING;
                    }
                    if (model.EC_MODEL.ToLower().StartsWith("sa-gill"))
                    {
                        if (model.EC_SA_HEAT == null || model.EC_SA_OFFSET_N == null)
                        {
                            result = (int)Globals.ErrorValidationCodes.EC_SA_MANDATORY_MISSING;
                        }
                    }
                    //possible: {EC_SAMPLING_INT,EC_LOGGER,EC_FILE},EC_SA_WIND_FORMAT,EC_SA_GILL_ALIGN,EC_SA_GILL_PCI,EC_GA_FLOW_RATE,EC_GA_LICOR_FM_SN,EC_GA_LICOR_TP_SN,EC_GA_LICOR_AIU_SN

                    //bool isBoundOk = IsBoundVariablesCorrect<decimal?>(3, model.EC_SAMPLING_INT, (decimal?)model.EC_LOGGER, (decimal?)model.EC_FILE);
                    bool iBoundOk = GeneralValidation.CountBoundedProps<GRP_EC>(model, 3, "EC_SAMPLING_INT", "EC_LOGGER", "EC_FILE");


                    int notNull = 0;
                    if (model.EC_SAMPLING_INT != null) ++notNull;
                    if (model.EC_LOGGER != null) ++notNull;
                    if (model.EC_FILE != null) ++notNull;
                    if (/*notNull != 0 && notNull != 3*/!iBoundOk)
                    {
                        result = (int)Globals.ErrorValidationCodes.SAMPLING_LOGGER_FILE_ERROR;
                    }

                    break;
                case "removal":
                    //must have ONLY EC_DATE
                    if (!String.IsNullOrEmpty(model.EC_DATE_START) || !String.IsNullOrEmpty(model.EC_DATE_END))
                    {
                        result = (int)Globals.ErrorValidationCodes.INSTALLATION_ONLY_DATE;
                    }
                    //possible if there is a previous Installation: if so, compare the _date
                    break;
                case "maintenance":
                    //must have ONLY EC_DATE if one of facultative variables sent
                    //EC_SAMPLING_INT,EC_LOGGER,EC_FILE,EC_SA_WIND_FORMAT,EC_SA_GILL_ALIGN,EC_SA_GILL_PCI,EC_GA_FLOW_RATE,EC_GA_LICOR_FM_SN,EC_GA_LICOR_TP_SN,EC_GA_LICOR_AIU_SN
                    string[] fac = { "EC_SAMPLING_INT", "EC_LOGGER", "EC_FILE", "EC_SA_WIND_FORMAT", "EC_SA_GILL_ALIGN", "EC_SA_GILL_PCI", "EC_GA_FLOW_RATE", "EC_GA_LICOR_FM_SN", "EC_GA_LICOR_TP_SN", "EC_GA_LICOR_AIU_SN" };

                    if (!String.IsNullOrEmpty(model.EC_DATE_START) || !String.IsNullOrEmpty(model.EC_DATE_END))
                    {
                        result = (int)Globals.ErrorValidationCodes.INSTALLATION_ONLY_DATE;
                    }
                    //last EC_TYPE must be installation
                    if (lastRecord != null)
                    {
                        if (String.Compare(lastRecord.EC_TYPE, "removal", true) == 0)
                        {
                            result = (int)Globals.ErrorValidationCodes.INST_NOT_VALID_OPERATION;
                        }
                        /*else
                        {
                            if (String.Compare(lastRecord.EC_DATE, model.EC_DATE, true) > 0)
                            {
                                result = 11112;
                            }
                        }*/
                    }
                    /*else
                    {
                        result = 11113;
                    }*/

                    if (!GeneralValidation.CountBoundedProps<GRP_EC>(model, 3, "EC_SAMPLING_INT", "EC_LOGGER", "EC_FILE"))
                    {
                        result = (int)Globals.ErrorValidationCodes.SAMPLING_LOGGER_FILE_ERROR;
                    }

                    break;
                case "field calibration":
                case "field calibration check":
                    if ((model.EC_GA_CAL_CO2_OFFSET == null && model.EC_GA_CAL_CO2_REF != null) ||
                        (model.EC_GA_CAL_CO2_OFFSET != null && model.EC_GA_CAL_CO2_REF == null))
                    {
                        result = (int)Globals.ErrorValidationCodes.EC_GA_FIELD_CALIBRATION;
                    }
                    break;
                default:
                    //Disturbance,Field Cleaning,Firmware Update,Parts Substitution,General comment
                    //no further checks to perform
                    break;
            }

            if (result == 0)
            {
                if ((String.Compare(model.EC_TYPE, "installation", true) == 0) || (String.Compare(model.EC_TYPE, "Maintenance", true) == 0))
                {
                    if (model.EC_LOGGER != null && model.EC_FILE != null)
                    {
                        //check if EC_LOGGER and EC_FILE are present in GRP_FILE for this site and for EC type
                        //maybe create a general static function??
                    }
                }
            }

            return result;
        }

        public static async Task<int> InstrumentInGrpInst(GRP_INST model, int siteId, IcosDbContext db)
        {
            int resp = 0;
            if (String.Compare(model.INST_FACTORY.ToLower(), "purchase") == 0)
            {
                return 0;
            }
            var inst = await db.GRP_INST.Where(md => md.INST_MODEL == model.INST_MODEL && md.INST_SN == model.INST_SN && md.SiteId == siteId && md.INST_FACTORY.ToLower() == "purchase")
                                              .OrderBy(md => md.INST_DATE).FirstOrDefaultAsync();
            if (inst == null)
            {
                resp = (int)Globals.ErrorValidationCodes.NOT_IN_GRP_INST;
            }
            else
            {
                string _iDate = inst.INST_DATE;
                if (String.Compare(_iDate, model.INST_DATE) > 0)
                {
                    resp = (int)Globals.ErrorValidationCodes.INST_PURCHASE_DATE_GREATER_THAN_INST_OP_DATE;
                }
            }
            return resp;
        }

        public static async Task<int> SensorInGrpInst(string model, string sn, string date, int siteId, IcosDbContext db)
        {
            int resp = 0;

            var inst = await db.GRP_INST.Where(md => md.INST_MODEL == model && md.INST_SN == sn && md.SiteId == siteId && md.INST_FACTORY.ToLower() == "purchase")
                                              .OrderBy(md => md.INST_DATE).FirstOrDefaultAsync();
            if (inst == null)
            {
                resp = (int)Globals.ErrorValidationCodes.NOT_IN_GRP_INST;
            }
            else
            {
                string _iDate = inst.INST_DATE;
                if (String.Compare(_iDate, date) > 0)
                {
                    resp = (int)Globals.ErrorValidationCodes.INST_PURCHASE_DATE_GREATER_THAN_INST_OP_DATE;
                }
            }
            return resp;
        }
    }
}
