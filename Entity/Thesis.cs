using Core.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class Thesis : IEntity
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(250)]
        [Index]
        public string CodeName { get; set; }
        [Required]
        [MaxLength(250)]
        public string ThesisName { get; set; }
        public string Description { get; set; }
        public int InternshipCategoryID { get; set; }
        public int CompanyID { get; set; }
        public string ApplicationUserId { get; set; }
        public int Amount { get; set; }
        public int CurrencyID { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public int ThesisTypeID { get; set; }
        public bool IsActive { get; set; }

        #region Virtual

        [ForeignKey("CurrencyID")]
        public Currency Currency { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("InternshipCategoryID")]
        public InternshipCategory InternshipCategory { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        [ForeignKey("ThesisTypeID")]
        public ThesisType ThesisType { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(ThesisName);
        }

        #endregion
    }
}