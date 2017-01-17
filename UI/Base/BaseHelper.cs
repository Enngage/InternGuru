using System.Web.Mvc;

namespace UI.Base
{
    public abstract class HelperBase
    {
        protected WebViewPage WebViewPage { get; }

        protected HelperBase(WebViewPage webViewPage)
        {
            WebViewPage = webViewPage;
        }
    }
}
