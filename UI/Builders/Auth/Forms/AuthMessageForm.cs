using System.ComponentModel.DataAnnotations;
using UI.Base;

namespace UI.Builders.Auth.Forms
{
    public class AuthMessageForm : BaseForm
    {
        [Required(ErrorMessage = "Nelze odeslat prázdnou zprávu")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Nevalidní příjemce zprávy")]
        public string RecipientApplicationUserId { get; set; }
        public int CompanyID { get; set; }
    }
}
