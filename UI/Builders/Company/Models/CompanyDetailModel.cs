using System.Collections.Generic;

namespace UI.Builders.Company.Models
{
    public class CompanyDetailModel
    {
        public string CompanyName { get; set; }
        public int YearFounded { get; set; }
        public string PublicEmail { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int CountryID { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string Web { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public int CompanySizeID { get; set; }
        public string CompanySizeName { get; set; }
        public int CompanyCategoryID { get; set; }
        public string CompanyCategoryName { get; set; }
        public int ID { get; set; }
        public IEnumerable<CompanyDetailInternshipModel> Internships { get; set; }
        public IEnumerable<CompanyThesisModel> Theses { get; set; }
    }
}
