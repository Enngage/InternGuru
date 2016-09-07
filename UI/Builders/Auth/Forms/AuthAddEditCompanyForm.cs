
using Common.Helpers.Country;
using Common.Helpers.Internship;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Forms
{
    public class AuthAddEditCompanyForm : BaseForm
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Název firmy nemůže být prázdný")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Zadejte kategorii")]
        public int CompanyCategoryID { get; set; }
        [Required(ErrorMessage = "Rok založení nemůže být prázdný")]
        public int YearFounded { get; set; }
        [Required(ErrorMessage = "E-mail nemůže být prázdný")]
        [EmailAddress(ErrorMessage = "Nevalidní e-mailová adresa")]
        public string PublicEmail { get; set; }
        [Required(ErrorMessage = "Krátký popis nemůže být prázdný")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Dlouhý popis nemůže být prázdný")]
        [AllowHtml]
        public string LongDescription { get; set; }
        [Required(ErrorMessage = "Adresa firmy nemůže být prázdná")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Město nemůže být prázdné")]
        public string City { get; set; }
        [Required(ErrorMessage = "Stát nemůže být prázdný")]
        public string Country { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        [Required(ErrorMessage = "Web nemůže být prázdný")]
        public string Web { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        [Required(ErrorMessage = "Velikost firmy nemůže být prázdná")]
        public int CompanySize { get; set; }

        public IEnumerable<InternshipCompanySizeModel> AllowedCompanySizes { get; set; }
        public IEnumerable<CountryModel> Countries { get; set; }
        public IEnumerable<AuthCompanyCategoryModel> CompanyCategories { get; set; }

        /// <summary>
        /// Indicates whether the model represents existing company (based on ID)
        /// </summary>
        public bool IsExistingCompany
        {
            get
            {
                return ID != 0;
            }
        }

    }
}
