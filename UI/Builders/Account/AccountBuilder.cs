using UI.Abstract;
using Core.Context;
using Cache;
using System.Text;
using Common.Config;

namespace UI.Builders.Account
{
    public class AccountBuilder : BuilderAbstract
    {

        #region Services


        #endregion

        #region Constructor

        public AccountBuilder(
            IAppContext appContext,
            ICacheService cacheService
            ) : base(appContext, cacheService)
        {
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
            emailHtml.AppendFormat("<p>Váš {0} tým</p>", AppConfig.SiteName);

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
            emailHtml.AppendFormat("<p>Váš {0} tým</p>", AppConfig.SiteName);

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
