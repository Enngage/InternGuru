
using System;
using System.Collections.Generic;
using UI.Emails;
using UI.UIServices.Models;

namespace UI.UIServices
{
    public interface IEmailTemplateService
    {

        /// <summary>
        /// Path to e-mail templates folder
        /// </summary>
        string ServerEmailTemplateFolderPath { get; }

        /// <summary>
        /// Gets HTML of a template
        /// </summary>
        /// <param name="emailType">Type of e-mail</param>
        /// <returns>HTML of template</returns>
        string GetTemplateHtml(EmailTypeEnum emailType);

        /// <summary>
        /// Gets text from given template and replaces macros
        /// </summary>
        /// <param name="emailType">Type of e-mail</param>
        /// <param name="replacements">Replacements</param>
        /// <returns>Text with replaced macros</returns>
        string GetTemplateHtml(EmailTypeEnum emailType, IEnumerable<MacroReplacement> replacements);

        /// <summary>
        /// Gets HTML of a template
        /// </summary>
        /// <param name="fileName">File name of the template without extension(e.g. "mytemplate")</param>
        /// <returns>HTML of template</returns>
        string GetTemplateHtml(string fileName);

        /// <summary>
        /// Gets text from given template and replaces macros
        /// </summary>
        /// <param name="fileName">File name of the template without extension(e.g. "mytemplate")</param>
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

        /// <summary>
        /// List of default macro replacements that are available in all e-mail templates. 
        /// Fields include "Site name", "Address" etc...
        /// Should be called by GetTemplateHtml methods
        /// </summary>
        /// <returns>Collection of default macro replacements</returns>
        IEnumerable<MacroReplacement> GetDefaultMacroReplacemens();

        /// <summary>
        /// Gets basic e-mail using default data
        /// </summary>
        /// <param name="recipient">E-mail address of recipient</param>
        /// <param name="title">Title of e-mail</param>
        /// <param name="text">Text of e-mail</param>
        /// <param name="preheader">Preheader - will be displayed in e-mail preview</param>
        /// <param name="buttonUrl">Url of the button</param>
        /// <param name="buttonText">Text of the button</param>
        /// <returns>Template with replaced macros</returns>
        string GetBasicTemplate(string recipient, string title, string text, string preheader , string buttonUrl, string buttonText);
    }
}
