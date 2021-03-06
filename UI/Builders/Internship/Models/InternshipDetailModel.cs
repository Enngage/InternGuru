﻿using Core.Helpers.Internship;
using System;
using System.Collections.Generic;

namespace UI.Builders.Internship.Models
{
    public class InternshipDetailModel
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public int InternshipCategoryID { get; set; }
        public string InternshipCategoryName { get; set; }
        public string Title { get; set; }
        public string Requirements { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsPaid { get; set; }
        public bool HideAmount { get; set; }
        public double Amount { get; set; }
        public string CurrencyName { get; set; }
        public bool CurrencyShowSignOnLeft { get; set; }
        public string CurrencyCode { get; set; }
        public string AmountTypeCodeName { get; set; }
        public string AmountTypeName { get; set; }
        public InternshipDurationTypeEnum MinDurationType { get; set; }
        public int MinDurationInDefaultValue { get; set; }
        public string MinDurationTypeCodeName { get; set; }
        public int MinDurationInMonths { get; set; }
        public int MinDurationInDays { get; set; }
        public int MinDurationInWeeks { get; set; }
        public InternshipDurationTypeEnum MaxDurationType { get; set; }
        public int MaxDurationInDefaultValue { get; set; }
        public string MaxDurationTypeCodeName { get; set; }
        public int MaxDurationInMonths { get; set; }
        public int MaxDurationInDays { get; set; }
        public int MaxDurationInWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool HasFlexibleHours { get; set; }
        public string WorkingHours { get; set; }
        public string Languages { get; set; }
        public int MinEducationTypeID { get; set; }
        public int StudentStatusOptionID { get; set; }
        public DateTime ActiveSince { get; set; }

        // activity stats
        public InternshipActivityStats ActivityStats { get; set; }

        // model properties
        public InternshipDetailCompanyModel Company { get; set; }
        public InternshipEducationTypeModel MinEducation { get; set; }
        public InternshipStudentStatusOptionModel StudentStatusOption { get; set; }

        /// <summary>
        /// Languages that are required for the internship
        /// </summary>
        public IEnumerable<InternshipLanguageModel> RequiredLanguages { get; set; }

        // virtual properties
        public int MinDurationTypeID { get; set; }
        public int MaxDurationTypeID { get; set; }
    }
}
