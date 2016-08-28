using System;
using System.Collections.Generic;

namespace Cache
{
    internal class CacheSetup<T> : CacheSetupAbstract, ICacheSetup
    {
        #region Cache properties

        /// <summary>
        /// List of dependencies
        /// </summary>
        public IList<string> Dependencies { get; set; }

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
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key cannot be null");
            }

            // set cache properties
            this.inputKey = key;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = new List<string>();
        }

        /// <summary>
        /// Initializes cache configuration
        /// </summary>
        /// <param name="key">Name of the cache in memory (Usually method where it is called)</param>
        /// <param name="cacheMinutes">Indicates how long the item is stored in cache</param>
        /// <param name="dependencies">List of dependencies (when key is touched with, all cache items having the key in cache dependency list will be cleared)</param>
        public CacheSetup(string key, int cacheMinutes, IList<string> dependencies)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key cannot be null");
            }

            // set cache properties
            this.inputKey = key;
            this.cacheMinutes = cacheMinutes;

            if (dependencies == null)
            {
                this.dependencies = new List<string>();
            }
            else
            {
                this.dependencies = dependencies;
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Gets full name of cache setup based on its properties (key, objectID, pageNumber, sort ...)
        /// </summary>
        /// <returns></returns>
        public string GetCacheKey() {
            string fullKey = string.Format("key[0]", this.inputKey);

            fullKey += string.Format(".cacheFor[0]", this.cacheMinutes);

            if (ObjectID > 0)
            {
                fullKey += string.Format(".id[0]", this.ObjectID);
            }

            if (PageNumber > 0)
            {
                fullKey += string.Format(".page[0]", this.PageNumber);
            }

            if (PageSize > 0)
            {
                fullKey += string.Format(".size[0]", this.PageSize);
            }

            if (!String.IsNullOrEmpty(this.Sort))
            {
                fullKey += string.Format(".sort[0]", this.Sort);
            }

            return fullKey;
        }

        #endregion
    }
}
