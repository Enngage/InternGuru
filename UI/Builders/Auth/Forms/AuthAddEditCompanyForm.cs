﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using UI.Base;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Forms
{
    public class AuthAddEditCompanyForm : BaseForm
    {
        public int ID { get; set; }
        public Guid CompanyGuid { get; set; }

        [Required(ErrorMessage = "Název firmy nemůže být prázdný")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Kategorie musí být vyplněna")]
        public int CompanyCategoryID { get; set; }

        [Required(ErrorMessage = "Popis firmy nemůže být prázdný")]
        [MaxLength(500, ErrorMessage = "Maximální délka popisu firmy je 500 znaků.")]
        public string LongDescription { get; set; }

        [Required(ErrorMessage = "Adresa firmy nemůže být prázdná")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Město nemůže být prázdné")]
        public string City { get; set; }

        [Required(ErrorMessage = "Stát nemůže být prázdný")]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public float Lat { get; set; }

        public float Lng { get; set; }

        [Required(ErrorMessage = "URL webu není vyplněna")]
        public string Web { get; set; }

        public string Twitter { get; set; }

        public string LinkedIn { get; set; }

        public string Facebook { get; set; }

        [Required(ErrorMessage = "Velikost firmy není vyplněna")]
        public int CompanySizeID { get; set; }
        public string CompanySizeName { get; set; }

        // Files
        public HttpPostedFileBase Logo { get; set; }
        public HttpPostedFileBase Banner { get; set; }

        public IEnumerable<AuthCompanySize> CompanySizes { get; set; }
        public IEnumerable<AuthCountryModel> Countries { get; set; }
        public IEnumerable<AuthCompanyCategoryModel> CompanyCategories { get; set; }

        /// <summary>
        /// Indicates whether the model represents existing company (based on ID)
        /// </summary>
        public bool IsExistingCompany => ID != 0;

        /// <summary>
        /// Indicates if the the company was just registered
        /// </summary>
        public bool IsNewlyRegisteredCompany { get; set; }
    }
}
