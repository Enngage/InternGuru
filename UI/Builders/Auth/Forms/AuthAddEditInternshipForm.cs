using Core.Helpers.Internship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using UI.Attributes;
using UI.Base;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Forms
{
    public class AuthAddEditInternshipForm : BaseForm
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vyplňte název pozice")]
        [MaxLength(60, ErrorMessage = "Maximální délka názvu stáže je 60 znaků")]
        public string Title { get; set; }
        [AllowHtml]
        public string Requirements { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Vyplňte popis stáže")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Zadejte město výkonu stáže")]
        public string City { get; set; }

        [Required(ErrorMessage = "Zadejte stát výkonu stáže")]
        public int CountryID { get; set; }

        public string IsActive { get; set; }
        public string IsPaid{ get; set; }

        [ValidInteger(ErrorMessage = "Odměna musí být celé číslo")]
        public int Amount { get; set; }

        public int CurrencyID { get; set; }
        public int AmountTypeID { get; set; }

        [Required(ErrorMessage = "Zadejte datum možného nástupu do stáže")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Zvolte minimální délku trvání stáže")]
        public int MinDuration { get; set; }

        public int MinDurationTypeID { get; set; }

        [Required(ErrorMessage = "Zvolte maximální délku trvání stáže")]
        public int MaxDuration { get; set; }

        public int MaxDurationTypeID { get; set; }

        [Required(ErrorMessage = "Zvolte kategorii stáže")]
        public int InternshipCategoryID { get; set; }

        public string HasFlexibleHours { get; set; }
        public string WorkingHours { get; set; }
        public string MinDurationTypeCodeName { get; set; }
        public string MaxDurationTypeCodeName { get; set; }
        public string Languages { get; set; }

        [Required(ErrorMessage = "Zadejte možnosti home office")]
        public int HomeOfficeOptionID { get; set; }

        [Required(ErrorMessage = "Zadejte požadovaný status studenta")]
        public int StudentStatusOptionID { get; set; }

        public IEnumerable<AuthCountryModel> Countries { get; set; }
        public IEnumerable<AuthCurrencyModel> Currencies { get; set; }
        public IEnumerable<AuthInternshipAmountType> AmountTypes { get; set; }
        public IEnumerable<AuthInternshipDurationType> DurationTypes { get; set; }
        public IEnumerable<AuthInternshipCategoryModel> InternshipCategories { get; set; }
        public IEnumerable<AuthInternshipLanguageModel> AllLanguages { get; set; }
        public IEnumerable<AuthInternshipStudentStatusOptionModel> StudentStatusOptions { get; set; }
        public IEnumerable<AuthInternshipHomeOfficeOptionModel> HomeOfficeOptions { get; set; }     

        // duration values
        public InternshipDurationTypeEnum MinDurationTypeEnum { get; set; }
        public InternshipDurationTypeEnum MaxDurationTypeEnum { get; set; }

        public int MinDurationInDays { get; set; }
        public int MinDurationInWeeks { get; set; }
        public int MinDurationInMonths { get; set; }

        public int MaxDurationInDays { get; set; }
        public int MaxDurationInWeeks { get; set; }
        public int MaxDurationInMonths { get; set; }


        /// <summary>
        /// Indicates whether the model represents existing internship (based on ID)
        /// </summary>
        public bool IsExistingInternship => ID != 0;

        public bool GetHasFlexibleHours()
        {
            return !string.IsNullOrEmpty(HasFlexibleHours) && HasFlexibleHours.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsPaid()
        {
            if (string.IsNullOrEmpty(IsPaid))
            {
                return false;
            }

            return IsPaid.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsActive()
        {
            return !string.IsNullOrEmpty(IsActive) && IsActive.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

       

    }
}
