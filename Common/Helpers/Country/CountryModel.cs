
namespace Common.Helpers.Country
{
    public class CountryModel
    {
        public string CountryName { get; set; }
        public string CoutryCodeName { get; set; }
        public string FlagIcon { get; set; }

        public string FlagIconHtml
        {
            get
            {
                return CountryHelper.GetCountryIcon(CoutryCodeName);
            }
        }
    }
}
