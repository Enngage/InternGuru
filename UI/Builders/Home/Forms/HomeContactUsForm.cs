using System.ComponentModel.DataAnnotations;
using UI.Base;

namespace UI.Builders.Home.Forms
{
    public class HomeContactUsForm : BaseForm
    {
        [Required(ErrorMessage = "Nelze odeslat prázdnou zprávu")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Zadej e-mail, na který se Ti můžeme ozvat zpátky")]
        [EmailAddress(ErrorMessage = "Zadej validní e-mailovou adresu")]
        public string Email { get; set; }
    }
}
