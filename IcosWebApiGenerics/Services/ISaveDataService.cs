﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Services
{
    public interface ISaveDataService<T>
    {
        Task<T> ItemInDbAsync(T t, int siteId);
        Task<bool> ItemInDbAsync(T t);
        Task<bool> UpdateItemAsync(int? id, int siteId, int userId, T t);
        Task<bool> SetItemInvalidAsync(int siteId, int userId, T t);
        Task<bool> SetItemInvalidAsync(T t);
        Task<bool> SaveItemAsync(T t, int insertUserId, int siteId);
    }
}
