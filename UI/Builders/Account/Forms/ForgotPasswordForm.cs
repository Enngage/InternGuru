﻿using System.ComponentModel.DataAnnotations;

namespace UI.Builders.Account.Forms
{
    public class ForgotPasswordForm
    {
        [Required(ErrorMessage = "Vyplň e-mail")]
        [EmailAddress( ErrorMessage = "Nevalidní e-mailová adresa")]
        public string Email { get; set; }
    }
}
