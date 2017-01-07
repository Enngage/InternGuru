using System;
using System.Collections.Generic;
using Cache.Abstract;

namespace Cache.Models
{
    internal class CacheSetup<T> : CacheSetupAbstract, ICacheSetup
    {
        #region Cache properties

        /// <summary>
        /// Identifies page number of cache item
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Identifies page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Identifies object ID of item stored in cache
        /// </summary>
        public int ObjectID { get; set; }

        /// <summary>
        /// Identifies object ID using string of item stored in cache
        /// </summary>
        public string ObjectStringID { get; set; }

        /// <summary>
        /// Identifies sort under which cache item is stored
        /// </summary>
        public string Sort { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes cache configuration
        /// </summary>
        /// <param name="key">Name of the cache in memory (Usually method where it is called)</param>
        /// <param name="cacheMinutes">Indicates how long the item is stored in cache</param>
        public CacheSetup(string key, int cacheMinutes)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            // set cache properties
            TypeName = typeof(T).Name;
            InputKey = key;
            CacheMinutes = cacheMinutes;
            Dependencies = new List<string>();
        }

        /// <summary>
        /// Initializes cache configuration
        /// </summary>
        /// <param name="key">Name of the cache in memory (Usually method where it is called)</param>
        /// <param name="cacheMinutes">Indicates how long the item is stored in cache</param>
        /// <param name="dependencies">List of dependencies (when key is touched with, all cache items having the key in cache dependency list will be cleared)</param>
        public CacheSetup(string key, int cacheMinutes, IList<string> dependencies)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            // set cache properties
            TypeName = typeof(T).Name;
            InputKey = key;
            CacheMinutes = cacheMinutes;

            this.Dependencies = dependencies ?? new List<string>();
        }

        #endregion

        #region Cache key

        /// <summary>
        /// Cache key used for object identification in memory
        /// </summary>
        public string CacheKey
        {
            get
            {
                return GetCacheKey();
            }
        }

        #endregion

        #region Key construction

        /// <summary>
        /// Gets full name of cache setup based on its properties (key, object type, objectID, pageNumber, sort ...)
        /// </summary>
        /// <returns>Cache key</returns>
        private string GetCacheKey() {
            string fullKey = $"key[{InputKey}]";

            fullKey += $".cacheFor[{CacheMinutes}]";

            fullKey += $".class[{TypeName}]";

            if (!string.IsNullOrEmpty(ObjectStringID))
            {
                fullKey += $".sid[{ObjectStringID}]";
            }

            if (ObjectID > 0)
            {
                fullKey += $".id[{ObjectID}]";
            }

            if (PageNumber > 0)
            {
                fullKey += $".page[{PageNumber}]";
            }

            if (PageSize > 0)
            {
                fullKey += $".size[{PageSize}]";
            }

            if (!string.IsNullOrEmpty(Sort))
            {
                fullKey += $".sort[{Sort}]";
            }

            return fullKey;
        }

        #endregion
    }
}
