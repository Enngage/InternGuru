using Core.Config.Enums;
using Core.Environment;
using System;
using System.Configuration;

namespace Core.Config
{
    public static class AppConfig
    {

        #region Environment

        /// <summary>
        /// Default cache minutes
        /// </summary>
        public static int DefaultCacheMinutes => Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCacheMinutes"]);

        /// <summary>
        /// Indicates if cache is enabled
        /// </summary>
        public static bool DisableCaching => ConfigurationManager.AppSettings["DisableCaching"] == "1";

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

        #region Web

        /// <summary>
        /// Represents main url of website
        /// </summary>
        public static string WebUrl => ConfigurationManager.AppSettings["WebUrl"];

        #endregion

        #region General

        /// <summary>
        /// E-mail of main contact (me)
        /// </summary>
        public static string MainContactEmail => ConfigurationManager.AppSettings["MainContactEmail"];

        /// <summary>
        /// Site name
        /// </summary>
        public static string SiteName => ConfigurationManager.AppSettings["SiteName"];

        /// <summary>
        /// Address
        /// </summary>
        public static string Address => ConfigurationManager.AppSettings["Address"];

        /// <summary>
        /// City
        /// </summary>
        public static string City => ConfigurationManager.AppSettings["City"];

        /// <summary>
        /// Country
        /// </summary>
        public static string Country => ConfigurationManager.AppSettings["Country"];

        /// <summary>
        /// Company name
        /// </summary>
        public static string CompanyName => ConfigurationManager.AppSettings["CompanyName"];

        /// <summary>
        /// Phone Contact (telephone number)
        /// </summary>
        public static string PhoneNumber => ConfigurationManager.AppSettings["PhoneNumber"];

        /// <summary>
        /// Company Lattitude
        /// </summary>
        public static double CompanyLat => Convert.ToDouble(ConfigurationManager.AppSettings["CompanyLat"]);

        /// <summary>
        /// Company Longtitude
        /// </summary>
        public static double CompanyLng => Convert.ToDouble(ConfigurationManager.AppSettings["CompanyLng"]);

        #endregion

        #region Files

        /// <summary>
        /// Maximum file size for upload
        /// </summary>
        public static int MaximumFileSize => Convert.ToInt32(ConfigurationManager.AppSettings["MaximumFileSize"]);

        #endregion

        #region E-mail

        /// <summary>
        /// Text used when subject is null or empty
        /// </summary>
        public static string NoSubjectText => ConfigurationManager.AppSettings["NoSubjectText"];

        /// <summary>
        /// From e-mail address
        /// </summary>
        public static string FromEmailAddress => ConfigurationManager.AppSettings["FromEmailAddress"];
        

        /// <summary>
        /// Name of folder where e-mail templates are stored
        /// </summary>
        public static string EmailTemplatesFolder => ConfigurationManager.AppSettings["EmailTemplatesFolder"];

        #endregion

        #region Email provider

        public static string GmailUserName => ConfigurationManager.AppSettings["GmailUsername"];

        public static int GmailPort => Convert.ToInt32(ConfigurationManager.AppSettings["GmailPort"]);

        public static string GmailPassword => ConfigurationManager.AppSettings["GmailPassword"];

        public static string GmailHost => ConfigurationManager.AppSettings["GmailHost"];

        public static bool GmailSsl => ConfigurationManager.AppSettings["GmailSSL"].Equals("true", StringComparison.OrdinalIgnoreCase);

        #endregion

        #region Social

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

        /// <summary>
        /// Represents Twitter url
        /// </summary>
        public static string TwitterAppUrl => ConfigurationManager.AppSettings["TwitterAppUrl"];

        #endregion

        #region Cookie names

        public static CookieConfigWrapper CookieNames => new CookieConfigWrapper();

        #endregion

    }
}
