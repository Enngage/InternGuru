using System;
using System.Web.Mvc;
using UI.RazorExtensions;

namespace UI.Base
{

    /// <summary>
    /// Base class for helpers used as BaseWebPage (accessible in Razor properties)
    /// </summary>
    public abstract class HelperBase
    {
        /// <summary>
        /// Represents WebViewPage 
        /// Note: Can be null in case HelperBase was initialized without passing WebViewPage
        /// </summary>
        private readonly WebViewPage _webViewPage;

        /// <summary>
        /// WebViewPage
        /// </summary>
        protected WebViewPage WebViewPage
        {
            get
            {
                if (_webViewPage == null)
                {
                    throw new NotSupportedException($"WebViewPage was not initialized. The WebViewPage is available only when helper is initialized via '{typeof(UIHelpersWebViewPage<>).Name}' and inherits from '{typeof(WebViewPage).Name}'. Make sure that child class contains constructor.");
                }
                return _webViewPage;
            }
        }

        /// <summary>
        /// Default constructor that does not initialize WebViewPage
        /// Helpers initialized this way cannot use utilities from WebViewPage (e.g. access ViewContext, HtmlHelper etc..)
        /// </summary>
        protected HelperBase()
        {
        }

        /// <summary>
        /// Initializes HelperBase class
        /// </summary>
        /// <param name="webViewPage">WebViewPage</param>
        protected HelperBase(WebViewPage webViewPage)
        {
            _webViewPage = webViewPage;
        }
    }
}
