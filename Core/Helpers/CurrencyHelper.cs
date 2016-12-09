
namespace Core.Helpers
{
    public static class CurrencyHelper
    {
        /// <summary>
        /// Displays value (e.g. $300 or 5000Kč)
        /// </summary>
        /// <param name="value">Amount</param>
        /// <param name="currencyName">Currency name: Kč, $, €</param>
        /// <param name="showSignOnLeft">Indicates if sign should be on the left or right side</param>
        /// <returns></returns>
        public static string DisplayCurrencyValue(double value, string currencyName, bool showSignOnLeft)
        {
            if (showSignOnLeft)
            {
                return $"{currencyName}{StringHelper.FormatNumber(value)}";
            }
            return $"{StringHelper.FormatNumber(value)}{currencyName}";
        }
    }
}
