﻿using IcosWebApiGenerics.Models.BADM;
using System.Threading.Tasks;


namespace IcosWebApiGenerics.Services
{
    public interface ISaveGroupService
    {
        Task ItemInDbAsync(BaseClass t, int siteId);
        Task<bool> UpdateItemAsync(int? id, int siteId, int userId, BaseClass t);
        Task<bool> SetItemInvalidAsync(int siteId, int userId, BaseClass t);
        Task<bool> SaveItemAsync(BaseClass t, int insertUserId, int siteId);
    }
}
