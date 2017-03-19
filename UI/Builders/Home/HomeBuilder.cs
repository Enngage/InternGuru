using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Config;
using Core.Helpers;
using Core.Helpers.Internship;
using Entity.Base;
using Service.Extensions;
using UI.Base;
using UI.Builders.Home.Forms;
using UI.Builders.Home.Models;
using UI.Builders.Home.Views;
using UI.Builders.Internship;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Builders.Home
{
    public class HomeBuilder : BaseBuilder
    {
        #region Extra builder

        private InternshipBuilder _internshipBuilder;

        #endregion

        #region Constructor

        public HomeBuilder(ISystemContext systemContext, IServicesLoader servicesLoader, InternshipBuilder internshipBuilder) : base(systemContext, servicesLoader)
        {
            _internshipBuilder = internshipBuilder;
        }

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

        public async Task<HomeIndexView> BuildIndexViewAsync()
        {
            return new HomeIndexView()
            {
                Internships = await GetHomeInternshipsAsync(),
                Categories = await _internshipBuilder.GetInternshipCategoriesAsync()
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

        #region Helper methods

        private async Task<IList<HomeInternshipListingModel>> GetHomeInternshipsAsync()
        {
            var topN = 10;

            var internshipQuery = Services.InternshipService.GetAll()
                .OnlyActive()
                .OrderByDescending(m => m.ActiveSince)
                .Take(topN)
                .Select(m => new HomeInternshipListingModel()
                {
                    InternshipID = m.ID,
                    InternshipTitle = m.Title,
                    CategoryName = m.InternshipCategory.Name,
                    CodeName = m.CodeName,
                    StartDate = m.StartDate,
                    IsPaid = m.IsPaid,
                    MaxDurationDays = m.MaxDurationInDays,
                    MaxDurationMonths = m.MaxDurationInMonths,
                    MaxDurationWeeks = m.MaxDurationInWeeks,
                    MinDurationDays = m.MinDurationInDays,
                    MinDurationWeeks = m.MinDurationInWeeks,
                    MinDurationMonths = m.MinDurationInMonths,
                    MaxDurationTypeCodeName = m.MaxDurationType.CodeName,
                    MinDurationTypeCodeName = m.MinDurationType.CodeName,
                    City = m.City
                });

            var cacheSetup = Services.CacheService.GetSetup<HomeInternshipListingModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var internships = await Services.CacheService.GetOrSetAsync(async () => await internshipQuery.ToListAsync(), cacheSetup);

            // initialize additional values
            foreach (var internship in internships)
            {
                // duration types
                internship.MinDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MinDurationTypeCodeName);
                internship.MaxDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MaxDurationTypeCodeName);

                internship.MinDurationDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MinDurationType, internship.MinDurationDays, internship.MinDurationWeeks, internship.MinDurationMonths);
                internship.MaxDurationDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MaxDurationType, internship.MaxDurationDays, internship.MaxDurationWeeks, internship.MaxDurationMonths);
            }

            return internships;
        }

        #endregion
    }
}
