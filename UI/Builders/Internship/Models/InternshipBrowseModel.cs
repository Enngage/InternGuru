using Common.Helpers.Internship;
using System;

namespace UI.Builders.Internship.Models
{
    public class InternshipBrowseModel
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public int InternshipCategoryID { get; set; }
        public string InternshipCategoryName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public DateTime Created { get; set; }
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string AmountTypeName { get; set; }
        public string AmountTypeCode { get; set; }
        public int MinDurationMonths { get; set; }
        public int MinDurationDays { get; set; }
        public int MinDurationWeeks { get; set; }
        public int MaxDurationMonths { get; set; }
        public int MaxDurationDays { get; set; }
        public int MaxDurationWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public InternshipDurationTypeEnum MinDurationType { get; set; }
        public InternshipDurationTypeEnum MaxDurationType { get; set; }
    }
}
