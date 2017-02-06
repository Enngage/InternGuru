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
    public class AuthAddEditThesisForm : BaseForm
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vyplňte název práce")]
        [MaxLength(250, ErrorMessage = "Název práce může mít maximálně 250 znaků")]
        public string ThesisName { get; set; }
        [Required(ErrorMessage = "Vyplňte popis práce")]
        [AllowHtml]
        public string Description { get; set; }
        [Required(ErrorMessage = "Zvolte kategorii")]
        public int InternshipCategoryID { get; set; }
        public string IsPaid { get; set; }
        public string IsActive { get; set; }
        [Required(ErrorMessage = "Zvolte typ práce")]
        public int ThesisTypeID { get; set; }
        [ValidInteger(ErrorMessage = "Odměna musí být celé číslo")]
        public int? Amount { get; set; }
        public int CurrencyID { get; set; }

        public IEnumerable<AuthThesisTypeModel> ThesisTypes { get; set; }
        public IEnumerable<AuthCurrencyModel> Currencies { get; set; }
        public IEnumerable<AuthInternshipCategoryModel> Categories { get; set; }

        /// <summary>
        /// Indicates if thesis has just been created
        /// </summary>
        public bool IsNewlyCreatedThesis { get; set; }

        public bool GetIsActive()
        {
            return InputHelper.GetCheckboxValueStatic(IsActive, false);
        }

        public bool GetIsPaid()
        {
            return InputHelper.GetCheckboxValueStatic(IsPaid, false);
        }

        /// <summary>
        /// Indicates whether the model represents existing thesis (based on ID)
        /// </summary>
        public bool IsExistingThesis => ID != 0;
    }
}
