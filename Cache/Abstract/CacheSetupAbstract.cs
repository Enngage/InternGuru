using System;
using System.Collections.Generic;

namespace Cache
{
    internal abstract class CacheSetupAbstract
    {

        #region Variables

        protected string inputKey;
        protected int cacheMinutes;
        protected IList<String> dependencies;
        protected DateTime? updated;

        #endregion

        #region Interface methods

        /// <summary>
        /// Time when cache item was last updated
        /// </summary>
        /// <returns>Update date</returns>
        public DateTime? GetUpdated()
        {
            return this.updated;
        }

        /// <summary>
        /// Sets new updated date of cache item
        /// </summary>
        /// <param name="time">New date time</param>
        public void SetUpdated(DateTime time)
        {
            this.updated = time;
        }

        /// <summary>
        /// Indicates for how lond the item should be stored in cache
        /// </summary>
        /// <returns></returns>
        public int GetCacheMinutes()
        {
            return this.cacheMinutes;
        }

        /// <summary>
        /// Gets all dependencies
        /// </summary>
        /// <returns>List of dependencies</returns>
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
