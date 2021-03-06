﻿using Core.Helpers.Internship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using UI.Attributes;
using UI.Base;
using UI.Builders.Auth.Models;
using UI.Helpers;

namespace UI.Builders.Auth.Forms
{
    public class AuthAddEditInternshipForm : BaseForm
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Název pozice není vyplněn")]
        [MaxLength(60, ErrorMessage = "Maximální délka názvu stáže je 60 znaků")]
        public string Title { get; set; }
        [AllowHtml]
        public string Requirements { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Popis stáže není vyplněn")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Město není vyplněno")]
        public string City { get; set; }

        [Required(ErrorMessage = "Stát není vyplněn")]
        public int CountryID { get; set; }

        public string IsActive { get; set; }
        public string IsPaid{ get; set; }

        [ValidInteger(ErrorMessage = "Odměna musí být celé číslo")]
        public int Amount { get; set; }

        public int CurrencyID { get; set; }
        public int AmountTypeID { get; set; }

        [Required(ErrorMessage = "Chybí datum možného nástupu do stáže")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Chybí minimální délka trvání stáže")]
        public int MinDuration { get; set; }

        public int MinDurationTypeID { get; set; }

        [Required(ErrorMessage = "Chybí maximální délka trvání stáže")]
        public int MaxDuration { get; set; }

        public int MaxDurationTypeID { get; set; }

        [Required(ErrorMessage = "Kategorie není vyplněna")]
        public int InternshipCategoryID { get; set; }

        public string HasFlexibleHours { get; set; }
        public string WorkingHours { get; set; }
        public string MinDurationTypeCodeName { get; set; }
        public string MaxDurationTypeCodeName { get; set; }
        public string Languages { get; set; }

        [Required(ErrorMessage = "Chybí minimální požadované vzdělání")]
        public int MinEducationTypeID { get; set; }

        [Required(ErrorMessage = "Status studenta není vyplněn")]
        public int StudentStatusOptionID { get; set; }
        public int? QuestionnaireID { get; set; }
        public string HideAmount { get; set; }
        public bool GetHideAmount()
        {
            return InputHelper.GetCheckboxValueStatic(HideAmount, false);
        }

        public IEnumerable<AuthQuestionnaireModel> Questionnaires { get; set; }
        public IEnumerable<AuthCountryModel> Countries { get; set; }
        public IEnumerable<AuthCurrencyModel> Currencies { get; set; }
        public IEnumerable<AuthInternshipAmountType> AmountTypes { get; set; }
        public IEnumerable<AuthInternshipDurationType> DurationTypes { get; set; }
        public IEnumerable<AuthInternshipCategoryModel> InternshipCategories { get; set; }
        public IEnumerable<AuthInternshipLanguageModel> AllLanguages { get; set; }
        public IEnumerable<AuthInternshipStudentStatusOptionModel> StudentStatusOptions { get; set; }
        public IEnumerable<AuthInternshipEducationTypeModel> EducationTypes { get; set; }     

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
        /// Indicates if internship has just been created
        /// </summary>
        public bool IsNewlyCreatedInternship { get; set; }


        /// <summary>
        /// Indicates whether the model represents existing internship (based on ID)
        /// </summary>
        public bool IsExistingInternship => ID != 0;

        public bool GetHasFlexibleHours()
        {
            return InputHelper.GetCheckboxValueStatic(HasFlexibleHours, false);
        }

        public bool GetIsPaid()
        {
            return InputHelper.GetCheckboxValueStatic(IsPaid, false);
        }

        public bool GetIsActive()
        {
            return InputHelper.GetCheckboxValueStatic(IsActive, false);
        }
    }
}
