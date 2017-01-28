using System.Linq;
using System.Threading.Tasks;
using Core.Config;
using UI.Base;
using UI.Builders.Home.Forms;
using UI.Builders.Home.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Builders.Home
{
    public class HomeBuilder : BaseBuilder
    {
        #region Constructor

        public HomeBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public HomeContactUsView BuildContactUsView()
        {
            return new HomeContactUsView()
            {
                ContactUsForm = new HomeContactUsForm()
            };
        }

        public HomeContactUsView BuildContactUsView(HomeContactUsForm form)
        {
            return new HomeContactUsView()
            {
                ContactUsForm = form ?? new HomeContactUsForm()
            };
        }

        #endregion

        #region Methods

        public async Task SubmitContactUsForm(HomeContactUsForm form)
        {
            // get basic template
            var basicTemplate = Services.EmailTemplateService.GetBasicTemplate(AppConfig.MainContactEmail,
                $"Zpráva od - {form.Email}", form.Message, form.Message, null, "Na web");

            // just send e-mail
            await Services.EmailService.SendEmailAsync(AppConfig.MainContactEmail,
                $"Kontaktní formulář - {AppConfig.SiteName}", basicTemplate);
        }

        #endregion
    }
}
