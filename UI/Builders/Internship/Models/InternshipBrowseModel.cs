using Common.Helpers;
using Common.Helpers.Internship;
using System;
using System.Linq;

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
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string AmountType { get; set; }
        public int MinDurationMonths { get; set; }
        public int MinDurationDays { get; set; }
        public int MinDurationWeeks { get; set; }
        public int MaxDurationMonths { get; set; }
        public int MaxDurationDays { get; set; }
        public int MaxDurationWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public string MinDurationType { get; set; }
        public string MaxDurationType { get; set; }
        public string MinDurationTypeDisplay
        {
            get
            {
                var minDuration = InternshipHelper.GetInternshipDurations().Where(m => m.DurationValue.Equals(MinDurationType)).FirstOrDefault();

                return minDuration == null ? null : minDuration.DurationName;
            }
        }
        public string MaxDurationTypeDisplay
        {
            get
            {
                var maxDuration = InternshipHelper.GetInternshipDurations().Where(m => m.DurationValue.Equals(MaxDurationType)).FirstOrDefault();

                return maxDuration == null ? null : maxDuration.DurationName;
            }
        }
        public int MinDurationValue
        {
            get
            {
                var minDurationEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(MinDurationType);

                if (minDurationEnum == InternshipDurationTypeEnum.Days)
                {
                    return MinDurationDays;
                }
                else if (minDurationEnum == InternshipDurationTypeEnum.Weeks)
                {
                    return MinDurationWeeks;
                }
                else if (minDurationEnum == InternshipDurationTypeEnum.Months)
                {
                    return MinDurationMonths;
                }
                return 0;
            }
        }
        public int MaxDurationValue
        {
            get
            {
                var maxDurationEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(MaxDurationType);

                if (maxDurationEnum == InternshipDurationTypeEnum.Days)
                {
                    return MaxDurationDays;
                }
                else if (maxDurationEnum == InternshipDurationTypeEnum.Weeks)
                {
                    return MaxDurationWeeks;
                }
                else if (maxDurationEnum == InternshipDurationTypeEnum.Months)
                {
                    return MaxDurationMonths;
                }
                return 0;
            }
        }
    }
}
