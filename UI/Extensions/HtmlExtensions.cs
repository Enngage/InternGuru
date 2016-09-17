using System.Text;
using System.Web;
using System.Web.Mvc;

namespace UI.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Renders warning message with title
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="message">Message</param>
        /// <param name="title">Title of message</param>
        /// <returns>HTML for info message</returns>
        public static IHtmlString RenderWarningMessage(this HtmlHelper html, string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui warning message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return html.Raw(sb);
        }

        /// <summary>
        /// Renders warning message with no title
        /// </summary>
        /// <param name="html"></param>
        /// <param name="message">Message</param>
        /// <returns>HTML for info message</returns>
        public static IHtmlString RenderWarningMessage(this HtmlHelper html, string message)
        {
            return RenderWarningMessage(html, message, null);
        }

        /// <summary>
        /// Renders info message with title
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="message">Message</param>
        /// <param name="title">Title of message</param>
        /// <returns>HTML for info message</returns>
        public static IHtmlString RenderInfoMessage(this HtmlHelper html, string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui info message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return html.Raw(sb);
        }

        /// <summary>
        /// Renders info message with no title
        /// </summary>
        /// <param name="html"></param>
        /// <param name="message">Message</param>
        /// <returns>HTML for info message</returns>
        public static IHtmlString RenderInfoMessage(this HtmlHelper html, string message)
        {
            return RenderInfoMessage(html, message, null);
        }

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

        /// <summary>
        /// Renders validation summary - used when submitting forms
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title">Title to show</param>
        /// <returns>Validation summary HTML</returns>
        public static IHtmlString RenderValidationSummary(this HtmlHelper html, string title)
        {
            return RenderErrorMessage(html, System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(html, false).ToHtmlString(), title);
        }

        /// <summary>
        /// Renders validation summary - used when submitting forms
        /// </summary>
        /// <param name="html"></param>
        /// <returns>Validation summary HTML</returns>
        public static IHtmlString RenderValidationSummary(this HtmlHelper html)
        {
            return RenderErrorMessage(html, System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(html, false).ToHtmlString(), null);
        }

        /// <summary>
        /// Renders error message is form submittion is not valid
        /// Displays message ONLY if the ModelState is not valid
        /// </summary>
        /// <param name="html"></param>
        /// <param name="message">Message to show</param>
        /// <param name="title">Title to show</param>
        /// <returns>Html with error message</returns>
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


        /// <summary>
        /// Renders error message is form submittion is not valid
        /// Displays message ONLY if the ModelState is not valid
        /// </summary>
        /// <param name="html"></param>
        /// <param name="message">Message to show</param>
        /// <returns>Html with error message</returns>
        public static IHtmlString RenderErrorMessage(this HtmlHelper html, string message)
        {
            return RenderErrorMessage(html, message, null);
        }


        /// <summary>
        /// Renders error message regardless of whether ModelState is valid 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="message">Message to show</param>
        /// <param name="title">Title to show</param>
        /// <returns>Html with error message</returns>
        public static IHtmlString RenderCustomErrorMessage(this HtmlHelper html, string message, string title)
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

        /// <summary>
        /// Renders error message regardless of whether ModelState is valid 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="message">Message to show</param>
        /// <returns>Html with error message</returns>
        public static IHtmlString RenderCustomErrorMessage(this HtmlHelper html, string message)
        {
            return RenderCustomErrorMessage(html, message, null);
        }
    }
}
