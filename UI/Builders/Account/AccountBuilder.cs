using System;
using UI.Base;
using Core.Config;
using UI.Builders.Account.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Builders.Account
{
    public class AccountBuilder : BaseBuilder
    {

        #region Services


        #endregion

        #region Constructor

        public AccountBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public RegisterViewModel BuildRegisterView()
        {
            return new RegisterViewModel()
            {
                RegisterForm = new Forms.RegisterForm()
            };
        }

        public LoginViewModel BuildLoginView()
        {
            return new LoginViewModel()
            {
                LoginForm = new Forms.LoginForm()
            };
        }

        public ForgotPasswordViewModel BuildForgotPasswordView()
        {
            return new ForgotPasswordViewModel()
            {
                ForgotForm = new Forms.ForgotPasswordForm()
            };
        }

        public ExternalLoginConfirmationViewModel BuildExternalLoginConfirmationView()
        {
            return new ExternalLoginConfirmationViewModel()
            {
                ConfirmationForm = new Forms.ExternalLoginConfirmationForm()
            };
        }

        public ResetPasswordViewModel BuildResetPasswordView()
        {
            return new ResetPasswordViewModel()
            {
                ResetPasswordForm = new Forms.ResetPasswordForm()
            };
        }

        #endregion

        #region Methods

        public AccountEmail GetForgottenPasswordEmail(string callbackUrl)
        {
            var emailTemplate = Services.EmailTemplateService.GetBasicTemplate(null, "Obnova hesla",
        "Pro tento účet byla vyžádána obnova hesla. Změnu hesla můžete provést odkazem níže. V opačném případě tento e-mail ignorujte.",
        "Obnovení hesla", callbackUrl,
        "Obnovit heslo");

            return new AccountEmail()
            {
                Body = emailTemplate,
                Subject = $"Obnova hesla - {AppConfig.SiteName}"
            };
        }

        public AccountEmail GetRegistrationEmail(string callbackUrl)
        {
            var emailTemplate = Services.EmailTemplateService.GetBasicTemplate(null, "Registrace",
                "Pro dokončení registrace potvrďte Váš e-mail pomocí odkazu níže.",
                "Potvrzení e-mailové adresy a dokončení registrace", callbackUrl,
                "Potvrdit e-mail");

            return new AccountEmail()
            {
                Body = emailTemplate,
                Subject = $"Registrace - {AppConfig.SiteName}"
            };
        }


        /// <summary>
        /// Logs login error
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        public void LogLoginError(string errorMessage)
        {
            Services.LogService.LogException(new Exception(errorMessage, null));
        }
        #endregion

        #region E-mail model

        public class AccountEmail
        {
            public string Body { get; set; }
            public string Subject { get; set; }
        }

        #endregion

    }
}
