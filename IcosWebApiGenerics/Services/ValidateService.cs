using IcosWebApi.Models;
using IcosWebApi.Models.Obj;
using IcosWebApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApi.Services
{
    public class ValidateService : IValidateService
    {
        public List<int> ErrorCodes { get; set; }
        private readonly IErrorLogger _errorLogger;

        public ValidateService(IErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
        }
        public int ValidateModel(BaseClass model)
        {
            int res = 0;
            switch (model.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION: //grp location
                    res = ValidateLocationGroup(model);
                    break;
                case (int)Globals.Groups.GRP_UTC_OFFSET:
                    res = ValidateUtcOffsetGroup(model);
                    break;
            }
            return res;
        }

        public List<int> ValidateModelExt(BaseClass model)
        {
            //int res = 0;
            switch (model.GroupId)
            {
                case (int)Globals.Groups.GRP_LOCATION: //grp location
                    ValidateLocationGroupExt(model);
                    break;
                case (int)Globals.Groups.GRP_UTC_OFFSET:
                    ValidateUtcOffsetGroupExt(model);
                    break;
            }
            return ErrorCodes;
        }
        public Response Validate(BaseClass model)
        {
            throw new NotImplementedException();
        }

        private int ValidateLocationGroup(BaseClass t)
        {
            GRP_LOCATION location = t as GRP_LOCATION;
            if (String.IsNullOrEmpty(location.LOCATION_DATE)) return 1;
            if (location.LOCATION_LAT > 1000) return 2;
            if (location.LOCATION_LONG > 1000) return 3;
            return 0;
        }

        private void ValidateLocationGroupExt(BaseClass t)
        {
            GRP_LOCATION location = t as GRP_LOCATION;

            if (String.IsNullOrEmpty(location.LOCATION_DATE))
            {
                ErrorCodes.Add( 1);
            }
            if (location.LOCATION_LAT > 1000) ErrorCodes.Add(2);
            if (location.LOCATION_LONG > 1000) ErrorCodes.Add(3);
        }
        /// <summary>
        /// Variables to check:
        /// UTC_OFFSET (must be a valid float number, possibly in a range)
        /// UTC_OFFSET_DATE_START: must be a valid isodate
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private int ValidateUtcOffsetGroup(BaseClass t)
        {
            GRP_UTC_OFFSET utcOffset = t as GRP_UTC_OFFSET;
            var _offset = utcOffset.UTC_OFFSET;
            if (Decimal.Compare(_offset, Globals.MIN_UTC_OFFSET_VAL) < 0 || Decimal.Compare(_offset, Globals.MAX_UTC_OFFSET_VAL) > 0)
            {
                return 1;
            }
            if (!Globals.ValidateIsoDate(utcOffset.UTC_OFFSET_DATE_START))
            {
                return 300;
            }
            return 0;
        }

        private void ValidateUtcOffsetGroupExt(BaseClass t)
        {
            GRP_UTC_OFFSET utcOffset = t as GRP_UTC_OFFSET;
            var _offset = utcOffset.UTC_OFFSET;
            if (Decimal.Compare(_offset, Globals.MIN_UTC_OFFSET_VAL) < 0 || Decimal.Compare(_offset, Globals.MAX_UTC_OFFSET_VAL) > 0)
            {
                ErrorCodes.Add(10);
            }
            if (!Globals.ValidateIsoDate(utcOffset.UTC_OFFSET_DATE_START))
            {
                ErrorCodes.Add(11);
            }
        }
    }
}
