using System.ComponentModel.DataAnnotations;

namespace UI.Builders.Account.Forms
{
    public class RegisterForm
    {
        [Required(ErrorMessage = "Zapomněl jsi vyplnit e-mail")]
        [EmailAddress]
        public string Email { get; set;}

        [Required(ErrorMessage = "Prázdné heslo se nehodí")]
        [StringLength(100, ErrorMessage = "Použij alespoň {2} znaků pro heslo", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují")]
        public string ConfirmPassword { get; set; }
    }
}
