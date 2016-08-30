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
        protected string typeName;

        #endregion

        #region Interface members

        /// <summary>
        /// Update time when cache item was last updated
        /// </summary>
        public void SetUpdated(DateTime updateTime)
        {
            this.updated = updateTime;
        }

        /// <summary>
        /// Represents type name passed to cache setup
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.typeName;
            }
        }

        /// <summary>
        /// Time when cache item was last updated
        /// </summary>
        public DateTime? Updated
        {
            get
            {
                return this.updated;
            }
        }

        /// <summary>
        /// Indicates for how lond the item should be stored in cache
        /// </summary>
        public int CacheMinutes
        {
            get
            {
                return this.cacheMinutes;
            }
        }

        /// <summary>
        /// Dependencies of cache item
        /// </summary>
        public IList<string> Dependencies
        {
            get
            {
                return this.dependencies;
            }
            set
            {
                this.dependencies = value;
            }
        }

        #endregion
    }
}
