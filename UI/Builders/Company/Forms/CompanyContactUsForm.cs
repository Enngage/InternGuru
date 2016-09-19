using System.ComponentModel.DataAnnotations;
using UI.Base;

namespace UI.Builders.Company.Forms
{
    public class CompanyContactUsForm : BaseForm
    {
        [Required(ErrorMessage = "Nelze odeslat prázdnou zprávu")]
        public string Message { get; set; }
    }
}
