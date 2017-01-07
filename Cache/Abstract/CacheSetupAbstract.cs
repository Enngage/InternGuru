using System;
using System.Collections.Generic;

namespace Cache.Abstract
{
    internal abstract class CacheSetupAbstract
    {

        #region Variables

        protected string InputKey;

        #endregion

        #region Interface members

        /// <summary>
        /// Represents type name passed to cache setup
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Time when cache item was last updated
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Indicates for how lond the item should be stored in cache
        /// </summary>
        public int CacheMinutes { get; set; }

        /// <summary>
        /// Dependencies of cache item
        /// </summary>
        public IList<string> Dependencies { get; set; }

        public void SetUpdated(DateTime time)
        {
            Updated = time;
        }

        #endregion
    }
}
