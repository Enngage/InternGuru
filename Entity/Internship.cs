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

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

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
        public string MinDurationType { get; set; }
        [Required]
        public int MinDurationInMonths { get; set; }
        [Required]
        public int MinDurationInDays { get; set; }
        [Required]
        public int MinDurationInWeeks { get; set; }
        public string MaxDurationType { get; set; }
        [Required]
        public int MaxDurationInMonths { get; set; }
        [Required]
        public int MaxDurationInDays { get; set; }
        [Required]
        public int MaxDurationInWeeks { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }


        #region Virtual

        public ApplicationUser ApplicationUser { get; set; }
        public InternshipCategory InternshipCategory { get; set; }
        public Company Company { get; set; }

        #endregion

        #region Entity abstract members

        public override object GetObjectID()
        {
            return ID;
        }

        public override string GetCodeName()
        {
            return ID.ToString();
        }

        #endregion
    }
}