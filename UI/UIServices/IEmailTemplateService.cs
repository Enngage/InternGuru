
using System.Collections.Generic;
using UI.UIServices.Models;

namespace UI.UIServices
{
    public interface IEmailTemplateService
    {
        /// <summary>
        /// Gets HTML of a template
        /// </summary>
        /// <param name="fileName">File name of the template (e.g. "mytemplate.html")</param>
        /// <returns>HTML of template</returns>
        string GetTemplateHtml(string fileName);

        /// <summary>
        /// Gets text from given template and replaces macros
        /// </summary>
        /// <param name="fileName">File name of the template (e.g. "mytemplate.html")</param>
        /// <param name="replacements">Replacements</param>
        /// <returns>Text with replaced macros</returns>
        string GetTemplateHtml(string fileName, IEnumerable<MacroReplacement> replacements);

        /// <summary>
        /// Replaces macros in given text
        /// </summary>
        /// <param name="text">Text in which macros will be replaced</param>
        /// <param name="replacements">Replacements</param>
        /// <returns>Text with replaced macros</returns>
        string ReplaceMacros(string text, IEnumerable<MacroReplacement> replacements);
    }
}
