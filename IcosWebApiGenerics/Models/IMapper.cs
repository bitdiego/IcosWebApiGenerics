using IcosWebApiGenerics.Models.BADM;

namespace IcosWebApiGenerics.Models
{
    public interface IMapper
    {
        BaseClass MapToObject(VarForGroup vg);

        int MapResult { get; set; }
    }
}
