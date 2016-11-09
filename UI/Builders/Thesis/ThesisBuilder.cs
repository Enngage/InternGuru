using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using Entity;
using System.Data.Entity;

using UI.Base;
using Core.Context;
using UI.Builders.Services;
using UI.Builders.Thesis.Views;
using UI.Builders.Thesis.Models;
using Core.Services.Enums;

namespace UI.Builders.Thesis
{
    public class ThesisBuilder : BaseBuilder
    {

        #region Constructor

        public ThesisBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Actions

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
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            bool isSearchQuery = !string.IsNullOrEmpty(search);

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
        /// Gets all browse models
        /// </summary>
        /// <returns>Collection of browse models</returns>
        private async Task<IEnumerable<ThesisBrowseModel>> GetAllThesisBrowseModelsAsync()
        {
            int cacheMinutes = 60;
     
            var cacheSetup = this.Services.CacheService.GetSetup<ThesisBrowseView>(GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
            };

            var thesesQuery = this.Services.ThesisService.GetAll()
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
                });

            var theses = await this.Services.CacheService.GetOrSet(async () => await thesesQuery.ToListAsync(), cacheSetup);

            // inititialize thesis type name
            foreach (var thesis in theses)
            {
                thesis.ThesisTypeNameConverted = thesis.ThesisTypeCodeName.Equals(ThesisTypeEnum.all.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", (await GetAllThesisTypesAsync())) : thesis.ThesisTypeName;
            }

            return theses;
        }

        /// <summary>
        /// Gets thesis categories
        /// </summary>
        /// <returns>Thesis categories</returns>
        private async Task<IEnumerable<ThesisCategoryModel>> GetThesisCategoriesAsync()
        {
            int cacheMinutes = 60;

            var cacheSetup = this.Services.CacheService.GetSetup<ThesisCategoryModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
            };

            var categoriesQuery = this.Services.InternshipCategoryService.GetAll()
                .Select(m => new ThesisCategoryModel()
                {
                    CategoryID = m.ID,
                    CategoryName = m.Name,
                    CodeName = m.CodeName,
                    ThesesCount = m.Theses.Count
                })
                .OrderBy(m => m.CategoryName);

            return await this.Services.CacheService.GetOrSetAsync(async () => await categoriesQuery.ToListAsync(), cacheSetup);
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

            var categoryQuery = this.Services.InternshipCategoryService.GetAll()
                .Where(m => m.CodeName == categoryCodeName)
                .Select(s => new
                {
                    CategoryID = s.ID
                })
                .Take(1);

            int cacheMinutes = 120;
            var cacheSetup = this.Services.CacheService.GetSetup<int>(this.GetSource(), cacheMinutes);

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.InternshipCategory>(),
                EntityKeys.KeyUpdateAny<Entity.InternshipCategory>(),
                EntityKeys.KeyDeleteAny<Entity.InternshipCategory>()
            };
            cacheSetup.ObjectStringID = categoryCodeName;

            var category = await this.Services.CacheService.GetOrSetAsync(async () => await categoryQuery.FirstOrDefaultAsync(), cacheSetup);

            return category == null ? 0 : category.CategoryID;
        }

        /// <summary>
        /// Gets thesis model
        /// </summary>
        /// <param name="thesisID">ThesisID</param>/param>
        /// <returns>Thesis or null if none is found</returns>
        private async Task<ThesisDetailModel> GetThesisDetailModelAsync(int thesisID)
        {
            var thesisQuery = this.Services.ThesisService.GetSingle(thesisID)
                .Where(m => m.IsActive == true)
                .Select(m => new ThesisDetailModel()
                {
                    Company = new ThesisDetailCompanyModel()
                    {
                        CompanyCodeName = m.Company.CodeName,
                        CompanyID = m.CompanyID,
                        CompanyName = m.Company.CompanyName,
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

            int cacheMinutes = 120;
            var cacheSetup = this.Services.CacheService.GetSetup<ThesisDetailModel>(this.GetSource(), cacheMinutes);

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Thesis>(thesisID),
                EntityKeys.KeyDelete<Entity.Thesis>(thesisID)
            };
            cacheSetup.ObjectID = thesisID;

            var thesis = await this.Services.CacheService.GetOrSetAsync(async () => await thesisQuery.FirstOrDefaultAsync(), cacheSetup);

            // set thesis type value
            thesis.ThesisTypeNameConverted = thesis.ThesisTypeCodeName.Equals(ThesisTypeEnum.all.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", await GetAllThesisTypesAsync()) : thesis.ThesisTypeName;

            return thesis;

        }

        /// <summary>
        /// Gets all thesis types except the "all" type
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllThesisTypesAsync()
        {
            return (await this.Services.ThesisTypeService.GetAllCachedAsync())
                .Where(m => !m.CodeName.Equals(ThesisTypeEnum.all.ToString(), StringComparison.OrdinalIgnoreCase))
                .Select(m => m.Name)
                .ToList();
        }

        #endregion
    }
}
