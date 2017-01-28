using System;

namespace Core.Project
{
    public static class VersionInfo
    {
        #region Version

        /// <summary>
        /// Release date
        /// </summary>
        public static DateTime VersionRelase => new DateTime(2017, 15, 1);

        /// <summary>
        /// Version
        /// </summary>
        public static Version Version => new Version(0, 93);

        #endregion
    }
}
