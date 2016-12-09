using System;

namespace Core.Project
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
                return new DateTime(2016, 11, 9);
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        public static Version Version
        {
            get
            {
                return new Version(0, 90);
            }
        }

        #endregion
    }
}
