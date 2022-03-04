using IcosWebApi.Models.Obj;

namespace IcosWebApi.Models
{
    public interface IMapper
    {
        BaseClass MapToObject(VarForGroup vg);

        int MapResult { get; set; }
    }
}
