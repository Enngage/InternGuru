using System;
using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class UrlHelper: HelperBase
    {
        public UrlHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Current absolute url
        /// </summary>
        public string CurrentUrl => WebViewPage.Request.Url?.AbsoluteUri;

        /// <summary>
        /// Url to the main site without actions/params
        /// </summary>
        public string SiteUrl => WebViewPage.Request.Url?.Scheme + "://" + WebViewPage.Request.Url?.Authority;

        /// <summary>
        /// Action of current request
        /// </summary>
        public string CurrentAction => WebViewPage.ViewContext.RouteData.Values["action"].ToString();

        /// <summary>
        /// Controller of current request
        /// </summary>
        public string CurrentController => WebViewPage.ViewContext.RouteData.Values["controller"].ToString();

        /// <summary>
        /// Converts given relative url to absolute url
        /// </summary>
        /// <param name="relativeUrl">Relative url (eg: /Content/file.jpg)</param>
        /// <returns>Absolute url or null if converting path fails</returns>
        public string GetAbsoluteUrl(string relativeUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(relativeUrl))
                {
                    return null;
                }

                // check if relative url isnt absolute already
                if (relativeUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return relativeUrl;
                }

                var absolute = new Uri(SiteUrl);

                // add "/" before relativeUrl if it is not present to ensure correct path
                if (!relativeUrl.StartsWith("/"))
                {
                    relativeUrl = "/" + relativeUrl;
                }

                return new Uri(absolute, relativeUrl).AbsoluteUri;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets value from given query string
        /// </summary>
        /// <param name="name">Name of the query string</param>
        /// <returns>Value from query string or null</returns>
        public string GetQueryString(string name)
        {
            return WebViewPage.Request.QueryString[name];
        }

        /// <summary>
        /// Gets value from given query string
        /// </summary>
        /// <param name="name">Name of the query string</param>
        /// <param name="defaultValue">Default value if string is null</param>
        /// <returns>Value of the query string or default value if query string with given name does not exist or is empty</returns>
        public string GetQueryString(string name, string defaultValue)
        {
            return WebViewPage.Request.QueryString[name] ?? defaultValue;
        }

        /// <summary>
        /// Gets value from current route parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <returns>Value of the route parameter or null if the route param does not exist</returns>
        public string GetRouteParam(string name)
        {
            return WebViewPage.ViewContext.RouteData.Values[name]?.ToString();
        }


        /// <summary>
        /// Gets value from current route parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="defaultValue">Default value if param does not exist</param>
        /// <returns>Value of the query string or default value if query string with given name does not exist or is empty</returns>
        public string GetRouteParam(string name, string defaultValue)
        {
            return WebViewPage.ViewContext.RouteData.Values[name]?.ToString() ?? defaultValue;
        }

        /// <summary>
        /// Gets URL to Facebook profile based on userName
        /// </summary>
        /// <param name="userName">UserName of facebook account</param>
        /// <returns>URL to Facebook profile</returns>
        public string GetFacebookUrl(string userName)
        {
            return $"https://www.facebook.com/{userName}";
        }

        /// <summary>
        /// Gets URL to Twitter profile based on userName
        /// </summary>
        /// <param name="userName">UserName of Twitter account</param>
        /// <returns>URL to Twitter profile</returns>
        public string GetTwitterUrl(string userName)
        {
            return $"https://www.twitter.com/{userName}";
        }

        /// <summary>
        /// Gets URL to LinkedIn profile based on userName
        /// </summary>
        /// <param name="userName">UserName of LinkedIn account</param>
        /// <returns>URL to LinkedIn profile</returns>
        public string GetLinkedInUrl(string userName)
        {
            return $"https://www.linkedin.com/company/{userName}";
        }
    }
}
