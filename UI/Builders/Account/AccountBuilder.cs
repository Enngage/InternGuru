using UI.Abstract;
using Core.Context;
using Cache;
using System.Text;
using Common.Config;
using UI.Builders.Account.Views;
using Core.Services;

namespace UI.Builders.Account
{
    public class AccountBuilder : BuilderAbstract
    {

        #region Services


        #endregion

        #region Constructor

        public AccountBuilder(
            IAppContext appContext,
            ICacheService cacheService,
            IIdentityService identityService,
            ILogService logService
            ) : base(
                appContext, 
                cacheService, 
                identityService, 
                logService)
        {
        }

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
            var emailHtml = new StringBuilder();
            emailHtml.AppendFormat("<h1>{0}</h1>", AppConfig.SiteName);
            emailHtml.AppendFormat("<p>Pro resetování hesla použijte následující odkaz:<p>");
            emailHtml.AppendFormat("<p><br /><a href=\"{0}\">{0}</a></p>", callbackUrl);
            emailHtml.AppendFormat("<p><br />Děkujeme,</p>");
            emailHtml.AppendFormat("<p>{0} tým</p>", AppConfig.SiteName);

            return new AccountEmail()
            {
                Body = emailHtml.ToString(),
                Subject = "Zapomenuté heslo"
            };
        }

        public AccountEmail GetRegistrationEmail(string callbackUrl)
        {
            var emailHtml = new StringBuilder();
            emailHtml.AppendFormat("<h1>{0}</h1>", AppConfig.SiteName);
            emailHtml.AppendFormat("<p>Pro potvrzení e-mailové adresy použijte následující odkaz:<p>");
            emailHtml.AppendFormat("<p><br /><a href=\"{0}\">{0}</a></p>", callbackUrl);
            emailHtml.AppendFormat("<p><br />Děkujeme,</p>");
            emailHtml.AppendFormat("<p>{0} tým</p>", AppConfig.SiteName);

            return new AccountEmail()
            {
                Body = emailHtml.ToString(),
                Subject = "Potvrzení e-mailové adresy"
            };
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
