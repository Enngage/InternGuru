using Common.Helpers.Currency;
using System.Linq;
using System.Collections.Generic;

namespace Common.Helpers
{
    public static class CurrencyHelper
    {
        /// <summary>
        /// Gets collection of available currencies
        /// </summary>
        /// <returns>available currencies</returns>
        public static IEnumerable<CurrencyModel> GetCurrencies()
        {
            return new List<CurrencyModel>()
            {
                new CurrencyModel() {
                    CurrencyCodeName = "CZK",
                    CurrencyName = "Kč",
                    ShowSignOnLeft = false
                },
                 new CurrencyModel() {
                    CurrencyCodeName = "EUR",
                    CurrencyName = "€",
                    ShowSignOnLeft = false
                },
                  new CurrencyModel() {
                    CurrencyCodeName = "USD",
                    CurrencyName = "$",
                    ShowSignOnLeft = true
                },
            };
        }

        /// <summary>
        /// Gets currency or null
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public static CurrencyModel GetCurrency(string codeName)
        {
            return GetCurrencies().Where(m => m.CurrencyCodeName.Equals(codeName, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}
