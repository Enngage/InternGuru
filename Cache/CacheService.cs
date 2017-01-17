using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Cache.Models;

namespace Cache
{
    // Class providing cache methods: see http://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application
    // Usage : cacheProvider.GetOrSet("cache key", (delegate method if cache is empty));
    // Example: var products=cacheService.GetOrSet("catalog.products", ()=>productRepository.GetAll())
    public class CacheService : ICacheService
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="forceDisableCaching">Indicates if caching is disabled or enabled</param>
        /// <param name="defaultCacheMinutes">Default number of minutes to cache items</param>
        public CacheService(bool forceDisableCaching, int defaultCacheMinutes)
        {
            _forceDisableCaching = forceDisableCaching;
            _defaultCacheMinutes = defaultCacheMinutes;
        }

        #endregion

        #region Variables

        private static MemoryCache _memoryCache = new MemoryCache("memCache"); // cache name

        private readonly bool _forceDisableCaching; // disables caching for all calls (use only for testing)
        private readonly int _defaultCacheMinutes;

        #endregion

        #region Clear cache

        /// <summary>
        /// Clears cache
        /// </summary>
        public void ClearCache(){
            _memoryCache.Dispose();
            CacheDependency.ClearAll();
            _memoryCache = new MemoryCache("memCache");
        }

        #endregion

        #region Cache setup

        public ICacheSetup GetSetup<T>(string key)
        {
            return new CacheSetup<T>(key, _defaultCacheMinutes);
        }

        public ICacheSetup GetSetup<T>(string key, int cacheMinutes)
        {
            return new CacheSetup<T>(key, cacheMinutes);
        }

        public ICacheSetup GetSetup<T>(string key, int cacheMinutes, IList<string> dependencies)
        {
            return new CacheSetup<T>(key, cacheMinutes, dependencies);
        }

        #endregion

        #region Async GetOrSet

        /// <summary>
        /// Gets and/or sets the result of given method to cache 
        /// Result is returned as integer. If no result is found, 0 is returned
        /// </summary>
        /// <param name="getItemCallback">function</param>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>Result of the delegated method</returns>
        public async Task<int> GetOrSetAsync(Func<Task<int>> getItemCallback, ICacheSetup cacheSetup)
        {
            var item = _memoryCache.Get(cacheSetup.CacheKey);

            if (item == null)
            {
                var cacheExpires = GetCacheExpires(cacheSetup.CacheMinutes);

                item = await getItemCallback();

                var cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = cacheExpires
                };

                if (item == null)
                {
                    // add dependency back to list
                    CacheDependency.AddCacheSetup(cacheSetup);
                    return 0;
                }

                // add result to cache
                _memoryCache.Add(cacheSetup.CacheKey, item, cacheItemPolicy);
            }
            else
            {
                // do not get data from cache if CacheSetup is not present
                if (!CacheDependency.GetDataFromCache(cacheSetup))
                {
                    // remove item from cache
                    _memoryCache.Remove(cacheSetup.CacheKey);

                    var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.CacheMinutes));
                    item = await getItemCallback();

                    if (item == null)
                    {
                        // add dependency back to list
                        CacheDependency.AddCacheSetup(cacheSetup);
                        return 0;
                    }

                    var cacheItemPolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = cacheExpires
                    };

                    // add result to cache
                    _memoryCache.Add(cacheSetup.CacheKey, item, cacheItemPolicy);
                }
            }

            // add dependency and return item
            CacheDependency.AddCacheSetup(cacheSetup);
            return (int)item;
        }

        /// <summary>
        /// Gets and/or sets the result of given method to cache 
        /// Result is returned as T
        /// </summary>
        /// <param name="getItemCallback">function</param>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>Result of the delegated method</returns>
        public async Task<T> GetOrSetAsync<T>(Func<Task<T>> getItemCallback, ICacheSetup cacheSetup) where T : class
        {
            // try to get item from cache
            var item = GetItem<T>(cacheSetup.CacheKey);

            if (item == null)
            {
                // check if empty object is stored in cache
                if (EmptyObjectIsCached(cacheSetup))
                {
                    return null;
                }

                var cacheExpires = GetCacheExpires(cacheSetup.CacheMinutes);

                item = await getItemCallback();

                var cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = cacheExpires
                };

                if (item == null)
                {
                    // add dependency back to list
                    CacheDependency.AddCacheSetup(cacheSetup);
                    AddEmptyObjectToCache(cacheSetup);
                    return null;
                }
                _memoryCache.Add(cacheSetup.CacheKey, item, cacheItemPolicy);
            }
            else
            {
                // do not get data from cache if CacheSetup is not present
                if (!CacheDependency.GetDataFromCache(cacheSetup))
                {
                    // remove item from cache
                    _memoryCache.Remove(cacheSetup.CacheKey);

                    var cacheExpires = GetCacheExpires(cacheSetup.CacheMinutes);

                    // execute function
                    item = await getItemCallback();

                    if (item == null)
                    {
                        // add dependency back to list
                        CacheDependency.AddCacheSetup(cacheSetup);
                        AddEmptyObjectToCache(cacheSetup);
                        return null;
                    }

                    var cacheItemPolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = cacheExpires
                    };

                    // add result to cache
                    _memoryCache.Add(cacheSetup.CacheKey, item, cacheItemPolicy);
                }
            }

            // add dependency and return item
            CacheDependency.AddCacheSetup(cacheSetup);
            return item;
        }

        #endregion

        #region GetOrSet

        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="getItemCallback">function</param>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>Result of the delegated method</returns>
        public T GetOrSet<T>(Func<T> getItemCallback, ICacheSetup cacheSetup) where T : class
        {
            // try to get item from cache
            var item = GetItem<T>(cacheSetup.CacheKey);

            if (item == null)
            {
                // check if empty object is stored in cache
                if (EmptyObjectIsCached(cacheSetup))
                {
                    return null;
                }

                // determine when cache expires
                var cacheExpires = GetCacheExpires(cacheSetup.CacheMinutes);

                // execute callback method
                item = getItemCallback();

                // check the result
                if (item == null)
                {
                    // executed function returned null
                    CacheDependency.AddCacheSetup(cacheSetup);
                    AddEmptyObjectToCache(cacheSetup);
                    return null;
                }

                var cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = cacheExpires
                };

                // add result to cache
                _memoryCache.Add(cacheSetup.CacheKey, item, cacheItemPolicy);
            }
            else
            {
                // do not get data from cache if CacheSetup is not present
                if (!CacheDependency.GetDataFromCache(cacheSetup))
                {
                    // remove item from cache
                    _memoryCache.Remove(cacheSetup.CacheKey);

                    var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.CacheMinutes));
                    item = getItemCallback();

                    if (item == null)
                    {
                        // add dependency back to list
                        CacheDependency.AddCacheSetup(cacheSetup);
                        AddEmptyObjectToCache(cacheSetup);
                        return null;
                    }

                    var cacheItemPolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = cacheExpires
                    };

                    _memoryCache.Add(cacheSetup.CacheKey, item, cacheItemPolicy);
                }
            }

            // add dependency and return item
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
            var item = _memoryCache.GetCacheItem(cacheKey);

            return item?.Value as T;
        }

        /// <summary>
        /// Gets given item from cache
        /// 
        /// </summary>
        /// <param name="cacheKey">Key of the item</param>
        /// <returns>Data from cache</returns>
        public object GetItem(string cacheKey)
        {
            var item = _memoryCache.GetCacheItem(cacheKey);
            return item?.Value;
        }

        #endregion

        #region Public methods

        public List<KeyValuePair<string, object>> GetMemoryUsageList()
        {
            return _memoryCache.ToList();
        }

        /// <summary>
        /// Returns true if values from given cacheSetup should be returned from cache or false if data should be re-loaded
        /// </summary>
        /// <param name="cacheSetup">cacheSetup</param>
        /// <returns>True if data should be returned from cache, false otherwise</returns>
        public bool CacheDataIsValid(ICacheSetup cacheSetup)
        {
            var item = _memoryCache.Get(cacheSetup.CacheKey);

            return item != null && CacheDependency.GetDataFromCache(cacheSetup);
        }

        /// <summary>
        /// Gets list of all setup lists in cache
        /// </summary>
        /// <returns>List of all cache setups</returns>
        public List<ICacheSetup> GetAllSetups()
        {
            return CacheDependency.GetAll();
        }

        /// <summary>
        /// Touches cache for given key (clears cache setups having key in its dependencies)
        /// </summary>
        /// <param name="key">Key to touch</param>
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
            var item = _memoryCache.Get(cacheSetup.CacheKey);
            if (item == null)
            {
                // nothing to invalidate
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
            return _forceDisableCaching ? 0 : cacheMinutes;
        }

        /// <summary>
        /// Checks if empty object is stored in cache
        /// Used when object to be stored in cache is null because it is not possible to store null it cache
        /// </summary>
        /// <param name="cacheSetup">CacheSetup</param>
        /// <returns>True if empty object is stored in cache, false otherwise</returns>
        private bool EmptyObjectIsCached(ICacheSetup cacheSetup)
        {
            var item = _memoryCache.Get(cacheSetup.CacheKey) as EmptyItem;

            return item != null;
        }

        /// <summary>
        /// Ads empty object to cache in case the "real" object is null 
        /// This prevents execution of SQL queries if object is null and therefore not stored in cache
        /// </summary>
        /// <param name="cacheSetup">cacheSetup</param>
        private void AddEmptyObjectToCache(ICacheSetup cacheSetup)
        {
            var cacheExpires = DateTime.Now.AddMinutes(GetCacheMinutes(cacheSetup.CacheMinutes));

            var cacheItemPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = cacheExpires
            };

            _memoryCache.Add(cacheSetup.CacheKey, new EmptyItem(), cacheItemPolicy);
        }

        private DateTime GetCacheExpires(int cacheMinutes)
        {
            return DateTime.Now.AddMinutes(GetCacheMinutes(cacheMinutes));
        }


        #endregion
    }
}
