using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class HtmlHelper : HelperBase
    {

        public HtmlHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Replaces end lines with "<br />"
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>Html with <br /></returns>
        public string AddLineBreaks(string text)
        {
            var textWithLines = text.Replace(System.Environment.NewLine, "<br />");
            textWithLines = textWithLines.Replace("\n", "<br />");
            return textWithLines;
        }
    }
}
