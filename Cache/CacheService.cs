using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Cache
{
    // Class providing cache methods: see http://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application
    // Usage : cacheProvider.GetOrSet("cache key", (delegate method if cache is empty));
    // Example: var products=cacheService.GetOrSet("catalog.products", ()=>productRepository.GetAll())
    public class CacheService : ICacheService
    {

        #region Variables

        private static bool forceDisableCaching = false; // disables caching for all calls (use only for testing)
        private static MemoryCache memoryCache = new MemoryCache("memCache", null);

        #endregion

        #region Clear cache

        /// <summary>
        /// Clears cache
        /// </summary>
        public void ClearCache(){
            memoryCache.Dispose();
            CacheDependency.ClearAll();
            memoryCache = new MemoryCache("memCache", null);
        }

        #endregion

        #region Get cache setup

        public ICacheSetup GetSetup<T>(string key, int cacheMinutes)
        {
            return new CacheSetup<T>(key, cacheMinutes);
        }


        public ICacheSetup GetSetup<T>(string key, int cacheMinutes, string objectID)
        {
            return new CacheSetup<T>(key, cacheMinutes, objectID);
        }

        public ICacheSetup GetSetup<T>(string key, int cacheMinutes, List<String> dependencies)
        {
            return new CacheSetup<T>(key, cacheMinutes, dependencies);
        }

        public ICacheSetup GetSetup<T>(string key, int cacheMinutes, string objectID, List<String> dependencies = null)
        {
            return new CacheSetup<T>(key, cacheMinutes, objectID, dependencies);
        }

        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="source">Source of calling (ideally: "{controller}.{action}" </param>
        /// <param name="objectID">Further identification of object</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="sort">sort</param>
        public ICacheSetup GetSetup<T>(string source, int cacheMinutes, string objectID, int pageNumber, int pageSize, string sort, List<String> dependencies = null)
        {
            return new CacheSetup<T>(source, cacheMinutes, objectID, pageNumber, pageSize, sort, dependencies);
        }

        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="source">Source of calling (ideally: "{controller}.{action}" </param>
        /// <param name="objectID">Further identification of object</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="sort">sort</param>
        public ICacheSetup GetSetup<T>(string source, int cacheMinutes, int pageNumber, int pageSize, string sort, List<String> dependencies = null)
        {
            return new CacheSetup<T>(source, cacheMinutes, pageNumber, pageSize, sort, dependencies);
        }

        #endregion

        #region Async GetOrSet

        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="Func<T>">Delegated method</param>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>Result of the delegated method</returns>
        public async Task<T> GetOrSetAsync<T>(Func<Task<T>> getItemCallback, ICacheSetup cacheSetup) where T : class
        {
            T item = memoryCache.Get(cacheSetup.GetCacheKey()) as T;
            if (item == null)
            {
                var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.GetCacheMinutes()));
                item = await getItemCallback();

                var cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = cacheExpires
                };

                if (item == null)
                {
                    return null;
                }
                memoryCache.Add(cacheSetup.GetCacheKey(), item, cacheItemPolicy);
            }
            else
            {
                // do not get data from cache if CacheSetup is not present
                if (!CacheDependency.GetDataFromCache(cacheSetup))
                {
                    // remove item from cache
                    memoryCache.Remove(cacheSetup.GetCacheKey());

                    var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.GetCacheMinutes()));
                    item = await getItemCallback();

                    if (item == null)
                    {
                        return null;
                    }

                    var cacheItemPolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = cacheExpires
                    };

                    memoryCache.Add(cacheSetup.GetCacheKey(), item, cacheItemPolicy);
                }
            }

            // add dependency back to list
            CacheDependency.AddCacheSetup(cacheSetup);
            return item;
        }

        #endregion

        #region GetOrSet without resolving

        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="Func<T>">Delegated method</param>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>Result of the delegated method</returns>
        public T GetOrSet<T>(Func<T> getItemCallback, ICacheSetup cacheSetup) where T : class
        {
            T item = memoryCache.Get(cacheSetup.GetCacheKey()) as T;
            if (item == null)
            {
                var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.GetCacheMinutes()));
                item = getItemCallback();

                if (item == null)
                {
                    return null;
                }

                var cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = cacheExpires
                };

                memoryCache.Add(cacheSetup.GetCacheKey(), item, cacheItemPolicy);
            }
            else
            {
                // do not get data from cache if CacheSetup is not present
                if (!CacheDependency.GetDataFromCache(cacheSetup))
                {
                    // remove item from cache
                    memoryCache.Remove(cacheSetup.GetCacheKey());

                    var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.GetCacheMinutes()));
                    item = getItemCallback();

                    if (item == null)
                    {
                        return null;
                    }

                    var cacheItemPolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = cacheExpires
                    };

                    memoryCache.Add(cacheSetup.GetCacheKey(), item, cacheItemPolicy);
                }
            }

            // add dependency back to list
            CacheDependency.AddCacheSetup(cacheSetup);
            return item;
        }

        #endregion

        #region Get cache item

        /// <summary>
        /// Gets given item from cache
        /// 
        /// </summary>
        /// <param name="cacheKey">Key of the item</param>
        /// <returns>Data from cache</returns>
        public T GetItem<T>(string cacheKey) where T : class
        {
            var item = memoryCache.GetCacheItem(cacheKey);
            if (item != null)
            {
                return item.Value as T;
            }
            return null;
        }

        /// <summary>
        /// Gets given item from cache
        /// 
        /// </summary>
        /// <param name="cacheKey">Key of the item</param>
        /// <returns>Data from cache</returns>
        public object GetItem(string cacheKey)
        {
            var item = memoryCache.GetCacheItem(cacheKey);
            if (item != null)
            {
                return item.Value;
            }
            return null;
        }

        #endregion

        #region Public methods

        public List<KeyValuePair<String, object>> GetMemoryUsageList()
        {
            return memoryCache.ToList();
        }

        /// <summary>
        /// Returns true if values from given cacheSetup should be returned from cache or false if data should be re-loaded
        /// </summary>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>True if data should be returned from cache, false otherwise</returns>
        public bool CacheDataIsValid(ICacheSetup cacheSetup)
        {
            var item = memoryCache.Get(cacheSetup.GetCacheKey());
            if (item == null)
            {
                // data are not cached at all - always reload data
                return false;
            }
            else
            {
                // do not get data from cache if CacheSetup is not present
                if (!CacheDependency.GetDataFromCache(cacheSetup))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public List<ICacheSetup> GetAllSetups()
        {
            return CacheDependency.GetAll();
        }

        public void TouchKey(string key)
        {
            CacheDependency.TouchKey(key);
        }

        /// <summary>
        /// Invalides given CacheSetup so that next calls will be retrieved from DB rather then from cache
        /// </summary>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>True if data should be returned from cache, false otherwise</returns>
        public void Invalidate(ICacheSetup cacheSetup)
        {
            var item = memoryCache.Get(cacheSetup.GetCacheKey());
            if (item == null)
            {
                // nothing to invalidate
            }
            else
            {
                item = null;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets cache minutes based on default setting and forceDisableCaching property
        /// </summary>
        /// <param name="cacheMinutes">CacheMinutes</param>
        /// <returns>Minutes for how long the item should be cached</returns>
        private int GetCacheMinutes(int cacheMinutes)
        {
            if (forceDisableCaching)
            {
                return 0;
            }

            return cacheMinutes;
        }

        #endregion
    }
}
