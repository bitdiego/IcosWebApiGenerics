using IcosWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApi
{
    public interface IValidatorService<T>
    {
        /*Task<T> GetItemAsync();
        Task<T> GetItemValueAsync(int siteId);
        
        Task<T> GetItemValueByIdAsync(int siteId, int? id);
        Task<IEnumerable<T>> GetItemValuesAsync(int siteId);
        
        Task<bool> DeleteItemAsync(int? id, int siteId, int userId);
        
        Task<IQueryable<string>> GetBadmListDataAsync(int dataId);
        IQueryable<string> GetBadmListData(int dataId);*/
        //int ValidateModel(T t);
        Response Validate(T t);
        Response ItemInBadmList(string value, int cvIndex);

    }
}
