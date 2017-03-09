using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Base;
using UI.Builders.Shared.Models;

namespace UI.Helpers
{
    public class HeaderHelper : HelperBase
    {
        public HeaderHelper(WebViewPage webViewPage) : base(webViewPage) { }

        private const string HeaderViewPath = "~/Views/Modules/Header/Header.cshtml";

        public IHtmlString RenderHeader(IUiHeader header)
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return this.WebViewPage.Html.Partial(HeaderViewPath, header);
        }
    }
}
