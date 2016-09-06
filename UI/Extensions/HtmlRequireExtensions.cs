using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Common.Project;

namespace UI.Extensions
{
    /* http://stackoverflow.com/questions/5355427/populate-a-razor-section-from-a-partial */
    public static class HtmlRequireExtensions
    {
        /// <summary>
        /// Registers CSS on home page
        /// </summary>
        /// <example>HtmlHelper.RequireCSS("stylesheet/main.min")</example>
        /// <param name="html">Html helper</param>
        /// <param name="path">Path to CSS file without ".css"</param>
        /// <param name="priority">Priority</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <returns>Script on the home page</returns>
        public static string RequireCSS(this HtmlHelper html, string path, int priority = 1, string htmlAttributes = null)
        {
            string CSSPath = path;

            CSSPath = String.Format("{0}.css", CSSPath);

            // add version hash
            CSSPath += String.Format("?v={0}", VersionInfo.Version);

            var requiredCSS = HttpContext.Current.Items["RequiredCSS"] as List<ResourceInclude>;
            if (requiredCSS == null) HttpContext.Current.Items["RequiredCSS"] = requiredCSS = new List<ResourceInclude>();
            if (!requiredCSS.Any(i => i.Path == CSSPath))
            {
                requiredCSS.Add(new ResourceInclude()
                {
                    Path = CSSPath,
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
        /// <returns>Html</returns>
        public static HtmlString EmitRequiredCSS(this HtmlHelper html)
        {
            var requiredCSS = HttpContext.Current.Items["RequiredCSS"] as List<ResourceInclude>;
            if (requiredCSS == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (var item in requiredCSS.OrderByDescending(i => i.Priority))
            {
                sb.AppendFormat("<link {0} href=\"{1}\" rel=\"stylesheet\" />", item.HtmlAttributes, item.Path);
            }
            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// Registers script on home page
        /// </summary>
        /// <example>HtmlHelper.RequireScript("scripts/channel/file")</example>
        /// <param name="html">Html helper</param>
        /// <param name="path">Path to JS file without ".js"</param>
        /// <param name="priority">Priority</param>
        /// <param name="htmlAttributes">Html attributes</param>
        /// <param name="includeVersion">Indicates if version query string will be included</param>
        /// <param name="useAsync">Indicates if async load will be used</param>
        /// <param name="includeExtension">Indicates if Extension will be added to script</param>
        /// <returns>Script on the home page</returns>
        public static string RequireScript(this HtmlHelper html, string path, int priority = 1, string htmlAttributes = null, bool includeVersion = true, bool useAsync = false, bool includeExtension = true)
        {
            string scriptPath = path;

            if (includeExtension)
            {
                scriptPath = String.Format("{0}.js", scriptPath);
            }

            // add version hash
            if (includeVersion)
            {
                scriptPath += String.Format("?v={0}", VersionInfo.Version);
            }

            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) HttpContext.Current.Items["RequiredScripts"] = requiredScripts = new List<ResourceInclude>();
            if (!requiredScripts.Any(i => i.Path == scriptPath))
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
        /// <returns>Html</returns>
        public static HtmlString EmitRequiredScripts(this HtmlHelper html)
        {
            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) return null;
            StringBuilder sb = new StringBuilder();
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
            return new HtmlString(sb.ToString());
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
