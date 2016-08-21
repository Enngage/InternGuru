using System;
using System.Collections.Generic;

namespace Cache
{
    internal abstract class CacheSetupAbstract : ICacheSetup
    {

        #region Variables

        protected int cacheMinutes;
        protected string cacheKey;
        protected List<String> dependencies;
        protected DateTime? updated;

        #endregion

        #region Interface methods

        public DateTime? GetUpdated()
        {
            return this.updated;
        }

        public void SetUpdated(DateTime time)
        {
            this.updated = time;
        }
        public int GetCacheMinutes()
        {
            return this.cacheMinutes;
        }
        public void SetCacheMinutes(int minutes)
        {
            this.cacheMinutes = minutes;
        }
        public string GetCacheKey()
        {
            return this.cacheKey;
        }
        public void SetCacheKey(string cacheKey)
        {
            this.cacheKey = cacheKey;
        }
        public String[] GetDependencies()
        {
            var dependencyArray = new String[this.dependencies.Count];
            for (int i = 0; i < this.dependencies.Count; i++)
            {
                dependencyArray[i] = this.dependencies[i];
            }
            return dependencyArray;
        }

        #endregion
    }
}
