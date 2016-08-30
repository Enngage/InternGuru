using Cache;
using Core.Context;
using Core.Services;
using System;
using System.Data.Entity;
using PagedList.EntityFramework;
using System.Linq;
using System.Collections.Generic;
using UI.Abstract;
using UI.Builders.Company.Models;
using UI.Builders.Company.Views;
using System.Threading.Tasks;

namespace UI.Builders.Company
{
    public class CompanyBuilder : BuilderAbstract
    {
        #region Services

        ICompanyService companyService;

        #endregion

        #region Constructor

        public CompanyBuilder(IAppContext appContext, ICacheService cacheService, ICompanyService companyService) : base(appContext, cacheService)
        {
            this.companyService = companyService;
        }

        #endregion

        #region Actions

        public async Task<CompanyIndexView> BuildIndexViewAsync(int? page)
        {
            int cacheMinutes = 60;
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var cacheSetup = CacheService.GetSetup<CompanyModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Company.KeyCreateAny<Entity.Company>(),
                Entity.Company.KeyDeleteAny<Entity.Company>(),
                Entity.Company.KeyUpdateAny<Entity.Company>()
            };

            var companiesQuery = companyService.GetAll()
                .OrderBy(m => m.CompanyName)
                .Select(m => new CompanyModel()
                {
                    City = m.City,
                    CompanyName = m.CompanyName,
                    Country = m.Country,
                    ID = m.ID,
                    InternshipCount = m.Internships.Count()
                });

            var companies = await CacheService.GetOrSetAsync(async () => await companiesQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);

            var tease = CacheService.GetAllSetups();

            return new CompanyIndexView()
            {
                Companies = companies
            };

        }

        #endregion
    }
}
