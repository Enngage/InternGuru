using System.Text;
using System.Web;
using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class MessageHelper : HelperBase
    {

        public MessageHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Renders icon using given class 
        /// See http://semantic-ui.com/elements/icon.html for icon class names
        /// </summary>
        /// <param name="className">Class name (see Semantic-ui icons) without "icon"</param>
        /// <returns>Icon</returns>
        public IHtmlString RenderIcon(string className)
        {
            return string.IsNullOrEmpty(className) ? null : WebViewPage.Html.Raw($"<i class=\"{className} icon\"></i>");
        }

        /// <summary>
        /// Renders warning message with title
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title of message</param>
        /// <returns>HTML for info message</returns>
        public IHtmlString RenderWarningMessage(string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui warning message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return WebViewPage.Html.Raw(sb);
        }

        /// <summary>
        /// Renders warning message with no title
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>HTML for info message</returns>
        public IHtmlString RenderWarningMessage(string message)
        {
            return RenderWarningMessage(message, null);
        }

        /// <summary>
        /// Renders info message with title
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title of message</param>
        /// <returns>HTML for info message</returns>
        public IHtmlString RenderInfoMessage(string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui info message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return WebViewPage.Html.Raw(sb);
        }

        /// <summary>
        /// Renders info message with no title
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>HTML for info message</returns>
        public IHtmlString RenderInfoMessage(string message)
        {
            return RenderInfoMessage(message, null);
        }

        public IHtmlString RenderSuccessMessage(string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui positive message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return WebViewPage.Html.Raw(sb);
        }

        /// <summary>
        /// Renders either success message or validation form summary in a nicely presented HTML format
        /// </summary>
        /// <param name="isSuccess">Indicates if form was saved successfully</param>
        /// <param name="successMessage">Message to show if form was saved successfully</param>
        /// <returns></returns>
        public IHtmlString RenderFormValidationResult(bool isSuccess, string successMessage)
        {
            return isSuccess ? RenderSuccessMessage(successMessage) : RenderValidationSummary();
        }

        /// <summary>
        /// Renders either success message or validation form summary in a nicely presented HTML format
        /// </summary>
        /// <param name="isSuccess">Indicates if form was saved successfully</param>
        /// <returns></returns>
        public IHtmlString RenderFormValidationResult( bool isSuccess)
        {
            return isSuccess ? RenderSuccessMessage("Uloženo") : RenderValidationSummary();
        }

        public IHtmlString RenderSuccessMessage(string message)
        {
            return RenderSuccessMessage(message, null);
        }

        /// <summary>
        /// Renders validation summary - used when submitting forms
        /// </summary>
        /// <param name="title">Title to show</param>
        /// <returns>Validation summary HTML</returns>
        public IHtmlString RenderValidationSummary(string title)
        {
            return RenderErrorMessage(System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(WebViewPage.Html, false).ToHtmlString(), title);
        }

        /// <summary>
        /// Renders validation summary - used when submitting forms
        /// </summary>
        /// <returns>Validation summary HTML</returns>
        public IHtmlString RenderValidationSummary()
        {
            return RenderErrorMessage(System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(WebViewPage.Html, false).ToHtmlString(), null);
        }

        /// <summary>
        /// Renders error message is form submittion is not valid
        /// Displays message ONLY if the ModelState is not valid
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="title">Title to show</param>
        /// <returns>Html with error message</returns>
        public IHtmlString RenderErrorMessage(string message, string title)
        {
            // render only if model is not valid
            if (!WebViewPage.Html.ViewData.ModelState.IsValid)
            {

                var sb = new StringBuilder();
                sb.AppendLine("<div class=\"ui negative message\">");

                if (!string.IsNullOrEmpty(title))
                {
                    sb.AppendFormat("<div class=\"header\">{0}</div>", title);
                }

                sb.AppendFormat("<p>{0}</p>", message);
                sb.AppendLine("</div>");

                return WebViewPage.Html.Raw(sb);
            }

            return null;
        }

        /// <summary>
        /// Renders error message is form submittion is not valid
        /// Displays message ONLY if the ModelState is not valid
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <returns>Html with error message</returns>
        public IHtmlString RenderErrorMessage(string message)
        {
            return RenderErrorMessage(message, null);
        }

        /// <summary>
        /// Renders error message regardless of whether ModelState is valid 
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="title">Title to show</param>
        /// <returns>Html with error message</returns>
        public IHtmlString RenderCustomErrorMessage(string message, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ui negative message\">");

            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendFormat("<div class=\"header\">{0}</div>", title);
            }

            sb.AppendFormat("<p>{0}</p>", message);
            sb.AppendLine("</div>");

            return WebViewPage.Html.Raw(sb);
        }

        /// <summary>
        /// Renders error message regardless of whether ModelState is valid 
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <returns>Html with error message</returns>
        public IHtmlString RenderCustomErrorMessage(string message)
        {
            return RenderCustomErrorMessage(message, null);
        }
    }
}
