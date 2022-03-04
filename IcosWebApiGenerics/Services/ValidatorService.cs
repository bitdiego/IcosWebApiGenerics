using IcosWebApi.Models;
using IcosWebApi.Utils;
using System;
using IcosWebApi.Services.ValidationFunctions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IcosWebApi.Data;
using IcosWebApi.Models.Obj;

namespace IcosWebApi.Services
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

        public Response Validate(T t)
        {
            //Response res = GrpValidator.GetResponse();
            switch (t.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION: //grp location
                    GRP_LOCATION location = t as GRP_LOCATION;
                    response = GrpValidator.ValidateLocationResponse(location);
                    break;
                case (int)Globals.Groups.GRP_UTC_OFFSET:
                    GRP_UTC_OFFSET utc = t as GRP_UTC_OFFSET;
                    response = GrpValidator.ValidateUtcResponse(utc);
                    break;
                case (int)Globals.Groups.GRP_LAND_OWNERSHIP:
                    GRP_LAND_OWNERSHIP land = t as GRP_LAND_OWNERSHIP;
                    response = GrpValidator.ValidateLandOwnerResponse(land, _context);
                    break;
                case (int)Globals.Groups.GRP_TOWER:
                    GRP_TOWER tower = t as GRP_TOWER;
                    response = GrpValidator.ValidateTowerResponse(tower, _context);
                    break;
                case (int)Globals.Groups.GRP_CLIM_AVG:
                    GRP_CLIM_AVG climAvg = t as GRP_CLIM_AVG;
                    response = GrpValidator.ValidateClimateAvgResponse(climAvg);
                    break;
                case (int)Globals.Groups.GRP_DM:
                    GRP_DM distManager = t as GRP_DM;
                    response = GrpValidator.ValidateDistManResponse(distManager, _context);
                    break;
            }
            return response;
        }

        private int ValidateLocationGroup(T t)
        {
            GRP_LOCATION location = t as GRP_LOCATION;
            if (String.IsNullOrEmpty(location.LOCATION_DATE)) return 1;
            if (location.LOCATION_LAT > 1000) return 2;
            if (location.LOCATION_LONG > 1000) return 3;
            return 0;
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
            if (!Globals.ValidateIsoDate(utcOffset.UTC_OFFSET_DATE_START))
            {
                return 300;
            }
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
