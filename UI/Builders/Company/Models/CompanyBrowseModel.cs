
using System;
using UI.Helpers;

namespace UI.Builders.Company.Models
{
    public class CompanyBrowseModel
    {
        public int ID { get; set; }
        public Guid CompanyGuid { get; set; }
        public string CodeName { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public int InternshipCount { get; set; }
        public int ThesesCount { get; set; }
        public string Url { get; set; }
        public string BannerImageUrl { get; set; }
        public string LogoImageUrl { get; set; }
        public string UrlToInternships { get; set; }
        public string UrlToTheses { get; set; }
        public string CountryIcon { get; set; }
        public string PluralInternshipsCountWord { get; set; }
        public string PluralThesesCountWord { get; set; }

        public string CountryIconHtml => CountryHelper.GetCountryIconStatic(CountryIcon);
    }
}
