
using System;

namespace Cache
{
    public interface ICacheSetup
    {
        DateTime? GetUpdated();
        void SetUpdated(DateTime time);
        int GetCacheMinutes();
        string GetCacheKey();
        String[] GetDependencies();
    }
}
