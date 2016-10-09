using Common.Helpers.Internship;
using System;

namespace UI.Builders.Internship.Models
{
    public class InternshipDetailModel
    {
        public int ID { get; set; }
        public int InternshipCategoryID { get; set; }
        public string InternshipCategoryName { get; set; }
        public string Title { get; set; }
        public string Requirements { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string AmountTypeCodeName { get; set; }
        public string AmountTypeName { get; set; }
        public InternshipDurationTypeEnum MinDurationType { get; set; }
        public int MinDurationInMonths { get; set; }
        public int MinDurationInDays { get; set; }
        public int MinDurationInWeeks { get; set; }
        public InternshipDurationTypeEnum MaxDurationType { get; set; }
        public int MaxDurationInMonths { get; set; }
        public int MaxDurationInDays { get; set; }
        public int MaxDurationInWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool HasFlexibleHours { get; set; }
        public string WorkingHours { get; set; }
        public InternshipDetailCompanyModel Company { get; set; }

        // virtual properties
        public int MinDurationDefault { get; set; }
        public int MaxDurationDefault { get; set; }
    }
}
