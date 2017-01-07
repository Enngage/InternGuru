using Core.Config;
using System.IO;
using System.Collections.Generic;
using UI.UIServices.Models;

namespace UI.UIServices
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private const string EmailTemplateFolder = "EmailTemplates";

        private string ServerEmailTemplateFolderPath
        {
            get
            {
                return SystemConfig.ServerRootPath + EmailTemplateFolder + "\\";
            }
        }
        public string GetTemplateHtml(string fileName)
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

        public string GetTemplateHtml(string fileName, IEnumerable<MacroReplacement> replacements)
        {
            return ReplaceMacros(GetTemplateHtml(fileName), replacements);
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
