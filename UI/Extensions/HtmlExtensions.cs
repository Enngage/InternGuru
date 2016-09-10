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

        /// <summary>
        /// Renders either success message or validation form summary in a nicely presented HTML format
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="isSuccess">Indicates if form was saved successfully</param>
        /// <param name="successMessage">Message to show if form was saved successfully</param>
        /// <returns></returns>
        public static IHtmlString RenderFormValidationResult(this HtmlHelper html, bool isSuccess, string successMessage)
        {
            if (isSuccess)
            {
                return RenderSuccessMessage(html, successMessage);
            }
            else
            {
                return RenderValidationSummary(html);
            }
        }

        /// <summary>
        /// Renders either success message or validation form summary in a nicely presented HTML format
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="isSuccess">Indicates if form was saved successfully</param>
        /// <returns></returns>
        public static IHtmlString RenderFormValidationResult(this HtmlHelper html, bool isSuccess)
        {
            if (isSuccess)
            {
                return RenderSuccessMessage(html, "Uloženo");
            }
            else
            {
                return RenderValidationSummary(html);
            }
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
            // render only if model is not valid
            if (!html.ViewData.ModelState.IsValid)
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

            return null;
        }

        public static IHtmlString RenderErrorMessage(this HtmlHelper html, string message)
        {
            return RenderSuccessMessage(html, message, null);
        }
    }
}
