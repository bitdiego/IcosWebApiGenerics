using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.ECValidation
{
    public class ECValidation
    {
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        public static async Task ValidateEcResponseAsync(GRP_EC ecInst, IcosDbContext db, Response response)
        {
            //ec sensor present in GRP_INST
            if (!String.IsNullOrEmpty(ecInst.EC_MODEL) || !String.IsNullOrEmpty(ecInst.EC_SN))
            {
                string dateToCheck = String.IsNullOrEmpty(ecInst.EC_DATE) ? ecInst.EC_DATE_START : ecInst.EC_DATE;
                if (!String.IsNullOrEmpty(dateToCheck))
                {
                    errorCode = await InstrumentsValidation.SensorInGrpInst(ecInst.EC_MODEL, ecInst.EC_SN, dateToCheck, ecInst.SiteId, db);
                    if (errorCode > 0)
                    {
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "EC_MODEL");
                    }
                }
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(ecInst.EC_MODEL, "EC_MODEL", "GRP_EC");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_MODEL", "$V0$", "EC_MODEL", "$GRP$", "GRP_EC");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(ecInst.EC_SN, "EC_SN", "GRP_EC");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_SN", "$V0$", "EC_SN", "$GRP$", "GRP_EC");
            }

            errorCode = GeneralValidation.MissingMandatoryData<string>(ecInst.EC_TYPE, "EC_TYPE", "GRP_EC");
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_TYPE", "$V0$", "EC_TYPE", "$GRP$", "GRP_EC");
            }

            //check dates constraints
            /*errorCode = DatesValidator.IsoDateCompare(ecInst.EC_DATE, ecInst.EC_DATE_START, ecInst.EC_DATE_END);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_DATE", "$V0$", "EC_DATE", "$V1$", "EC_DATE_START", "$V2$", "EC_DATE_END", "$GRP$", "GRP_EC");
            }
            */
            errorCode = await InstrumentsValidation.LastExpectedOpByDateAsync(ecInst, db);
        }

        public static async Task<Response> ValidateEcSysResponseAsync(GRP_ECSYS ecSys, IcosDbContext context, Response response)
        {
            //1. check if GA e SA are in Inst group
            if (!String.IsNullOrEmpty(ecSys.ECSYS_GA_MODEL) || !String.IsNullOrEmpty(ecSys.ECSYS_GA_SN))
            {
                string dateToCheck = ecSys.ECSYS_DATE;
                if (!String.IsNullOrEmpty(dateToCheck))
                {
                    errorCode = await InstrumentsValidation.SensorInGrpInst(ecSys.ECSYS_GA_MODEL, ecSys.ECSYS_GA_SN, dateToCheck, ecSys.SiteId, context);
                    if (errorCode > 0)
                    {
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "ECSYS_GA_MODEL");
                    }
                    else
                    {
                        //2. check if they are correctly installed in GRP_EC
                        errorCode = await InstrumentsValidation.EcSysInstalled(ecSys.ECSYS_GA_MODEL, ecSys.ECSYS_GA_SN, ecSys.ECSYS_DATE, ecSys.SiteId, context);
                        if (errorCode > 0)
                        {
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "ECSYS_GA_MODEL");
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(ecSys.ECSYS_SA_MODEL) || !String.IsNullOrEmpty(ecSys.ECSYS_SA_SN))
            {
                string dateToCheck = ecSys.ECSYS_DATE;
                if (!String.IsNullOrEmpty(dateToCheck))
                {
                    errorCode = await InstrumentsValidation.SensorInGrpInst(ecSys.ECSYS_SA_MODEL, ecSys.ECSYS_SA_SN, dateToCheck, ecSys.SiteId, context);
                    if (errorCode > 0)
                    {
                        response.Code += errorCode;
                        response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "ECSYS_SA_MODEL");
                    }
                    else
                    {
                        //2. check if they are correctly installed in GRP_EC
                        errorCode = await InstrumentsValidation.EcSysInstalled(ecSys.ECSYS_SA_MODEL, ecSys.ECSYS_SA_SN, ecSys.ECSYS_DATE, ecSys.SiteId, context);
                        if (errorCode > 0)
                        {
                            response.Code += errorCode;
                            response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "ECSYS_SA_MODEL");
                        }
                    }
                }
            }
            

            //3. if GA_PUMP, check constraints on ECSYS_SEPs
            //4. all ECSYS_SEPs are mandatory
            throw new NotImplementedException();
        }

        public static Task<Response> ValidateEcWexclResponseAsync(GRP_ECWEXCL ecWexcl, IcosDbContext context, Response response)
        {
            //All variables (except _COMMENT) are mandatory
            throw new NotImplementedException();
        }
    }
}
