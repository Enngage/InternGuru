
using System;

namespace Cache
{
    public interface ICacheSetup
    {
        DateTime? GetUpdated();
        void SetUpdated(DateTime time);
        int GetCacheMinutes();
        void SetCacheMinutes(int minutes);
        string GetCacheKey();
        void SetCacheKey(string cacheKey);
        String[] GetDependencies();
    }
}
