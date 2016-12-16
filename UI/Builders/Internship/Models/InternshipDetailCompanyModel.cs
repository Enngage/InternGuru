
using System;

namespace UI.Builders.Internship.Models
{
    public class InternshipDetailCompanyModel
    {
        public string CompanyName { get; set; }
        public string CompanyCodeName { get; set; }
        public int CompanyID { get; set; }
        public Guid CompanyGuid { get; set; }
        public int YearFounded { get; set; }
        public string PublicEmail { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string Web { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string CompanySizeName { get; set; }
    }
}
