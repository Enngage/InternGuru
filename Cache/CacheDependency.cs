using System;
using System.Collections.Generic;
using System.Linq;

namespace Cache
{
    internal static class CacheDependency
    {

        #region Properties

        /// <summary>
        /// Dictionary where all cache items are stored
        /// </summary>
        private static readonly Dictionary<string, ICacheSetup> CacheSetupList = new Dictionary<string, ICacheSetup>();

        #endregion

        #region Public methods

        /// <summary>
        /// Deletes all cacke dependency keys
        /// </summary>
        public static void ClearAll()
        {
            CacheSetupList.Clear();  
        }

        /// <summary>
        /// Touches given key (removes cacheSetup from list if the key exists in its dependencies)
        /// </summary>
        /// <param name="key">Key</param>
        public static void TouchKey(string key)
        {
            foreach (var dict in CacheSetupList.ToList())
            {
                if (CacheSetupDependsOnKey(key, dict.Value))
                {
                    // remove cache setup from cache
                    CacheSetupList.Remove(dict.Key);
                }
            }
        }

        /// <summary>
        /// Checks if  given CacheSetup depends on given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="cacheSetup">CacheSetup</param>
        /// <returns>True if cache setup depends on given key or false</returns>
        private static bool CacheSetupDependsOnKey(string key, ICacheSetup cacheSetup)
        {
            return cacheSetup.Dependencies.Contains(key);
        }

        /// <summary>
        /// Adds cache setup to list (if it already isnt' there)
        /// </summary>
        /// <param name="cacheSetup">cacheSetup</param>
        public static void AddCacheSetup(ICacheSetup cacheSetup)
        {

            if (!CacheSetupList.ContainsKey(cacheSetup.CacheKey))
            {
                cacheSetup.SetUpdated(DateTime.Now);
                CacheSetupList.Add(cacheSetup.CacheKey, cacheSetup);
            }
            else
            {
                CacheSetupList[cacheSetup.CacheKey].SetUpdated(DateTime.Now);
            }
        }

        /// <summary>
        /// Returns true if data should be returned from cache
        /// </summary>
        /// <param name="cacheSetup">CacheSetup</param>
        /// <returns>True if data should be retrieved from cache, false otherwise</returns>
        public static bool GetDataFromCache(ICacheSetup cacheSetup)
        {
            if (CacheSetupList.ContainsKey(cacheSetup.CacheKey))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all dependencies
        /// </summary>
        /// <returns>Dependencies</returns>
        public static List<ICacheSetup> GetAll()
        {
            var list = new List<ICacheSetup>();
            foreach (var cacheSetup in CacheSetupList)
            {
                list.Add(cacheSetup.Value);
            }
            return list;
        }

        #endregion
    }
}
