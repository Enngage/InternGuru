using System;
using System.Collections.Generic;

namespace Common.Helpers
{
    public static class CurrencyHelper
    {
        /// <summary>
        /// Gets collection of available currencies
        /// </summary>
        /// <returns>available currencies</returns>
        public static IEnumerable<string> GetCurrencies()
        {
            return new List<String>()
            {
                "CZK",
                "EUR",
                "USD"
            };
        }
    }
}
