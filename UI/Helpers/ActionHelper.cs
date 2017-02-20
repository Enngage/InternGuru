using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class ActionHelper : HelperBase
    {
        public ActionHelper(WebViewPage webViewPage) : base(webViewPage) { }

        public IHtmlString RenderDeleteButton(int objectID, string elementClass)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<a href=\"#\" data-id=\"{objectID}\" class=\"{elementClass}\">");
            sb.AppendLine("<i title=\"Smazat\" class=\"remove icon black\"></i>");
            sb.AppendLine("</a>");

            return WebViewPage.Html.Raw(sb.ToString());
        }

        public IHtmlString RenderEditButton(string actionName, string controllerName, object routeValues)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<a href=\"{WebViewPage.Url.Action(actionName, controllerName, routeValues)}\">");
            sb.AppendLine("<i title=\"Upravit\" class=\"edit icon black\"></i>");
            sb.AppendLine("</a>");

            return WebViewPage.Html.Raw(sb.ToString());
        }

        public IHtmlString RenderViewButton(string actionName, string controllerName, object routeValues)
        {
            return RenderViewButton(actionName, controllerName, routeValues, true);
        }

        public IHtmlString RenderViewButton(string actionName, string controllerName, object routeValues, bool openInNewWindow)
        {
            var sb = new StringBuilder();
            var target = openInNewWindow ? "target=\"_blank\"" : string.Empty;
            sb.AppendLine($"<a href=\"{WebViewPage.Url.Action(actionName, controllerName, routeValues)}\" {target}>");
            sb.AppendLine("<i title=\"Zobrazit\" class=\"unhide icon black\"></i>");
            sb.AppendLine("</a>");

            return WebViewPage.Html.Raw(sb.ToString());
        }
    }
}
