using System;

namespace Common.Project
{
    public static class VersionInfo
    {
        #region Version

        public static DateTime VersionRelase
        {
            get
            {
                return new DateTime(2016, 8, 21);
            }
        }

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
