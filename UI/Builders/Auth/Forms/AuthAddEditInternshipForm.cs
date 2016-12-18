using Core.Helpers.Internship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
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

        public double Amount { get; set; }

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

        public IEnumerable<AuthCountryModel> Countries { get; set; }
        public IEnumerable<AuthCurrencyModel> Currencies { get; set; }
        public IEnumerable<AuthInternshipAmountType> AmountTypes { get; set; }
        public IEnumerable<AuthInternshipDurationType> DurationTypes { get; set; }
        public IEnumerable<AuthInternshipCategoryModel> InternshipCategories { get; set; }
        public IEnumerable<AuthInternshipLanguageModel> AllLanguages { get; set; }

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
        public bool IsExistingInternship
        {
            get
            {
                return ID != 0;
            }
        }

        public bool GetHasFlexibleHours()
        {
            if (String.IsNullOrEmpty(HasFlexibleHours))
            {
                return false;
            }

            return HasFlexibleHours.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsPaid()
        {
            if (String.IsNullOrEmpty(IsPaid))
            {
                return false;
            }

            return IsPaid.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsActive()
        {
            if (String.IsNullOrEmpty(IsActive))
            {
                return false;
            }

            return IsActive.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets duration in weeks from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in weeks</returns>
        public int GetDurationInWeeks(InternshipDurationTypeEnum durationType, int duration)
        {
            if (durationType == InternshipDurationTypeEnum.Weeks)
            {
                // no need to convert
                return duration;
            }

            if (durationType == InternshipDurationTypeEnum.Days)
            {
                // convert Days to Weeks
                return (int)duration / 7;
            }

            if (durationType == InternshipDurationTypeEnum.Months)
            {
                // convert Months to Weeks
                return (int)duration * 4;
            }

            throw new ArgumentException("Invalid duration type");
        }

        /// <summary>
        /// Gets duration in days from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in days</returns>
        public int GetDurationInDays(InternshipDurationTypeEnum durationType, int duration)
        {
            if (durationType == InternshipDurationTypeEnum.Weeks)
            {
                // convert weeks to days
                return duration * 7;
            }

            if (durationType == InternshipDurationTypeEnum.Days)
            {
                // no need to convert
                return duration;
            }

            if (durationType == InternshipDurationTypeEnum.Months)
            {
                // convert Months to Days
                return duration * 30;
            }

            throw new ArgumentException("Invalid duration type");
        }

        /// <summary>
        /// Gets duration in months from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in months</returns>
        public int GetDurationInMonths(InternshipDurationTypeEnum durationType, int duration)
        {
            if (durationType == InternshipDurationTypeEnum.Weeks)
            {
                // convert weeks to monts
                return (int)duration / 4;
            }

            if (durationType == InternshipDurationTypeEnum.Days)
            {
                // convert Days to Months
                return (int)duration / 30;
            }

            if (durationType == InternshipDurationTypeEnum.Months)
            {
                // no need to convert
                return duration;
            }

            throw new ArgumentException("Invalid duration type");
        }

    }
}
