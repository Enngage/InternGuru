using System.ComponentModel.DataAnnotations;
using UI.Base;

namespace UI.Builders.Home.Forms
{
    public class HomeContactUsForm : BaseForm
    {
        [Required(ErrorMessage = "Nelze odeslat prázdnou zprávu")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Zadejte e-mail, na který se Vám můžeme ozvat zpátky")]
        [EmailAddress(ErrorMessage = "Zadejte validní e-mailovou adresu")]
        public string Email { get; set; }
    }
}
