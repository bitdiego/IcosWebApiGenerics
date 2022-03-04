using IcosWebApi.Models.Obj;
using System.Threading.Tasks;


namespace IcosWebApi.Services
{
    public interface ISaveGroupService
    {
        Task ItemInDbAsync(BaseClass t, int siteId);
        Task<bool> UpdateItemAsync(int? id, int siteId, int userId, BaseClass t);
        Task<bool> SetItemInvalidAsync(int siteId, int userId, BaseClass t);
        Task<bool> SaveItemAsync(BaseClass t, int insertUserId, int siteId);
    }
}
