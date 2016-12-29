using Core.Helpers;
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
        public int InternshipCategoryID { get; set; }
        public int CompanyID { get; set; }
        public string ApplicationUserId { get; set; }
        public int CountryID { get; set; }
        public int AmountTypeID { get; set; }
        public int CurrencyID { get; set; }
        public int MinDurationTypeID { get; set; }
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
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
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