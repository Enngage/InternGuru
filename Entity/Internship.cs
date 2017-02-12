using Core.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class Internship : IEntity, IEntityWithTimeStamp, IEntityWithUserStamp, IEntityWithRestrictedAccess
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(250)]
        [Index]
        public string CodeName { get; set; }
        [Required]
        public int InternshipCategoryID { get; set; }
        [Required]
        public int CompanyID { get; set; }
        [Required]
        public string CreatedByApplicationUserId { get; set; }
        [Required]
        public string UpdatedByApplicationUserId { get; set; }
        [Required]
        public int CountryID { get; set; }
        [Required]
        public int AmountTypeID { get; set; }
        [Required]
        public int CurrencyID { get; set; }
        [Required]
        public int MinDurationTypeID { get; set; }
        [Required]
        public int MaxDurationTypeID { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        public string Requirements { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public int Amount { get; set; }
        [Required]
        public int MinDurationInMonths { get; set; }
        [Required]
        public int MinDurationInDays { get; set; }
        [Required]
        public int MinDurationInWeeks { get; set; }
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
        [MaxLength(500)]
        public string Languages { get; set; }
        public int HomeOfficeOptionID { get; set; }
        public int StudentStatusOptionID { get; set; }
        public DateTime ActiveSince { get; set; }

        #region Virtual

        [ForeignKey("MinDurationTypeID")]
        public InternshipDurationType MinDurationType { get; set; }
        [ForeignKey("MaxnDurationTypeID")]
        public InternshipDurationType MaxDurationType { get; set; }
        [ForeignKey("CurrencyID")]
        public Currency Currency { get; set; }
        [ForeignKey("AmountTypeID")]
        public InternshipAmountType AmountType { get; set; }
        [ForeignKey("CountryID")]
        public Country Country { get; set; }
        [ForeignKey("CreatedByApplicationUserId")]
        public ApplicationUser CreatedByApplicationUser { get; set; }
        [ForeignKey("UpdatedByApplicationUserId")]
        public ApplicationUser UpdatedByApplicationUser { get; set; }
        [ForeignKey("InternshipCategoryID")]
        public InternshipCategory InternshipCategory { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        [ForeignKey("HomeOfficeOptionID")]
        public HomeOfficeOption HomeOfficeOption { get; set; }
        [ForeignKey("StudentStatusOptionID")]
        public StudentStatusOption StudentStatusOption { get; set; }

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