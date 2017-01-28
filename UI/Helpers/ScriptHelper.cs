using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Core.Project;
using UI.Base;

namespace UI.Helpers
{
    public class ScriptHelper : HelperBase
    {

        public ScriptHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Registers CSS on home page
        /// </summary>
        /// <example>HtmlHelper.RequireCSS("stylesheet/main.min")</example>
        /// <param name="path">Path to CSS file without ".css"</param>
        /// <param name="priority">Priority</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>Script on the home page</returns>
        public string RequireCss(string path, int priority = 1, string htmlAttributes = null)
        {
            var cssPath = path;

            cssPath = $"{cssPath}.css";

            // add version hash
            cssPath += $"?v={VersionInfo.Version}";

            var requiredCss = HttpContext.Current.Items["RequiredCSS"] as List<ResourceInclude>;
            if (requiredCss == null) HttpContext.Current.Items["RequiredCSS"] = requiredCss = new List<ResourceInclude>();
            if (requiredCss.All(i => i.Path != cssPath))
            {
                requiredCss.Add(new ResourceInclude()
                {
                    Path = cssPath,
                    Priority = priority,
                    HtmlAttributes = htmlAttributes,
                    UseAsync = false,
                });
            }
            return null;
        }

        /// <summary>
        /// Renders CSS stored via "RequireCSS" method
        /// </summary>
        /// <returns>CSS scripts</returns>
        public IHtmlString EmitRequiredCss()
        {
            var requiredCss = HttpContext.Current.Items["RequiredCSS"] as List<ResourceInclude>;
            if (requiredCss == null) return null;
            var sb = new StringBuilder();
            foreach (var item in requiredCss.OrderByDescending(i => i.Priority))
            {
                sb.AppendFormat("<link {0} href=\"{1}\" rel=\"stylesheet\" />", item.HtmlAttributes, item.Path);
            }
            return WebViewPage.Html.Raw(sb.ToString());
        }

        /// <summary>
        /// Registers script on home page
        /// </summary>
        /// <example>HtmlHelper.RequireScript("scripts/channel/file")</example>
        /// <param name="path">Path to JS file without ".js"</param>
        /// <param name="priority">Priority</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <param name="includeVersion">Indicates if version query string will be included</param>
        /// <param name="useAsync">Indicates if async load will be used</param>
        /// <param name="includeExtension">Indicates if Extension will be added to script</param>
        /// <returns>Script on the home page</returns>
        public string RequireScript(string path, int priority = 1, string htmlAttributes = null, bool includeVersion = true, bool useAsync = false, bool includeExtension = true)
        {
            var scriptPath = path;

            if (includeExtension)
            {
                scriptPath = $"{scriptPath}.js";
            }

            // add version hash
            if (includeVersion)
            {
                scriptPath += $"?v={VersionInfo.Version}";
            }

            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) HttpContext.Current.Items["RequiredScripts"] = requiredScripts = new List<ResourceInclude>();
            if (requiredScripts.All(i => i.Path != scriptPath))
            {
                requiredScripts.Add(new ResourceInclude()
                {
                    Path = scriptPath,
                    Priority = priority,
                    HtmlAttributes = htmlAttributes,
                    UseAsync = useAsync,
                });
            }
            return null;
        }

        /// <summary>
        /// Renders scripts stored via "RequireScript" method
        /// </summary>
        /// <returns>Scripts html</returns>
        public IHtmlString EmitRequiredScripts()
        {
            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) return null;
            var sb = new StringBuilder();
            foreach (var item in requiredScripts.OrderByDescending(i => i.Priority))
            {
                if (item.UseAsync)
                {
                    sb.AppendFormat("<script async defer {0} src=\"{1}\"></script>\n", item.HtmlAttributes, item.Path);
                }
                else
                {
                    sb.AppendFormat("<script {0} src=\"{1}\" type=\"text/javascript\"></script>\n", item.HtmlAttributes, item.Path);
                }
            }
            return WebViewPage.Html.Raw(sb.ToString());
        }

        /// <summary>
        /// Representation of resource
        /// </summary>
        public class ResourceInclude
        {
            public string Path { get; set; }
            public int Priority { get; set; }
            public string HtmlAttributes { get; set; }
            public bool UseAsync { get; set; }
        }
    }
}
