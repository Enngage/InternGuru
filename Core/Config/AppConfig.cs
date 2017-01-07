using Core.Config.Enums;
using Core.Environment;
using System;
using System.Configuration;

namespace Core.Config
{
    public static class AppConfig
    {

        #region Cache

        /// <summary>
        /// Default cache minutes
        /// </summary>
        public static int DefaultCacheMinutes => Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCacheMinutes"]);

        /// <summary>
        /// Indicates if cache is enabled
        /// </summary>
        public static bool DisableCaching => ConfigurationManager.AppSettings["DisableCaching"] == "1";

        #endregion

        #region E-mail

        /// <summary>
        /// From e-mail address
        /// </summary>
        public static string NoReplyEmailAddress => ConfigurationManager.AppSettings["NoReplyEmailAddress"];

        #endregion

        #region Maximum file size

        /// <summary>
        /// Maximum file size for upload
        /// </summary>
        public static int MaximumFileSize => Convert.ToInt32(ConfigurationManager.AppSettings["MaximumFileSize"]);

        #endregion

        #region Site

        /// <summary>
        /// Represents site name
        /// </summary>
        public static string SiteName => ConfigurationManager.AppSettings["SiteName"];

        #endregion

        #region Facebook

        /// <summary>
        /// Represents Facebook AppID
        /// </summary>
        public static string FacebookAppID => ConfigurationManager.AppSettings["FacebookAppID"];

        /// <summary>
        /// Represents Facebook AppSecret
        /// </summary>
        public static string FacebookAppSecret => ConfigurationManager.AppSettings["FacebookAppSecret"];

        /// <summary>
        /// Represents Facebook page URL (www.facebook.com/elmonosdb ...)
        /// </summary>
        public static string FacebookAppUrl => ConfigurationManager.AppSettings["FacebookAppUrl"];

        #endregion

        #region Google

        /// <summary>
        /// Represents Google API KEY
        /// </summary>
        public static string GoogleApiKey => ConfigurationManager.AppSettings["GoogleApiKey"];

        /// <summary>
        /// Represents Google client ID 
        /// </summary>
        public static string GoogleClientID => ConfigurationManager.AppSettings["GoogleClientID"];

        /// <summary>
        /// Represents Google client secret 
        /// </summary>
        public static string GoogleClientSecret => ConfigurationManager.AppSettings["GoogleClientSecret"];

        #endregion

        #region Twitter

        /// <summary>
        /// Represents Twitter url
        /// </summary>
        public static string TwitterAppUrl => ConfigurationManager.AppSettings["TwitterAppUrl"];

        #endregion

        #region Environment

        /// <summary>
        /// Represents evironment of current instance (based on web.config key)
        /// </summary>
        public static EnvironmentEnum Environment
        {
            get
            {
                var value = ConfigurationManager.AppSettings["Environment"];

                if (value.Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    return EnvironmentEnum.Live;
                }
                if (value.Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    return EnvironmentEnum.Dev;
                }
                throw new Exception("Invalid environment. Review 'Environment' web.config key");
            }
        }

        #endregion

        #region Cookie names

        public static CookieConfigWrapper CookieNames => new CookieConfigWrapper();

        #endregion

    }
}
