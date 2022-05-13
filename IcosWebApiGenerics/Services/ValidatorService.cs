using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Utils;
using System;
using IcosWebApiGenerics.Services.ValidationFunctions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Services.ValidationFunctions.StationDescValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.GreenAreaIndexValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.ECValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.DataRecordValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.SamplingValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.STALValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.MeteoValidation;
using IcosWebApiGenerics.Services.ValidationFunctions.StorageValidation;

namespace IcosWebApiGenerics.Services
{
    public class ValidatorService<T> : IValidatorService<T> where T : BaseClass, new()
    {
        private readonly IcosDbContext _context;
        private Response response;
        public ValidatorService(IcosDbContext context)
        {
            this._context = context;
            response= GrpValidator.GetResponse();
            response.Code = 0;
            //response.Error = "";
            response.Messages.Clear();
        }
        /* public int ValidateModel(T t)
         {
             int res = 0;
             switch (t.GroupId)
             {
                 case (int)Globals.Groups.GRP_LOCATION: //grp location
                     res = ValidateLocationGroup(t);
                     break;
                 case (int)Globals.Groups.GRP_UTC_OFFSET:
                     res = ValidateUtcOffsetGroup(t);
                     break;
             }
             return res;
         }*/

        public Response ItemInBadmList(string value, int cvIndex)
        {
            string bmList = _context.BADMList.Where(item => item.cv_index == cvIndex).Select(x => x.vocabulary).FirstOrDefault();
            var res = _context.BADMList.Where(item => item.cv_index == cvIndex)
                                       .Any(item => (String.Compare(item.shortname, value, true) == 0));
            if (!res)
            {

            }

            return response;
        }

        public async Task<Response> Validate(T t)
        {
            switch (t.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION:
                    GRP_LOCATION location = t as GRP_LOCATION;
                    StationDescValidation.ValidateLocationResponse(location, response);
                    break;
                case (int)Globals.Groups.GRP_UTC_OFFSET:
                    GRP_UTC_OFFSET utc = t as GRP_UTC_OFFSET;
                    StationDescValidation.ValidateUtcResponse(utc, response);
                    break;
                case (int)Globals.Groups.GRP_LAND_OWNERSHIP:
                    GRP_LAND_OWNERSHIP land = t as GRP_LAND_OWNERSHIP;
                    await StationDescValidation.ValidateLandOwnerResponseAsync(land, _context, response);
                    break;
                case (int)Globals.Groups.GRP_TOWER:
                    GRP_TOWER tower = t as GRP_TOWER;
                    await StationDescValidation.ValidateTowerResponseAsync(tower, _context, response);
                    break;
                case (int)Globals.Groups.GRP_CLIM_AVG:
                    GRP_CLIM_AVG climAvg = t as GRP_CLIM_AVG;
                    StationDescValidation.ValidateClimateAvgResponse(climAvg, response);
                    break;
                case (int)Globals.Groups.GRP_DM:
                    GRP_DM distManager = t as GRP_DM;
                    StationDescValidation.ValidateDistManResponse(distManager, _context, response);
                    break;
                case (int)Globals.Groups.GRP_PLOT:
                    GRP_PLOT samplingScheme = t as GRP_PLOT;
                    response = await SamplingValidation.ValidateSamplingSchemeResponseAsync(samplingScheme, _context);
                    break;
                case (int)Globals.Groups.GRP_FLSM:
                    GRP_FLSM flsm = t as GRP_FLSM;
                    response = await SamplingValidation.ValidateFlsmResponseAsync(flsm, _context);
                    break;
                case (int)Globals.Groups.GRP_SOSM:
                    GRP_SOSM sosm = t as GRP_SOSM;
                    response = SamplingValidation.ValidateSosmResponse(sosm, _context);
                    break;
                case (int)Globals.Groups.GRP_DHP:
                    GRP_DHP dhp = t as GRP_DHP;
                    response = await GrpValidator.ValidateDhpResponseAsync(dhp, _context);
                    break;
                case (int)Globals.Groups.GRP_GAI:
                    GRP_GAI gai = t as GRP_GAI;
                    response = await GreenAreaIndexValidation.ValidateGaiResponseAsync(gai, _context);
                    break;
                case (int)Globals.Groups.GRP_CEPT:
                    GRP_CEPT cept = t as GRP_CEPT;
                    response = GreenAreaIndexValidation.ValidateCeptResponse(cept);
                    break;
                case (int)Globals.Groups.GRP_BULKH:
                    GRP_BULKH bulkh = t as GRP_BULKH;
                    response = await GreenAreaIndexValidation.ValidateBulkhResponseAsync(bulkh, _context);
                    break;
                case (int)Globals.Groups.GRP_SPPS:
                    GRP_SPPS spps = t as GRP_SPPS;
                    response = await STALValidation.ValidateSppsResponseAsync(spps, _context);
                    break;
                case (int)Globals.Groups.GRP_TREE:
                    GRP_TREE dSnow = t as GRP_TREE;
                    response = STALValidation.ValidateTreeResponse(dSnow, _context);
                    break;
                case (int)Globals.Groups.GRP_AGB:
                    GRP_AGB agb = t as GRP_AGB;
                    response = STALValidation.ValidateAgbResponse(agb, _context);
                    break;
                case (int)Globals.Groups.GRP_LITTERPNT:
                    GRP_LITTERPNT litterPnt = t as GRP_LITTERPNT;
                    response = STALValidation.ValidateLitterPntResponse(litterPnt, _context);
                    break;
                case (int)Globals.Groups.GRP_ALLOM:
                    GRP_ALLOM allom = t as GRP_ALLOM;
                    response = STALValidation.ValidateAllomResponse(allom, _context);
                    break;
                case (int)Globals.Groups.GRP_WTDPNT:
                    GRP_WTDPNT wtdPnt = t as GRP_WTDPNT;
                    response = GrpValidator.ValidateWtdPntResponse(wtdPnt, _context);
                    break;
                case (int)Globals.Groups.GRP_D_SNOW:
                    GRP_D_SNOW d_Snow = t as GRP_D_SNOW;
                    response = GrpValidator.ValidateDSnowResponse(d_Snow, _context);
                    break;
                case (int)Globals.Groups.GRP_INST:
                    GRP_INST inst = t as GRP_INST;
                    response = await GrpValidator.ValidateInstResponseAsync(inst, _context);
                    break;
                case (int)Globals.Groups.GRP_LOGGER:
                    GRP_LOGGER logger = t as GRP_LOGGER;
                    response = await DataRecordValidation.ValidateLoggerResponseAsync(logger, _context);
                    break;
                case (int)Globals.Groups.GRP_FILE:
                    GRP_FILE _file = t as GRP_FILE;
                    response = await DataRecordValidation.ValidateFileResponseAsync(_file, _context);
                    break;
                case (int)Globals.Groups.GRP_EC:
                    GRP_EC ecInst = t as GRP_EC;
                    response = await ECValidation.ValidateEcResponseAsync(ecInst, _context);
                    break;
                case (int)Globals.Groups.GRP_ECSYS:
                    GRP_ECSYS ecSys = t as GRP_ECSYS;
                    response = await ECValidation.ValidateEcSysResponseAsync(ecSys, _context);
                    break;
                case (int)Globals.Groups.GRP_ECWEXCL:
                    GRP_ECWEXCL ecWexcl = t as GRP_ECWEXCL;
                    response = await ECValidation.ValidateEcWexclResponseAsync(ecWexcl, _context);
                    break;
                case (int)Globals.Groups.GRP_BM:
                    GRP_BM bmModel = t as GRP_BM;
                    response = await MeteoValidation.ValidateBmResponseAsync(bmModel, _context);
                    break;
                case (int)Globals.Groups.GRP_STO:
                    GRP_STO stModel = t as GRP_STO;
                    response = await StorageValidation.ValidateStorageResponseAsync(stModel, _context);
                    break;
            }
            return response;
        }

        /// <summary>
        /// Variables to check:
        /// UTC_OFFSET (must be a valid float number, possibly in a range)
        /// UTC_OFFSET_DATE_START: must be a valid isodate
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private int ValidateUtcOffsetGroup(T t)
        {
            GRP_UTC_OFFSET utcOffset = t as GRP_UTC_OFFSET;
            var _offset = utcOffset.UTC_OFFSET;
            if(Decimal.Compare(_offset, Globals.MIN_UTC_OFFSET_VAL) <0 || Decimal.Compare(_offset, Globals.MAX_UTC_OFFSET_VAL) > 0)
            {
                return 1;
            }
            /*if (!Globals.ValidateIsoDate(utcOffset.UTC_OFFSET_DATE_START))
            {
                return 300;
            }*/
            return 0;
        }

        /*private Response ValidateLocationResponse(T t)
        {
            Response r = new Response();
            GRP_LOCATION location = t as GRP_LOCATION;
            if (String.IsNullOrEmpty(location.LOCATION_DATE))
            {
                r.Code |= 1;
                r.Message += "<br />Error: LOCATION_DATE is mandatory";
            }
            if (location.LOCATION_LAT > 1000)
            {
                r.Code |= 2;
                r.Message += "<br />Error: LOCATION_LAT must be between -180 and 180";
            }
            if (location.LOCATION_LONG > 1000)
            {
                r.Code |= 3;
                r.Message += "<br />Error: LOCATION_LONG must be between -360 and 360";
            }
            return r;
        }*/
    }
}
