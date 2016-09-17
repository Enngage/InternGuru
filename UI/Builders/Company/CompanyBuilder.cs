using Core.Context;
using System.Data.Entity;
using PagedList.EntityFramework;
using System.Linq;
using System.Collections.Generic;
using UI.Base;
using UI.Builders.Company.Models;
using UI.Builders.Company.Views;
using System.Threading.Tasks;
using UI.Builders.Services;

namespace UI.Builders.Company
{
    public class CompanyBuilder : BaseBuilder
    {

        #region Variables

        private readonly int browseCompaniesPageSize = 4;

        #endregion

        #region Constructor

        public CompanyBuilder(IAppContext appContext,IServicesLoader servicesLoader): base(appContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<CompanyIndexView> BuildIndexViewAsync(int? page)
        {
            int pageNumber = (page ?? 1);

            return new CompanyIndexView()
            {
                Companies = await GetCompaniesAsync(pageNumber, null)
            };

        }

        public async Task<CompanyDetailView> BuildDetailViewAsync(string codeName)
        {
            int cacheMinutes = 60;

            var cacheSetup = CacheService.GetSetup<CompanyDetailModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Company.KeyUpdateCodeName<Entity.Company>(codeName),
                Entity.Company.KeyDeleteCodeName<Entity.Company>(codeName),
            };

            var companyQuery = Services.CompanyService.GetAll()
                .Where(m => m.CodeName == codeName)
                .Take(1)
                .Select(m => new CompanyDetailModel()
                {
                    Address = m.Address,
                    CompanyCategoryID = m.CompanyCategoryID,
                    CompanyCategoryName = m.CompanyCategory.Name,
                    CompanySize = m.CompanySize,
                    Facebook = m.Facebook,
                    Lat = m.Lat,
                    LinkedIn = m.LinkedIn,
                    Lng = m.Lng,
                    LongDescription = m.LongDescription,
                    PublicEmail = m.PublicEmail,
                    Twitter = m.Twitter,
                    Web = m.Web,
                    YearFounded = m.YearFounded,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    Country = m.Country,
                    ID = m.ID,
                    Internships = m.Internships
                        .Select(s => new CompanyDetailInternshipModel()
                        {
                            ID = s.ID,
                            Title = s.Title
                        })
                });

            var company = await CacheService.GetOrSetAsync(async () => await companyQuery.FirstOrDefaultAsync(), cacheSetup);

            if (company == null)
            {
                return null;
            }

            return new CompanyDetailView()
            {
                Company = company,
            };
        }

        #endregion

        #region Web API methods

        public Task<IList<CompanyBrowseModel>> GetMoreCompaniesAsync(int pageNumber, string search)
        {
            return GetCompaniesAsync(pageNumber, search);
        }

        #endregion

        #region Helper methods

        private async Task<IList<CompanyBrowseModel>> GetCompaniesAsync(int pageNumber, string search)
        {
            int cacheMinutes = 60;

            // get companies from db/cache
            var cacheSetup = CacheService.GetSetup<CompanyBrowseModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Company.KeyCreateAny<Entity.Company>(),
                Entity.Company.KeyDeleteAny<Entity.Company>(),
                Entity.Company.KeyUpdateAny<Entity.Company>()
            };

            // cache different pages separately
            cacheSetup.PageNumber = pageNumber;

            var companiesQuery = Services.CompanyService.GetAll()
                .OrderBy(m => m.CompanyName)
                .Select(m => new CompanyBrowseModel()
                {
                    City = m.City,
                    CompanyName = m.CompanyName,
                    Country = m.Country,
                    ID = m.ID,
                    InternshipCount = m.Internships.Count(),
                    CodeName = m.CodeName,
                });

            // search
            if (!string.IsNullOrEmpty(search))
            {
                companiesQuery = companiesQuery.Where(m => m.CompanyName.Contains(search));

                var companies = await companiesQuery.ToPagedListAsync(pageNumber, browseCompaniesPageSize);

                return companies.ToList();
            }
            // not search
            else
            {
                var companies = await CacheService.GetOrSetAsync(async () => await companiesQuery.ToPagedListAsync(pageNumber, browseCompaniesPageSize), cacheSetup);

                return companies.ToList();
            }
        }

        #endregion
    }
}
