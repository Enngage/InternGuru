using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class UrlHelper: HelperBase
    {
        public UrlHelper(WebViewPage webViewPage) : base(webViewPage) { }

        public string CurrentAction => WebViewPage.ViewContext.RouteData.Values["action"].ToString();
    }
}
