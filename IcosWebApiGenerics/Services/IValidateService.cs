using IcosWebApi.Models;
using IcosWebApi.Models.Obj;
using System.Collections.Generic;

namespace IcosWebApi.Services
{
    public interface IValidateService
    {
        int ValidateModel(BaseClass model);
        Response Validate(BaseClass model);

        ///////////////
        List<int> ErrorCodes { get; set; }
    }
}
