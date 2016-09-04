using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Internship : EntityAbstract
    {
        public int ID { get; set; }
        
        [ForeignKey("InternshipCategory")]
        public int InternshipCategoryID { get; set; }

        [ForeignKey("Company")]
        public int CompanyID { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string AmountType { get; set; }
        [Required]
        public int DurationInMonths { get; set; }
        [Required]
        public int DurationInDays { get; set; }
        [Required]
        public int DurationInWeeks { get; set; }
        [Required]
        public DateTime StartDate { get; set; }


        #region Virtual

        public InternshipCategory InternshipCategory { get; set; }
        public Company Company { get; set; }

        #endregion
    }
}