using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.MeteoValidation
{
    public class MeteoValidation
    {
        private static int errorCode = 0;
       // public object ValidationUtils { get; private set; }

        public static async Task ValidateBmResponseAsync(GRP_BM bmInst, IcosDbContext db, Response response)
        {
            if (!String.IsNullOrEmpty(bmInst.BM_MODEL) || !String.IsNullOrEmpty(bmInst.BM_SN))
            {
                string dateToCheck = String.IsNullOrEmpty(bmInst.BM_DATE) ? bmInst.BM_DATE_START : bmInst.BM_DATE;
                if (!String.IsNullOrEmpty(dateToCheck))
                {
                    errorCode = await InstrumentsValidation.SensorInGrpInst(bmInst.BM_MODEL, bmInst.BM_SN, dateToCheck, bmInst.SiteId, db);
                    if (errorCode > 0)
                    {
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BM_MODEL");
                    }
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(bmInst.BM_MODEL, "BM_MODEL", "GRP_BM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BM_MODEL", "$V0$", "BM_MODEL", "$GRP$", "GRP_BM");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(bmInst.BM_SN, "BM_SN", "GRP_BM");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BM_SN", "$V0$", "BM_SN", "$GRP$", "GRP_BM");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(bmInst.BM_TYPE, "BM_TYPE", "GRP_BM");
            if (errorCode != 0)
            {

                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BM_TYPE", "$V0$", "BM_TYPE", "$GRP$", "GRP_BM");
            }

            errorCode = DatesValidator.IsoDateCompare(bmInst.BM_DATE, bmInst.BM_DATE_START, bmInst.BM_DATE_END);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "BM_DATE", "$V0$", "BM_DATE", "$V1$", "BM_DATE_START", "$V2$", "BM_DATE_END", "$GRP$", "GRP_BM");
            }

            errorCode = await InstrumentsValidation.LastExpectedOpByDateAsync(bmInst, db);
        }

        private int ValidateByBmType(GRP_BM model, IcosDbContext db)
        {
            int result = 0;
            string bmType = model.BM_TYPE.ToLower();
            string bmVhr = model.BM_VARIABLE_H_V_R;
            if (!String.IsNullOrEmpty(bmVhr))
            {
                result = VarHVRCorrect(bmVhr, db);
                if (result > 0) return result;
            }
            if (model.BM_LOGGER != null && model.BM_FILE != null && model.BM_SAMPLING_INT != null)
            {
                result = LoggerFileCheck(model, db);
            }

            switch (bmType)
            {
                case "installation":
                    /*
                     * mandatory:
                     * BM_HEIGHT
                        BM_EASTWARD_DIST
                        BM_NORTHWARD_DIST
                    non mandatory but bound together:
                        BM_VARIABLE_H_V_R
                        BM_SAMPLING_INT
                        BM_LOGGER
                        BM_FILE
                     * */
                    if (model.BM_HEIGHT == null || model.BM_EASTWARD_DIST == null || model.BM_NORTHWARD_DIST == null)
                    {
                        return (int)Globals.ErrorValidationCodes.BM_INSTALLATION_HEIGHT_EAST_NORTH_MANDATORY;
                    }
                    ///DIEGO:::FIX IT!
                    if (!GeneralValidation.CountBoundedProps<GRP_BM>(model, 4, "BM_VARIABLE_H_V_R", "BM_SAMPLING_INT", "BM_LOGGER", "BM_FILE"))
                    {
                        return (int)Globals.ErrorValidationCodes.BM_INSTALLATION_VAR_SAMPLING_LOGGER_FILE;
                    }
                    break;
                case "maintenance":
                    break;
                case "variable map":
                    /*
                     * BM_VARIABLE_H_V_R
                        BM_SAMPLING_INT
                        BM_LOGGER
                        BM_FILE
                    */
                    if (String.IsNullOrEmpty(bmVhr) || model.BM_SAMPLING_INT == null || model.BM_FILE == null || model.BM_LOGGER == null)
                    {
                        return (int)Globals.ErrorValidationCodes.BM_VARMAP_SAMPLING_FILE_LOGGER_MANDATORY;
                    }
                    break;
                case "disturbance":
                case "field calibration":
                case "field calibration check":
                case "field cleaning":
                case "firmware update":
                case "parts substitution":
                    //check BM_VARIABLE_H_V_R validity; performed elsewhere
                    break;
                case "connection failure":
                case "general comment":
                    break;
            }

            return result;
        }

        private int VarHVRCorrect(string varHVR, IcosDbContext db)
        {
            if (varHVR.Where(ch => ch == '_').Count() < 3)
            {
                return (int)Globals.ErrorValidationCodes.BM_VARIABLE_H_V_R_WRONG_FORMAT;
            }
            int count = 0;
            string temp = "";
            int i = varHVR.Count() - 1;
            for (; i > 0; i--)

            {
                if (varHVR[i] == '_')
                {
                    if (++count >= 3) break;
                }
            }
            temp = varHVR.Substring(0, i);

            var inVarList = db.BADMList.Any(item => item.cv_index == 73 && item.shortname.ToLower() == temp.ToLower());
            if (!inVarList)
            {
                return (int)Globals.ErrorValidationCodes.BM_VARIABLE_H_V_R_NOT_FOUND;
            }
            return 0;
        }

        public int LoggerFileCheck(GRP_BM model, IcosDbContext db)
        {
            string bmDateToCompare = String.IsNullOrEmpty(model.BM_DATE) ? model.BM_DATE_START : model.BM_DATE;
            var exist = db.GRP_FILE.Where(file => file.FILE_ID == model.BM_FILE &&
                                                      file.FILE_LOGGER_ID == model.BM_LOGGER &&
                                                      file.SiteId == model.SiteId && file.DataStatus == 0 &&
                                                      file.FILE_DATE.CompareTo(bmDateToCompare) <= 0 &&
                                                      file.FILE_TYPE == "BM").Any();
            if (!exist)
            {
                return (int)Globals.ErrorValidationCodes.BM_LOGGER_FILE_MISSING_IN_GRP_FILE;
            }
            return 0;
        }
    }
}
