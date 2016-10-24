
namespace UI.Helpers
{
    public static class CountryHelper
    {
        public static string GetCountryIcon(string countryIconClass)
        {
            if (string.IsNullOrEmpty(countryIconClass))
            {
                return "<i class=\"flag icon\"></i>";
            }

            return $"<i class=\"{countryIconClass.Trim()} flag\"></i>";
        }
    }
}
