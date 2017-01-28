using System.ComponentModel.DataAnnotations;

namespace UI.Builders.Account.Forms
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Prázdný e-mail")]
        [EmailAddress(ErrorMessage = "Nevalidní e-mailová adresa")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Prázdné heslo")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
