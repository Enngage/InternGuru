using System;
using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class InputHelper : HelperBase
    {
        public InputHelper(WebViewPage webViewPage) : base(webViewPage) { }

        public readonly string ValueOfEnabledCheckbox = ValueOfEnabledCheckboxStatic;

        /// <summary>
        /// Converts checkbox value of Semantic UI to proper bool value
        /// See: http://semantic-ui.com/modules/checkbox.html
        /// </summary>
        /// <param name="value">Value of semantic UI checkbox</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>True if checkbox is checked, default value if checkbox is invalid</returns>
        public bool GetCheckboxValue(string value, bool defaultValue)
        {
            return GetCheckboxValueStatic(value, defaultValue);
        }

        #region Static members

        public static readonly string ValueOfEnabledCheckboxStatic = "on";

        /// <summary>
        /// Converts checkbox value of Semantic UI to proper bool value
        /// See: http://semantic-ui.com/modules/checkbox.html
        /// </summary>
        /// <param name="value">Value of semantic UI checkbox</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>True if checkbox is checked, default value if checkbox is invalid</returns>
        public static bool GetCheckboxValueStatic(string value, bool defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
