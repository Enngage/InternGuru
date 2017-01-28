using System.ComponentModel.DataAnnotations;

namespace UI.Builders.Account.Forms
{
    public class RegisterForm
    {
        [Required(ErrorMessage = "E-mail nemůže být prázdný")]
        [EmailAddress( ErrorMessage = "E-mailová adresa není validní")]
        public string Email { get; set;}

        [Required(ErrorMessage = "Není vyplněno heslo")]
        [StringLength(100, ErrorMessage = "Heslo musí mít alespoň {2} znaků", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují")]
        public string ConfirmPassword { get; set; }
    }
}
