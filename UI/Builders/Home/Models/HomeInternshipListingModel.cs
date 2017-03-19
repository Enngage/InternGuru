using System;
using Core.Helpers.Internship;

namespace UI.Builders.Home.Models
{
    public class HomeInternshipListingModel
    {
        public int InternshipID { get; set; }
        public string City { get; set; }
        public string CodeName { get; set; }
        public string InternshipTitle { get; set; }
        public string CategoryName { get; set; }
        public bool IsPaid { get; set; }
        public int MinDurationMonths { get; set; }
        public int MinDurationDays { get; set; }
        public int MinDurationWeeks { get; set; }
        public int MaxDurationMonths { get; set; }
        public int MaxDurationDays { get; set; }
        public int MaxDurationWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public string MinDurationTypeCodeName { get; set; }
        public string MaxDurationTypeCodeName { get; set; }
        public InternshipDurationTypeEnum MinDurationType { get; set; }
        public InternshipDurationTypeEnum MaxDurationType { get; set; }
        public int MinDurationDefaultValue { get; set; }
        public int MaxDurationDefaultValue { get; set; }
    }
}
