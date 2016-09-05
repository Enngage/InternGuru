using System;

namespace UI.Builders.Internship.Models
{
    public class InternshipBrowseModel
    {
        public int ID { get; set; }
        public int InternshipCategoryID { get; set; }
        public string InternshipCategoryName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string AmountType { get; set; }
        public int DurationInMonths { get; set; }
        public int DurationInDays { get; set; }
        public int DurationInWeeks { get; set; }
        public DateTime StartDate { get; set; }
    }
}
