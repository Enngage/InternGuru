using Common.Helpers.Country;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class CountryHelper
    {
        /// <summary>
        /// Gets collection of allowed countries used for Company/Internships
        /// </summary>
        /// <returns>Collection of countries</returns>
        public static IEnumerable<CountryModel> GetCountries()
        {
            return new List<CountryModel>()
            {
              new CountryModel()
              {
                  CultureCodeName = "Czech",
                  CultureName = "Česká republika",
                  FlagIcon = "cz"
              },
              new CountryModel()
              {
                  CultureCodeName = "Slovakia",
                  CultureName = "Slovensko",
                  FlagIcon = "sk"
              },
              new CountryModel()
              {
                  CultureCodeName = "Germany",
                  CultureName = "Germany",
                  FlagIcon = "de"
              },
              new CountryModel()
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
        public static CountryModel GetCountry(string cultureCodeName)
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
