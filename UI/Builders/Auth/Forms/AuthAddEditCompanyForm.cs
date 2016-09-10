using Common.Helpers.Country;
using Common.Helpers.Internship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Forms
{
    public class AuthAddEditInternshipForm : BaseForm
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vyplňte název pozice")]
        public string Title { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Vyplňte popis pozice")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Zadejte město výkonu stáže")]
        public string City { get; set; }

        [Required(ErrorMessage = "Zadejte stát výkonu stáže")]
        public string Country { get; set; }

        public string IsPaid{ get; set; }

        public int Amount { get; set; }

        public string Currency { get; set; }

        public string AmountType { get; set; }

        [Required(ErrorMessage = "Zadejte datum možného nástupu do stáže")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Zvolte minimální délku trvání stáže")]
        public int MinDuration { get; set; }

        public string MinDurationType { get; set; }

        [Required(ErrorMessage = "Zvolte maximální délku trvání stáže")]
        public int MaxDuration { get; set; }

        public string MaxDurationType { get; set; }

        [Required(ErrorMessage = "Zvolte kategorii stáže")]
        public int InternshipCategoryID { get; set; }

        public IEnumerable<CountryModel> Countries { get; set; }
        public IEnumerable<string> Currencies { get; set; }
        public IEnumerable<string> AmountTypes { get; set; }
        public IEnumerable<InternshipDurationTypeModel> DurationTypes { get; set; }
        public IEnumerable<AuthInternshipCategoryModel> InternshipCategories { get; set; }

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

        public bool GetIsPaid()
        {
            return true;
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
