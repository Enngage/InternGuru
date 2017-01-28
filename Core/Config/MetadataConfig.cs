
using System.Configuration;

namespace Core.Config
{
    public static class MetadataConfig
    {
        #region Default metadata

        /// <summary>
        /// Path to default image
        /// </summary>
        public static string DefaultImagePath => ConfigurationManager.AppSettings["DefaultImagePath"];

        /// <summary>
        /// Default Description
        /// </summary>
        public static string DefaultDescription => ConfigurationManager.AppSettings["DefaultDescription"];

        /// <summary>
        /// Default Title
        /// </summary>
        public static string DefaultTitle => ConfigurationManager.AppSettings["DefaultTitle"];


        /// <summary>
        /// Path to apple touch icon
        /// </summary>
        public static string AppleTouchIconPath => ConfigurationManager.AppSettings["AppleTouchIconPath"];

        #endregion
    }
}
