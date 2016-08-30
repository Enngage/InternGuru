﻿using System;
using System.Collections.Generic;

namespace Cache
{
    public interface ICacheSetup
    {
        void SetUpdated(DateTime time);
        DateTime? Updated { get; }
        int CacheMinutes { get; }
        string CacheKey { get; }
        IList<String> Dependencies { get; set; }
        int ObjectID { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string Sort { get; set; }
    }
}
