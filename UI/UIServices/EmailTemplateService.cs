using Core.Config;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UI.Emails;
using UI.UIServices.Models;

namespace UI.UIServices
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly string _emailTemplateFolder = AppConfig.EmailTemplatesFolder;
        private readonly string _emailTemplatesExtension = ".html";

        public string ServerEmailTemplateFolderPath => SystemConfig.ServerRootPath + _emailTemplateFolder + "\\";

        public string GetTemplateHtml(string fileName)
        {
            // construct file path
            var templatePath = ServerEmailTemplateFolderPath + fileName + _emailTemplatesExtension;

            // get file
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Soubor {templatePath} nebyl nalezen.");
            }

            // read html
            return ReplaceMacros(File.ReadAllText(templatePath), GetDefaultMacroReplacemens());
        }

        public string GetTemplateHtml(string fileName, IEnumerable<MacroReplacement> replacements)
        {
            var defaultReplacements = replacements?.Concat(GetDefaultMacroReplacemens()) ?? GetDefaultMacroReplacemens();

            return ReplaceMacros(GetTemplateHtml(fileName), defaultReplacements);
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

        public string GetTemplateHtml(EmailTypeEnum emailType)
        {
            return GetTemplateHtml(emailType, null);
        }

        public string GetTemplateHtml(EmailTypeEnum emailType, IEnumerable<MacroReplacement> replacements)
        {
            var fileName = emailType.ToString();
            var defaultReplacements = replacements?.Concat(GetDefaultMacroReplacemens()) ?? GetDefaultMacroReplacemens();

            return GetTemplateHtml(fileName, defaultReplacements);
        }

        public IEnumerable<MacroReplacement> GetDefaultMacroReplacemens()
        {
            return new List<MacroReplacement>()
            {
                new MacroReplacement()
                {
                    MacroName = "SiteName",
                    Value = AppConfig.SiteName
                },
                new MacroReplacement()
                {
                    MacroName = "Address",
                    Value = AppConfig.Address
                },
                new MacroReplacement()
                {
                    MacroName = "City",
                    Value = AppConfig.City
                },
                new MacroReplacement()
                {
                    MacroName = "Country",
                    Value = AppConfig.Country
                },
                  new MacroReplacement()
                {
                    MacroName = "WebUrl",
                    Value = AppConfig.WebUrl
                },
            };
        }
    }
}
