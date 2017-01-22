using System;
using Core.Config;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Emails;
using UI.UIServices.Models;

namespace UI.UIServices
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly string _emailTemplateFolder = AppConfig.EmailTemplatesFolder;
        private readonly string _emailTemplatesExtension = ".html";
        private readonly UrlHelper _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

        /// <summary>
        /// Url to the main site without actions/params
        /// </summary>
        private string SiteUrl => _urlHelper.RequestContext.HttpContext.Request.Url?.Scheme + "://" + _urlHelper.RequestContext.HttpContext.Request.Url?.Authority;

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
                    Value = SiteUrl
                },
                new MacroReplacement()
                {
                    MacroName = "Year",
                    Value = DateTime.Now.Year.ToString()
                },
            };
        }

        public string GetBasicTemplate(string recipient, string title, string text, string preheader, string buttonUrl, string buttonText)
        {
           

            return GetTemplateHtml(EmailTypeEnum.BasicEmail, new List<MacroReplacement>()
            {
                new MacroReplacement()
                {
                    MacroName = "Title",
                    Value = title
                },
                new MacroReplacement()
                {
                    MacroName = "RecipientEmail",
                    Value = recipient
                },
                new MacroReplacement()
                {
                    MacroName = "MessageText",
                    Value = text
                },
                new MacroReplacement()
                {
                    MacroName = "Preheader",
                    Value = preheader
                },
                new MacroReplacement()
                {
                    MacroName = "ButtonText",
                    Value = buttonText
                },
                new MacroReplacement()
                {
                    MacroName = "ButtonUrl",
                    Value = buttonUrl
                },
            });
        }
    }
}
