using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using System.Collections.Generic;

namespace IcosWebApiGenerics.Services
{
    public interface IValidateService
    {
        int ValidateModel(BaseClass model);
        Response Validate(BaseClass model);

        ///////////////
        List<int> ErrorCodes { get; set; }
    }
}
