using System;

namespace Common.Project
{
    public static class VersionInfo
    {
        #region Version

        /// <summary>
        /// Release date
        /// </summary>
        public static DateTime VersionRelase
        {
            get
            {
                return new DateTime(2016, 8, 21);
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        public static Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }

        #endregion
    }
}
