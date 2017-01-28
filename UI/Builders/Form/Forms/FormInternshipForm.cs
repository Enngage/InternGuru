using System.ComponentModel.DataAnnotations;
using UI.Base;

namespace UI.Builders.Form.Forms
{
    public class FormInternshipForm : BaseForm
    {
        [Required(ErrorMessage = "Nelze odeslat prázdnou zprávu")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Nevalidní stáž")]
        public int InternshipID { get; set; }

    }
}
