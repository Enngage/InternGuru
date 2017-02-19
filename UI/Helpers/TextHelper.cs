using System.Web;
using System.Web.Mvc;
using Core.Helpers;
using UI.Base;

namespace UI.Helpers
{
    public class TextHelper : HelperBase
    {
        public TextHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Displays formatted price for internship
        /// </summary>
        /// <param name="hideAmount">Indicates if amount will be shown</param>
        /// <param name="value">Amount value</param>
        /// <param name="currencyName">Currency name</param>
        /// <param name="showSignOnLeft">Indicates on which side the currency sign will appear</param>
        /// <returns></returns>
        public string GetInternshipAmount(bool hideAmount, double value, string currencyName, bool showSignOnLeft)
        {
            return hideAmount ? "Ano" : DisplayCurrencyValue(value, currencyName, showSignOnLeft);
        }

        /// <summary>
        /// Displays formatted price for thesis
        /// </summary>
        /// <param name="hideAmount">Indicates if amount will be shown</param>
        /// <param name="value">Amount value</param>
        /// <param name="currencyName">Currency name</param>
        /// <param name="showSignOnLeft">Indicates on which side the currency sign will appear</param>
        /// <returns></returns>
        public string GetThesisAmount(bool hideAmount, double value, string currencyName, bool showSignOnLeft)
        {
            return hideAmount ? "Ano" : DisplayCurrencyValue(value, currencyName, showSignOnLeft);
        }

        /// <summary>
        /// Formats number. 
        /// Returns "9,593,938" instead of "9593938"
        /// </summary>
        /// <param name="number">Number to format</param>
        /// <returns>Formatted number as string</returns>
        public string FormatNumber(int number)
        {
            return StringHelper.FormatNumber(number);
        }

        /// <summary>
        /// Displays value (e.g. $300 or 5000Kč)
        /// </summary>
        /// <param name="value">Amount</param>
        /// <param name="currencyName">Currency name: Kč, $, €</param>
        /// <param name="showSignOnLeft">Indicates if sign should be on the left or right side</param>
        /// <returns>Formatter value</returns>
        public string DisplayCurrencyValue(double value, string currencyName, bool showSignOnLeft)
        {
            return showSignOnLeft ? $"{currencyName}{StringHelper.FormatNumber(value)}" : $"{StringHelper.FormatNumber(value)}{currencyName}";
        }
    }
}
