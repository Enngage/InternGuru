using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using UI.Base;

namespace UI.Builders.Auth.Forms
{
    public class AuthAddEditThesis : BaseForm
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
        public bool IsPaid { get; set; }
        [Required(ErrorMessage = "Zvolte typ práce")]
        public int ThesisTypeID { get; set; }
        public int Amount { get; set; }
        public int CurrencyID { get; set; }

        /// <summary>
        /// Indicates whether the model represents existing thesis (based on ID)
        /// </summary>
        public bool IsExistingThesisO
        {
            get
            {
                return ID != 0;
            }
        }

    }
}
