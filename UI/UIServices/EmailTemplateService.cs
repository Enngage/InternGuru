using Core.Config;
using System;
using System.IO;
using System.Collections.Generic;
using UI.UIServices.Models;

namespace UI.UIServices
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private const string EMAIL_TEMPLATE_FOLDER = "EmailTemplates";

        private string ServerEmailTemplateFolderPath
        {
            get
            {
                return SystemConfig.ServerRootPath + EMAIL_TEMPLATE_FOLDER + "\\";
            }
        }
        public string GetTemplateHTML(string fileName)
        {
            // construct file path
            var templatePath = ServerEmailTemplateFolderPath + fileName;

            // get file
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Soubor {templatePath} nebyl nalezen.");
            }

            // read html
            return File.ReadAllText(templatePath);
        }

        public string GetTemplateHTML(string fileName, IEnumerable<MacroReplacement> replacements)
        {
            return ReplaceMacros(GetTemplateHTML(fileName), replacements);
        }

        public string ReplaceMacros(string text, IEnumerable<MacroReplacement> replacements)
        {
            if (string.IsNullOrEmpty(text) || replacements == null)
            {
                return null;
            }

            foreach (var replacement in replacements)
            {
               text = text.Replace("{" + replacement.MacroName + "}", replacement.Value);
            }

            return text;
        }
    }
}
