
using System.Web.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class CountryHelper : HelperBase
    {
        public CountryHelper(WebViewPage webViewPage) : base(webViewPage) { }

        public string GetCountryIcon(string countryIconClass)
        {
            return GetCountryIconStatic(countryIconClass);
        }

        #region Static methods

        public static string GetCountryIconStatic(string countryIconClass)
        {
            return string.IsNullOrEmpty(countryIconClass) ? "<i class=\"flag icon\"></i>" : $"<i class=\"{countryIconClass.Trim()} flag\"></i>";
        }

        #endregion
    }
}
