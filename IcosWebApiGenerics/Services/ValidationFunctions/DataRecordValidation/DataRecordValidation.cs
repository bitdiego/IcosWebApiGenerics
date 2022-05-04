using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.DataRecordValidation
{
    public class DataRecordValidation
    {
        private static Response response = null;
        private static string Err = "";
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        public static Response GetResponse()
        {
            if (response == null)
            {
                response = new Response();
            }
            return response;
        }

        public static Task<Response> ValidateFileResponseAsync(GRP_FILE file, IcosDbContext context)
        {
            throw new NotImplementedException();
        }

        public static async Task<Response> ValidateLoggerResponseAsync(GRP_LOGGER logger, IcosDbContext db)
        {
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



            return response;
        }

    }
}
