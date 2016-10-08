
namespace Common.Helpers.Currency
{
    public class CurrencyModel
    {
        public string CurrencyName { get; set; }
        public string CurrencyCodeName { get; set; }
        public bool ShowSignOnLeft { get; set; }

        /// <summary>
        /// Displays value (e.g. $300 or 5000Kč)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string DisplayValue(double value)
        {
            if (ShowSignOnLeft)
            {
                return $"{CurrencyName}{value}";
            }
            return $"{value}{CurrencyName}";
        }
    }
}
