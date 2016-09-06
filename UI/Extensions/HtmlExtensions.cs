using System.Text;
using System.Web;
using System.Web.Mvc;

namespace UI.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString RenderSuccessMessage(this HtmlHelper html, string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui positive message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return html.Raw(sb);
        }

        public static IHtmlString RenderSuccessMessage(this HtmlHelper html, string message)
        {
            return RenderSuccessMessage(html, message, null);
        }

        public static IHtmlString RenderValidationSummary(this HtmlHelper html, string title)
        {
            return RenderErrorMessage(html, System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(html, false).ToHtmlString(), title);
        }

        public static IHtmlString RenderValidationSummary(this HtmlHelper html)
        {
            return RenderErrorMessage(html, System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(html, false).ToHtmlString(), null);
        }

        public static IHtmlString RenderErrorMessage(this HtmlHelper html, string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui negative message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return html.Raw(sb);
        }

        public static IHtmlString RenderErrorMessage(this HtmlHelper html, string message)
        {
            return RenderSuccessMessage(html, message, null);
        }
    }
}
