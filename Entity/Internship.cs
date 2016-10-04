using Common.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Internship : IEntity
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(250)]
        [Index]
        public string CodeName { get; set; }
        
        [ForeignKey("InternshipCategory")]
        public int InternshipCategoryID { get; set; }

        [ForeignKey("Company")]
        public int CompanyID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        public string Requirements { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
        [MaxLength(50)]
        public string Currency { get; set; }
        [MaxLength(50)]
        public string AmountType { get; set; }
        [Required]
        public string MinDurationType { get; set; }
        [Required]
        public int MinDurationInMonths { get; set; }
        [Required]
        public int MinDurationInDays { get; set; }
        [Required]
        public int MinDurationInWeeks { get; set; }
        [Required]
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
        public bool HasFlexibleHours { get; set; }
        [MaxLength(250)]
        public string WorkingHours { get; set; }

        #region Virtual

        public ApplicationUser ApplicationUser { get; set; }
        public InternshipCategory InternshipCategory { get; set; }
        public Company Company { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(Title);
        }

        #endregion
    }
}