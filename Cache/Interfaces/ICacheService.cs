using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cache
{
    public interface ICacheService
    {
        bool CacheDataIsValid(ICacheSetup cacheSetup);
        void ClearCache();
        List<ICacheSetup> GetAllSetups();
        object GetItem(string cacheKey);
        T GetItem<T>(string cacheKey) where T : class;
        List<KeyValuePair<string, object>> GetMemoryUsageList();
        T GetOrSet<T>(Func<T> getItemCallback, ICacheSetup cacheSetup) where T : class;
        Task<T> GetOrSetAsync<T>(Func<Task<T>> getItemCallback, ICacheSetup cacheSetup) where T : class;
        ICacheSetup GetSetup<T>(string key, int cacheMinutes);
        ICacheSetup GetSetup<T>(string key, int cacheMinutes, string objectID);
        ICacheSetup GetSetup<T>(string key, int cacheMinutes, List<string> dependencies);
        ICacheSetup GetSetup<T>(string key, int cacheMinutes, string objectID, List<string> dependencies = null);
        ICacheSetup GetSetup<T>(string source, int cacheMinutes, int pageNumber, int pageSize, string sort, List<string> dependencies = null);
        ICacheSetup GetSetup<T>(string source, int cacheMinutes, string objectID, int pageNumber, int pageSize, string sort, List<string> dependencies = null);
        void Invalidate(ICacheSetup cacheSetup);
        void TouchKey(string key);
    }
}