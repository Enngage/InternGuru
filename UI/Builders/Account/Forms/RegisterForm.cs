using System.ComponentModel.DataAnnotations;

namespace UI.Builders.Account.Forms
{
    public class RegisterForm
    {
        [Required(ErrorMessage = "Vyplňte e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vyplňte heslo")]
        [StringLength(100, ErrorMessage = "Heslo musí mít alespoň {2} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }
}
