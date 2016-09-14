using Core.Context;
using Core.Services;
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

        #region Constructor

        public CompanyBuilder(
            IAppContext appContext,
            IServicesLoader servicesLoader)
            : base(
                appContext,
                servicesLoader)
        {
        }

        #endregion

        #region Actions

        public async Task<CompanyIndexView> BuildIndexViewAsync(int? page)
        {
            int cacheMinutes = 60;
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var cacheSetup = CacheService.GetSetup<CompanyBrowseModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Company.KeyCreateAny<Entity.Company>(),
                Entity.Company.KeyDeleteAny<Entity.Company>(),
                Entity.Company.KeyUpdateAny<Entity.Company>()
            };

            var companiesQuery = Services.CompanyService.GetAll()
                .OrderBy(m => m.CompanyName)
                .Select(m => new CompanyBrowseModel()
                {
                    City = m.City,
                    CompanyName = m.CompanyName,
                    Country = m.Country,
                    ID = m.ID,
                    InternshipCount = m.Internships.Count()
                });

            var companies = await CacheService.GetOrSetAsync(async () => await companiesQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);

            return new CompanyIndexView()
            {
                Companies = companies
            };

        }

        public async Task<CompanyDetailView> BuildDetailViewAsync(int companyID)
        {
            int cacheMinutes = 60;

            var cacheSetup = CacheService.GetSetup<CompanyDetailModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Company.KeyUpdate<Entity.Company>(companyID),
                Entity.Company.KeyDelete<Entity.Company>(companyID),
            };

            var companyQuery = Services.CompanyService.GetAll()
                .Where(m => m.ID == companyID)
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

        public async Task<IList<CompanyBrowseModel>> GetMoreCompaniesAsync(int? page)
        {
            int pageSize = 10;

            // TODO 
            var list = new List<CompanyBrowseModel>();

            // generate random companies
            for (int i = 0; i <= pageSize; i++)
            {
                list.Add(new CompanyBrowseModel()
                {
                    City = "city",
                    CompanyName = "company_" + i,
                    Country = "country",
                    ID = i,
                    InternshipCount = i * 3 - i * 2
                });
            }

            return list;
        }

        #endregion
    }
}
