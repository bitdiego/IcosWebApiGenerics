using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services.ValidationFunctions.StorageValidation
{
    public class StorageValidation
    {
        private static int errorCode = 0;
        public static async Task<Response> ValidateStorageResponseAsync(GRP_STO stoModel, IcosDbContext db)
        {
            ResponseWrapper.SetResponse();
            errorCode = GeneralValidation.MissingMandatoryData<string>(stoModel.STO_TYPE, "STO_TYPE", "GRP_STO");
            if (errorCode != 0)
            {
                ResponseWrapper.WrapErrorCodeAndMessages(errorCode, "STO_TYPE", "$V0$", "STO_TYPE", "$GRP$", "GRP_STO");
            }

            if (!String.IsNullOrEmpty(stoModel.STO_DATE) || !String.IsNullOrEmpty(stoModel.STO_DATE_START))
            {
                string dateToCheck = String.IsNullOrEmpty(stoModel.STO_DATE) ? stoModel.STO_DATE_START : stoModel.STO_DATE;
                if(!String.IsNullOrEmpty(stoModel.STO_GA_MODEL) && !String.IsNullOrEmpty(stoModel.STO_GA_SN))
                {
                    errorCode = await InstrumentsValidation.SensorInGrpInst(stoModel.STO_GA_MODEL, stoModel.STO_GA_SN, dateToCheck, stoModel.SiteId, db);
                    if (errorCode > 0)
                    {
                        ResponseWrapper.WrapErrorCodeAndMessages(errorCode, "STO_MODEL");
                    }
                }
            }
            return ResponseWrapper.GetResponse();
        }
    }
}
