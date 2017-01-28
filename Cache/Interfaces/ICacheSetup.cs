using System;
using System.Collections.Generic;

namespace Cache
{
    public interface ICacheSetup
    {
        void SetUpdated(DateTime time);
        DateTime? Updated { get; }
        int CacheMinutes { get; }
        string CacheKey { get; }
        IList<string> Dependencies { get; set; }
        int ObjectID { get; set; }
        string ObjectStringID { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string Sort { get; set; }
    }
}
