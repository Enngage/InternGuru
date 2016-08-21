using Common.Environment;
using System;
using System.Configuration;

namespace Common.Config
{
    public static class AppConfig
    {

        #region Site

        /// <summary>
        /// Represents site name
        /// </summary>
        public static string SiteName
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteName"];
            }
        }

        #endregion

        #region Facebook

        /// <summary>
        /// Represents Facebook AppID
        /// </summary>
        public static string FacebookAppID
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppID"];
            }
        }

        /// <summary>
        /// Represents Facebook AppSecret
        /// </summary>
        public static string FacebookAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppSecret"];
            }
        }

        /// <summary>
        /// Represents Facebook page URL (www.facebook.com/elmonosdb ...)
        /// </summary>
        public static string FacebookAppUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppUrl"];
            }
        }

        #endregion

        #region Google


        /// <summary>
        /// Represents Google client ID 
        /// </summary>
        public static string GoogleClientID
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleClientID"];
            }
        }

        /// <summary>
        /// Represents Google client secret 
        /// </summary>
        public static string GoogleClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleClientSecret"];
            }
        }

        #endregion

        #region Twitter

        /// <summary>
        /// Represents Twitter url
        /// </summary>
        public static string TwitterAppUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterAppUrl"];
            }
        }

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
                else if (value.Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    return EnvironmentEnum.Dev;
                }
                else
                {
                    throw new Exception("Invalid environment. Review 'Environment' web.config key");
                }
            }
        }

        #endregion

    }
}
