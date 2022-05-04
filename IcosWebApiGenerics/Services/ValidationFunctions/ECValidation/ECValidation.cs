﻿using IcosWebApiGenerics.Data;
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

        public static async Task<Response> ValidateEcResponseAsync(GRP_EC ecInst, IcosDbContext db)
        {

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

            //ec sensor present in GRP_INST
            if (!String.IsNullOrEmpty(ecInst.EC_DATE) || !String.IsNullOrEmpty(ecInst.EC_DATE_START))
            {
                string dateToCheck = String.IsNullOrEmpty(ecInst.EC_DATE) ? ecInst.EC_DATE_START : ecInst.EC_DATE;
                errorCode = await InstrumentsValidation.SensorInGrpInst(ecInst.EC_MODEL, ecInst.EC_SN, dateToCheck, ecInst.SiteId, db);
                if (errorCode > 0)
                {
                    response.Code += errorCode;
                    response.FormatError(ErrorCodes.GrpEcErrors[errorCode], "EC_MODEL");
                }
            }

            //check dates constraints
            errorCode = DatesValidator.IsoDateCompare(ecInst.EC_DATE, ecInst.EC_DATE_START, ecInst.EC_DATE_END);
            if (errorCode != 0)
            {
                response.Code += errorCode;
                response.FormatError(ErrorCodes.GeneralErrors[errorCode], "EC_DATE", "$V0$", "EC_DATE", "$V1$", "EC_DATE_START", "$V2$", "EC_DATE_END", "$GRP$", "GRP_EC");
            }

            return response;
        }

        public static Task<Response> ValidateEcSysResponseAsync(GRP_ECSYS ecSys, IcosDbContext context)
        {
            throw new NotImplementedException();
        }

        public static Task<Response> ValidateEcWexclResponseAsync(GRP_ECWEXCL ecWexcl, IcosDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}