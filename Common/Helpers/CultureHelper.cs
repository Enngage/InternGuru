using Common.Helpers.Culture;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class CultureHelper
    {
        /// <summary>
        /// Gets collection of allowed countries used for Company/Internships
        /// </summary>
        /// <returns>Collection of countries</returns>
        public static IEnumerable<CultureModel> GetCountries()
        {
            return new List<CultureModel>()
            {
              new CultureModel()
              {
                  CultureCodeName = "Czech",
                  CultureName = "Česká republika",
                  FlagIcon = "cz"
              },
              new CultureModel()
              {
                  CultureCodeName = "Slovakia",
                  CultureName = "Slovensko",
                  FlagIcon = "sk"
              },
              new CultureModel()
              {
                  CultureCodeName = "Germany",
                  CultureName = "Germany",
                  FlagIcon = "de"
              },
              new CultureModel()
              {
                  CultureCodeName = "UK",
                  CultureName = "United Kingdom",
                  FlagIcon = "gb"
              }
           };
        }

        /// <summary>
        /// Gets country based on culture code name
        /// </summary>
        /// <param name="cultureCodeName">Code name of culture</param>
        /// <returns>Country or null</returns>
        public static CultureModel GetCountry(string cultureCodeName)
        {
            return GetCountries()
                .Where(m => m.CultureCodeName.Equals(cultureCodeName, System.StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets html code of flag for specified country
        /// http://semantic-ui.com/elements/flag.html
        /// </summary>
        /// <param name="cultureCodeName">Code name of country</param>
        /// <returns>Country flag or default one if country is not found/returns>
        public static string GetCountryIcon(string cultureCodeName)
        {
            var country = GetCountry(cultureCodeName);

            if (country == null)
            {
                return string.Format("<i class=\"flag\"></i>");
            }

            return string.Format("<i class=\"{0} flag\"></i>", country.FlagIcon);
        }
    }
}
