using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using Entity;
using System.Data.Entity;
using Entity.Base;
using UI.Base;
using Service.Services.Thesis.Enums;
using UI.Builders.Services;
using UI.Builders.Thesis.Views;
using UI.Builders.Thesis.Models;
using UI.Builders.Shared.Models;

namespace UI.Builders.Thesis
{
    public class ThesisBuilder : BaseBuilder
    {

        #region Constructor

        public ThesisBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<ThesisNotFoundView> BuildNotFoundViewAsync()
        {
            return new ThesisNotFoundView()
            {
                LatestTheses = (await GetAllThesisBrowseModelsAsync()).OrderByDescending(m => m.ID).Take(10)
            };
        }

        public async Task<ThesisDetailView> BuildDetailViewAsync(int thesisID)
        {
            var thesis = await GetThesisDetailModelAsync(thesisID);

            if (thesis == null)
            {
                return null;
            }

            return new ThesisDetailView()
            {
                Thesis = thesis
            };
        }

        public async Task<ThesisBrowseView> BuildBrowseViewAsync(string search, int? page, string category)
        {
            var pageSize = 20;
            var pageNumber = (page ?? 1);
            var isSearchQuery = !string.IsNullOrEmpty(search);

            // get all theses from cache
            var theses = await GetAllThesisBrowseModelsAsync();

            if (isSearchQuery)
            {
                theses = theses.Where(m => m.ThesisName.Contains(search));
            }

            // filter by category
            if (!string.IsNullOrEmpty(category))
            {
                var categoryID = await GetCategoryIDAsync(category);
                if (categoryID != 0)
                {
                    theses = theses.Where(m => m.InternshipCategoryID == categoryID);
                }
            }

            return new ThesisBrowseView()
            {
                Theses = theses.ToPagedList(pageNumber, pageSize),
                ThesisCategories = await GetThesisCategoriesAsync()
            };

        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets all browse models from DB or cache
        /// </summary>
        /// <returns>Collection of browse models</returns>
        private async Task<IEnumerable<ThesisBrowseModel>> GetAllThesisBrowseModelsAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<ThesisBrowseView>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Company>(),
                EntityKeys.KeyCreateAny<Entity.Company>(),
                EntityKeys.KeyDeleteAny<Entity.Company>()
            };

            var thesesQuery = Services.ThesisService.GetAll()
                .Where(m => m.IsActive)
                .Select(m => new ThesisBrowseModel()
                {
                    Amount = m.Amount,
                    CodeName = m.CodeName,
                    Created = m.Created,
                    CurrencyID = m.CurrencyID,
                    CurrencyName = m.Currency.CurrencyName,
                    CurrencyShowSignOnLeft = m.Currency.ShowSignOnLeft,
                    InternshipCategoryID = m.InternshipCategoryID,
                    InternshipCategoryName = m.InternshipCategory.Name, 
                    IsPaid = m.IsPaid,
                    ThesisTypeCodeName = m.ThesisType.CodeName, 
                    ID = m.ID,
                    ThesisTypeName = m.ThesisType.Name,
                    ThesisName = m.ThesisName,
                    ThesisTypeID = m.ThesisTypeID,
                    Company = new ThesisDetailCompanyModel()
                    {
                        CompanyID = m.CompanyID,
                        CompanyName = m.Company.CompanyName,
                        CompanyGuid = m.Company.Guid,
                        Address = m.Company.Address,
                        CompanyCodeName = m.Company.CodeName,
                        City = m.Company.City,
                        CountryCode = m.Company.Country.CountryCode,
                        CountryName = m.Company.Country.CountryName,
                        Facebook = m.Company.Facebook,
                        Lat = m.Company.Lat,
                        LinkedIn = m.Company.LinkedIn,
                         Lng = m.Company.Lng,
                         PublicEmail = m.Company.PublicEmail,
                         Twitter = m.Company.Twitter,
                         Web = m.Company.Web,
                         YearFounded = m.Company.YearFounded,
                         CompanySizeName = m.Company.CompanySize.CompanySizeName,
                         LongDescription = m.Company.LongDescription, 
                    },
                })
                .OrderByDescending(m => m.ID);

            var theses = await Services.CacheService.GetOrSet(async () => await thesesQuery.ToListAsync(), cacheSetup);

            // inititialize thesis type name
            foreach (var thesis in theses)
            {
                thesis.ThesisTypeNameConverted = thesis.ThesisTypeCodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", (await GetAllThesisTypesAsync())) : thesis.ThesisTypeName;
            }

            return theses;
        }

        /// <summary>
        /// Gets thesis categories
        /// Only categories with > 0 theses are returned
        /// Categories are ordered by the number of theses
        /// </summary>
        /// <returns>Thesis categories</returns>
        private async Task<IEnumerable<ThesisCategoryModel>> GetThesisCategoriesAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<ThesisCategoryModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
            };

            var categoriesQuery = Services.InternshipCategoryService.GetAll()
                .Select(m => new ThesisCategoryModel()
                {
                    CategoryID = m.ID,
                    CategoryName = m.Name,
                    CodeName = m.CodeName,
                    ThesesCount = m.Theses.Where(s => s.IsActive).Count()
                })
                .Where(m => m.ThesesCount > 0)
                .OrderByDescending(m => m.ThesesCount);

            return await Services.CacheService.GetOrSetAsync(async () => await categoriesQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets ID of category based on given code name or 0 if none is found
        /// </summary>
        /// <param name="categoryCodeName">Code name of internship category</param>
        /// <returns>CategoryID or null</returns>
        private async Task<int> GetCategoryIDAsync(string categoryCodeName)
        {
            if (string.IsNullOrEmpty(categoryCodeName))
            {
                return 0;
            }

            var categoryQuery = Services.InternshipCategoryService.GetAll()
                .Where(m => m.CodeName == categoryCodeName)
                .Select(s => new
                {
                    CategoryID = s.ID
                })
                .Take(1);

            var cacheSetup = Services.CacheService.GetSetup<int>(GetSource());

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<InternshipCategory>(),
                EntityKeys.KeyUpdateAny<InternshipCategory>(),
                EntityKeys.KeyDeleteAny<InternshipCategory>()
            };
            cacheSetup.ObjectStringID = categoryCodeName;

            var category = await Services.CacheService.GetOrSetAsync(async () => await categoryQuery.FirstOrDefaultAsync(), cacheSetup);

            return category == null ? 0 : category.CategoryID;
        }

        /// <summary>
        /// Gets thesis model
        /// </summary>
        /// <param name="thesisID">ThesisID</param>/param>
        /// <returns>Thesis or null if none is found</returns>
        private async Task<ThesisDetailModel> GetThesisDetailModelAsync(int thesisID)
        {
            var thesisQuery = Services.ThesisService.GetSingle(thesisID)
                .Where(m => m.IsActive)
                .Select(m => new ThesisDetailModel()
                {
                    Company = new ThesisDetailCompanyModel()
                    {
                        CompanyCodeName = m.Company.CodeName,
                        CompanyID = m.CompanyID,
                        CompanyName = m.Company.CompanyName,
                        CompanyGuid = m.Company.Guid,
                        Lat = m.Company.Lat,
                        Lng = m.Company.Lng,
                        Address = m.Company.Address,
                        City = m.Company.City,
                        CompanySizeName = m.Company.CompanySize.CompanySizeName,
                        CountryName = m.Company.Country.CountryName,
                        CountryCode = m.Company.Country.CountryCode,
                        Facebook = m.Company.Facebook,
                        LinkedIn = m.Company.LinkedIn,
                        LongDescription = m.Company.LongDescription,
                        PublicEmail = m.Company.PublicEmail,
                        Twitter = m.Company.Twitter,
                        Web = m.Company.Web,
                        YearFounded = m.Company.YearFounded
                    },
                    Amount = m.Amount,
                    CodeName = m.CodeName,
                    CurrencyName = m.Currency.CurrencyName,
                    CurrencyShowSignOnLeft = m.Currency.ShowSignOnLeft,
                    Created = m.Created,
                    Description = m.Description,
                    ID = m.ID,
                    IsPaid = m.IsPaid,
                    CurrencyID = m.CurrencyID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    InternshipCategoryName = m.InternshipCategory.Name,
                    ThesisName = m.ThesisName,
                    ThesisTypeID = m.ThesisTypeID,
                    ThesisTypeName = m.ThesisType.Name,
                    ThesisTypeCodeName = m.ThesisType.CodeName
                });

            var cacheSetup = Services.CacheService.GetSetup<ThesisDetailModel>(GetSource());

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Thesis>(thesisID),
                EntityKeys.KeyDelete<Entity.Thesis>(thesisID)
            };
            cacheSetup.ObjectID = thesisID;

            var thesis = await Services.CacheService.GetOrSetAsync(async () => await thesisQuery.FirstOrDefaultAsync(), cacheSetup);
            
            if (thesis == null)
            {
                return null;
            }

            // set thesis type value
            thesis.ThesisTypeNameConverted = thesis.ThesisTypeCodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", await GetAllThesisTypesAsync()) : thesis.ThesisTypeName;

            return thesis;

        }

        /// <summary>
        /// Gets all thesis types except the "all" type
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllThesisTypesAsync()
        {
            return (await Services.ThesisTypeService.GetAllCachedAsync())
                .Where(m => !m.CodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase))
                .Select(m => m.Name)
                .ToList();
        }

        #endregion
    }
}
