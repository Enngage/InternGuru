namespace UI.Helpers
{
    public static class HtmlAddons
    {
        /// <summary>
        /// Replaces end lines with "<br />"
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>Html with <br /></returns>
        public static string AddLineBreaks(string text)
        {
            var textWithLines = text.Replace(System.Environment.NewLine, "<br />");
            textWithLines = text.Replace("\n", "<br />");
            return textWithLines;
        }
    }
}
