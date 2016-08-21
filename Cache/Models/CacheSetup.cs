using System;
using System.Collections.Generic;

namespace Cache
{
    internal class CacheSetup<T> : CacheSetupAbstract, ICacheSetup
    {

        #region Constructors

        public CacheSetup(string key, int cacheMinutes)
        {
            string cacheKey = ConstructCacheKey(key);

            this.cacheKey = cacheKey;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = new List<String>();
        }


        public CacheSetup(string key, int cacheMinutes, string objectID)
        {
            string cacheKey = ConstructCacheKeyUnique(key, objectID);

            this.cacheKey = cacheKey;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = new List<String>();
        }

        public CacheSetup(string source, int cacheMinutes, List<String> dependencies)
        {
            string cacheKey = ConstructCacheKey(source);

            this.cacheKey = cacheKey;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = dependencies == null ? new List<String>() : dependencies;
        }


        public CacheSetup(string source, int cacheMinutes, string objectID, List<String> dependencies)
        {
            string cacheKey = ConstructCacheKey(objectID.ToString(), source);

            this.cacheKey = cacheKey;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = dependencies == null ? new List<String>() : dependencies;
        }


        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="source">Source of calling (ideally: "{controller}.{action}" </param>
        /// <param name="objectID">Further identification of object</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="sort">sort</param>
        public CacheSetup(string source, int cacheMinutes, string objectID, int pageNumber, int pageSize, string sort, List<String> dependencies = null)
        {
            string cacheKey = ConstructCacheKey(objectID, source, pageNumber, pageSize, sort);

            this.cacheKey = cacheKey;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = dependencies == null ? new List<String>() : dependencies;
        }

        /// <summary>
        /// Gets and/or sets the result of given method to cache
        /// </summary>
        /// <param name="source">Source of calling (ideally: "{controller}.{action}" </param>
        /// <param name="objectID">Further identification of object</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="sort">sort</param>
        public CacheSetup(string source, int cacheMinutes, int pageNumber, int pageSize, string sort, List<String> dependencies = null)
        {
            string cacheKey = ConstructCacheKey(source, pageNumber, pageSize, sort);

            this.cacheKey = cacheKey;
            this.cacheMinutes = cacheMinutes;
            this.dependencies = dependencies == null ? new List<String>() : dependencies;
        }

        #endregion

        #region Cache key builders

        private string ConstructCacheKeyUnique(string source, string objectID)
        {
            var typeOf = typeof(T);
            // get unique cache key
            return typeOf.Name + ".type[various]." + "source[" + source + "]." + "id[" + objectID + "]";
        }


        private string ConstructCacheKey(string source, int pageNumber, int pageSize, string sort = null)
        {
            var typeOf = typeof(T);
            // get unique cache key
            return typeOf.Name + ".type[pagedList]." + "source[" + source + "]." + "page[" + pageNumber + "]." + "pageSize[" + pageSize + "]." + "sort[" + sort + "]";
        }

        private string ConstructCacheKey(string objectID, string source, int pageNumber, int pageSize, string sort = null)
        {
            var typeOf = typeof(T);
            // get unique cache key
            return typeOf.Name + ".type[pagedList]." + "objectID[" + objectID + "]." + "source[" + source + "]." + "page[" + pageNumber + "]." + "pageSize[" + pageSize + "]." + "sort[" + sort + "]";
        }

        private string ConstructCacheKey(string objectID, string source)
        {
            var typeOf = typeof(T);
            // get unique cache key
            return typeOf.Name + ".type[single]." + "objectID[" + objectID + "]." + "source[" + source + "]";
        }

        private string ConstructCacheKey(string source)
        {
            var typeOf = typeof(T);
            // get unique cache key
            return typeOf.Name + ".type[various]" + ".source[" + source + "]"; ;
        }

        #endregion
    }
}
