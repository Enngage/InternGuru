using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UI.Base;
using Core.Context;
using Core.Services;
using UI.Builders.Internship.Views;
using UI.Builders.Internship.Models;
using PagedList.EntityFramework;
using UI.Builders.Services;

namespace UI.Builders.Internship
{
    public class InternshipBuilder : BaseBuilder
    {

        #region Constructor

        public InternshipBuilder(
            IAppContext appContext,
            IServicesLoader servicesLoader,
            IInternshipService internshipService,
            IIdentityService identityService,
            ILogService logService,
            ICompanyService companyService) : base(
                appContext,
                servicesLoader)
        {
        }

        #endregion

        #region Actions

        public async Task<InternshipBrowseView> BuildBrowseViewAsync(int? page)
        {
            int cacheMinutes = 60;
            int pageSize = 30;
            int pageNumber = (page ?? 1);

            var cacheSetup = CacheService.GetSetup<InternshipBrowseModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Internship.KeyCreateAny<Entity.Internship>(),
                Entity.Internship.KeyDeleteAny<Entity.Internship>(),
                Entity.Internship.KeyUpdateAny<Entity.Internship>()
            };

            var internshipsQuery = Services.InternshipService.GetAll()
                .OrderByDescending(m => m.Created)
                .Select(m => new InternshipBrowseModel()
                {
                    ID = m.ID,
                    Created = m.Created,
                    Amount = m.Amount,
                    AmountType = m.AmountType,
                    City = m.City,
                    CompanyID = m.CompanyID,
                    CompanyName = m.Company.CompanyName,
                    Country = m.Country,
                    Currency = m.Currency,
                    DurationInDays = m.MinDurationInDays,
                    DurationInMonths = m.MinDurationInMonths,
                    DurationInWeeks = m.MinDurationInWeeks,
                    InternshipCategoryID = m.InternshipCategoryID,
                    InternshipCategoryName = m.InternshipCategory.Name,
                    IsPaid = m.IsPaid,
                    StartDate = m.StartDate,
                    Title = m.Title
                });

            var internships = await CacheService.GetOrSetAsync(async () => await internshipsQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);

            return new InternshipBrowseView()
            {
                Internships = internships
            };
        }

        #endregion

    }
}
