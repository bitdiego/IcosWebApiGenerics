using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.DataRecordValidation
{
    public class DataRecordValidation
    {
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        public static async Task ValidateFileResponseAsync(GRP_FILE file, IcosDbContext context, Response response)
        {
            errorCode = GeneralValidation.MissingMandatoryData<int>(file.FILE_ID, "FILE_ID", "GRP_FILE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FILE_ID", "$V0$", "FILE_ID", "$GRP$", "GRP_FILE");
            }
            else
            {
                if (file.FILE_ID <= 0)
                {
                    //must be strictly positive
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<int>(file.FILE_LOGGER_ID, "FILE_LOGGER_ID", "GRP_FILE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FILE_LOGGER_ID", "$V0$", "FILE_LOGGER_ID", "$GRP$", "GRP_FILE");
            }
            else
            {
                if (file.FILE_LOGGER_ID <= 0)
                {
                    //must be strictly positive
                }

                var log = await context.GRP_LOGGER.AnyAsync(logger => logger.LOGGER_ID == file.FILE_LOGGER_ID && logger.SiteId == file.SiteId);
                if (!log)
                {
                    errorCode = (int)Globals.ErrorValidationCodes.LOGGER_ID_NOT_REGISTERED;
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpFileErrors[errorCode], "FILE_LOGGER_ID");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(file.FILE_DATE, "FILE_DATE", "GRP_FILE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FILE_DATE", "$V0$", "FILE_DATE", "$GRP$", "GRP_FILE");
            }
            
            errorCode = DatesValidator.IsoDateCheck(file.FILE_DATE, "FILE_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FILE_DATE", "$V0$", "FILE_DATE", "$V1$", file.FILE_DATE);
            }

            errorCode = CheckFileFormat(file.FILE_FORMAT, file.FILE_EXTENSION, file.SiteId);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FILE_FORMAT");
            }

            errorCode = CheckHeadConstraints(file.FILE_FORMAT, file.FILE_HEAD_NUM, file.FILE_HEAD_VARS, file.FILE_HEAD_TYPE);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "FILE_FORMAT");
            }
        }

        public static async Task ValidateLoggerResponseAsync(GRP_LOGGER logger, IcosDbContext db, Response response)
        {
            errorCode = await InstrumentsValidation.SensorInGrpInst(logger.LOGGER_MODEL, logger.LOGGER_SN, logger.LOGGER_DATE, logger.SiteId, db);
            if (errorCode > 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_MODEL", "$V0$", logger.LOGGER_MODEL, "$V1$", logger.LOGGER_SN);
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(logger.LOGGER_MODEL, "LOGGER_MODEL", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_MODEL", "$V0$", "LOGGER_MODEL", "$GRP$", "GRP_LOGGER");
            }
            else
            {
                errorCode = await GeneralValidation.ItemInBadmListAsync(logger.LOGGER_MODEL, (int)Globals.CvIndexes.LOGGER_MODEL, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_MODEL", "$V0$", logger.LOGGER_MODEL, "$V1$", "LOGGER_MODEL", "$GRP$", "GRP_LOGGER");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(logger.LOGGER_SN, "LOGGER_SN", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_SN", "$V0$", "LOGGER_SN", "$GRP$", "GRP_LOGGER");
            }
            else
            {
                errorCode = InstrumentsValidation.SerialNumberCheck(logger.LOGGER_MODEL, logger.LOGGER_SN);
                if (errorCode != 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_SN", "$V0$", "LOGGER_SN", "$GRP$", "GRP_LOGGER");
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<int>(logger.LOGGER_ID, "LOGGER_ID", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_ID", "$V0$", "LOGGER_ID", "$GRP$", "GRP_LOGGER");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(logger.LOGGER_DATE, "LOGGER_DATE", "GRP_LOGGER");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_DATE", "$V0$", "LOGGER_DATE", "$GRP$", "GRP_LOGGER");
            }
            errorCode = DatesValidator.IsoDateCheck(logger.LOGGER_DATE, "LOGGER_DATE");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "LOGGER_DATE", "$V0$", "LOGGER_DATE", "$V1$", logger.LOGGER_DATE);
            }
            //return response;
        }

        private static int CheckFileFormat(string fileFormat, string fileExt, int siteId)
        {
            int res = 0;
            if (fileFormat == null && fileExt == null)
            {
                return 0;
            }
            if ((fileFormat != null && fileExt == null) || (fileFormat == null && fileExt != null))
            {
                return (int)Globals.ErrorValidationCodes.FILE_FORMAT_FILEXT;
            }
            switch (fileFormat.ToLower())
            {
                case "binary":
                    if (String.Compare(fileExt.ToLower(), ".csv", true) == 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.CSV_NOT_ALLOWED_FOR_BINARY;
                    }
                    break;
                case "ascii":
                    if (String.Compare(fileExt.ToLower(), ".bin", true) == 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.BIN_NOT_ALLOWED_FOR_ASCII;
                    }
                    break;
            }
            return res;
        }

        private static int CheckHeadConstraints(string fileFormat, int? fileHeadNum, int? fileHeadVars, int? fileHeadType)
        {
            int res = 0;
            if (fileFormat == null)
            {
                if (fileHeadNum == null && fileHeadVars == null && fileHeadType == null)
                {
                    return 0;
                }
                else
                {
                   //return 555;
                }
            }
            if (String.Compare(fileFormat.ToLower(), "binary", true) == 0)
            {
                if (fileHeadNum == null && fileHeadVars == null && fileHeadType == null)
                {
                    return 0;
                }
                if (fileHeadType == null && fileHeadNum != null && fileHeadVars != null)
                {
                    if (fileHeadNum != 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_TYPE_MISSING_BINARY;
                    }
                    else if (fileHeadType == null && fileHeadNum == null && fileHeadVars != null)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_NUM_TYPE_MISSING_BINARY;
                    }
                    else if (fileHeadType != null && fileHeadNum == null && fileHeadVars == null)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_NUM_VARS_MISSING_BINARY;
                    }
                    else
                    {

                        if (fileHeadVars > fileHeadNum)
                        {
                            res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_GT_FILE_HEAD_NUM;
                        }
                        if (fileHeadType > fileHeadNum)
                        {
                            res = (int)Globals.ErrorValidationCodes.FILE_HEAD_TYPE_GT_FILE_HEAD_NUM;
                        }
                        if (fileHeadVars == fileHeadType && fileHeadNum != 0)
                        {
                            res = (int)Globals.ErrorValidationCodes.FILE_HEAD_TYPE_GT_FILE_HEAD_VARS;
                        }
                    }
                }
            }
            else //ASCII
            {
                if (fileHeadNum != null && fileHeadVars == null)
                {
                    if (fileHeadNum != 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_MISSING;
                    }

                }
                else if (fileHeadNum != null && fileHeadVars == null)
                {
                    //OK....
                }
                else if (fileHeadNum == null && fileHeadVars != null)
                {
                    res = (int)Globals.ErrorValidationCodes.FILE_HEAD_NUM_MISSING;
                }
                else
                {


                    if (fileHeadVars != null && fileHeadVars > fileHeadNum)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_VARS_GT_FILE_HEAD_NUM;
                    }
                    if (fileHeadVars == fileHeadType && fileHeadNum != 0)
                    {
                        res = (int)Globals.ErrorValidationCodes.FILE_HEAD_TYPE_EQ_FILE_HEAD_VARS;
                    }
                }
            }
            return res;
        }

    }
}
