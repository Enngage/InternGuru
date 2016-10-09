
namespace UI.Helpers
{
    public static class CountryHelper
    {
        public static string GetCountryIcon(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return "<i class=\"flag icon\"></i>";
            }

            return $"<i class=\"{className} flag\"></i>";
        }
    }
}
