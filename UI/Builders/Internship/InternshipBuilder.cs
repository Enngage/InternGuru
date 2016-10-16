using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UI.Base;
using Core.Context;
using UI.Builders.Internship.Views;
using UI.Builders.Internship.Models;
using UI.Builders.Services;
using System.Data.Entity;
using PagedList;
using Common.Extensions;
using Entity;
using Common.Helpers;
using Common.Helpers.Internship;

namespace UI.Builders.Internship
{
    public class InternshipBuilder : BaseBuilder
    {

        #region Constructor

        public InternshipBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<InternshipBrowseView> BuildBrowseViewAsync(int? page, string category, string search, string city)
        {
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            bool isSearchQuery = !string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(city); // indicates if cache will be used

            // get all internships and store them in cache (filtering will be faster)
            var internships = await GetAllInternshipsAsync();

            // filter by category
            var categoryID = await GetCategoryIDAsync(category);
            if (categoryID != 0)
            {
                internships = internships.Where(m => m.InternshipCategoryID == categoryID);
            }

            // search filter
            if (isSearchQuery)
            {
                // search query - do not cache
                if (!string.IsNullOrEmpty(search))
                {
                    internships = internships.Where(m => m.Description.Contains(search, StringComparison.OrdinalIgnoreCase) || m.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // city search
                if (!string.IsNullOrEmpty(city))
                {
                    internships = internships.Where(m => m.City.Contains(city, StringComparison.OrdinalIgnoreCase));
                }
            }

            return new InternshipBrowseView()
            {
                Internships = internships.ToPagedList(pageNumber, pageSize),
                InternshipCategories = await GetInternshipCategoriesAsync()
            };
        }

        public async Task<InternshipDetailView> BuildDetailViewAsync(int internshipID)
        {
            var internship = await GetInternshipDetailModelAsync(internshipID);

            if (internship == null)
            {
                return null;
            }

            return new InternshipDetailView()
            {
                Internship = internship
            };
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets internship categories
        /// </summary>
        /// <returns>Internship categories</returns>
        private async Task<IEnumerable<InternshipCategoryModel>> GetInternshipCategoriesAsync()
        {
            int cacheMinutes = 60;

            var cacheSetup = this.Services.CacheService.GetSetup<InternshipCategoryModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.InternshipCategory>(),
                EntityKeys.KeyDeleteAny<Entity.InternshipCategory>(),
                EntityKeys.KeyUpdateAny<Entity.InternshipCategory>(),
            };

            var categoriesQuery = this.Services.InternshipCategoryService.GetAll()
                .Select(m => new InternshipCategoryModel()
                {
                    CategoryID = m.ID,
                    CategoryName = m.Name,
                    CodeName = m.CodeName,
                    InternshipCount = m.Internships.Count
                })
                .OrderBy(m => m.CategoryName);

            return await this.Services.CacheService.GetOrSetAsync(async () => await categoriesQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets all internships from DB or cache
        /// </summary>
        /// <returns>Collection of all internships</returns>
        private async Task<IEnumerable<InternshipBrowseModel>> GetAllInternshipsAsync()
        {
            int cacheMinutes = 60;

            var cacheSetup = this.Services.CacheService.GetSetup<InternshipBrowseModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var internshipsQuery = Services.InternshipService.GetAll()
                .Where(m => m.IsActive == true)
                .OrderByDescending(m => m.Created)
                .Select(m => new InternshipBrowseModel()
                {
                    ID = m.ID,
                    CodeName = m.CodeName,
                    Created = m.Created,
                    Amount = m.Amount,
                    AmountTypeCode = m.AmountType.CodeName,
                    City = m.City,
                    CompanyID = m.CompanyID,
                    CompanyName = m.Company.CompanyName,
                    CountryCode = m.Country.CodeName,
                    CurrencyCode = m.Currency.CodeName,
                    CountryName = m.Country.CountryName,
                    AmountTypeName = m.AmountType.AmountTypeName,
                    CurrencyName = m.Currency.CurrencyName,
                    CurrencyShowSignOnLeft = m.Currency.ShowSignOnLeft,
                    MinDurationDays = m.MinDurationInDays,
                    MinDurationMonths = m.MinDurationInMonths,
                    MinDurationWeeks = m.MinDurationInWeeks,
                    MaxDurationDays = m.MaxDurationInDays,
                    MaxDurationMonths = m.MaxDurationInMonths,
                    MaxDurationWeeks = m.MaxDurationInWeeks,
                    InternshipCategoryID = m.InternshipCategoryID,
                    InternshipCategoryName = m.InternshipCategory.Name,
                    IsPaid = m.IsPaid,
                    StartDate = m.StartDate,
                    Title = m.Title,
                    Description = m.Description,
                    Requirements = m.Requirements,
                    MaxDurationTypeCodeName = m.MaxDurationType.CodeName,
                    MinDurationTypeCodeName = m.MinDurationType.CodeName
                });

            var internships = await this.Services.CacheService.GetOrSetAsync(async () => await internshipsQuery.ToListAsync(), cacheSetup);

            // initialize duration types and values
            foreach (var internship in internships)
            {
                internship.MinDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MinDurationTypeCodeName);
                internship.MaxDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MaxDurationTypeCodeName);

                internship.MinDurationDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MinDurationType, internship.MinDurationDays, internship.MinDurationWeeks, internship.MinDurationMonths);
                internship.MaxDurationDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MaxDurationType, internship.MaxDurationDays, internship.MaxDurationWeeks, internship.MaxDurationMonths);
            }

            return internships;
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
        /// Gets Internship detail model from db/cache
        /// </summary>
        /// <param name="internshipID">Internship ID</param>
        /// <returns>Internship or null if none is found</returns>
        private async Task<InternshipDetailModel> GetInternshipDetailModelAsync(int internshipID)
        {
            var internshipQuery = this.Services.InternshipService.GetSingle(internshipID)
                .Select(m => new InternshipDetailModel()
                {
                    Company = new InternshipDetailCompanyModel()
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
                    AmountTypeCodeName = m.AmountType.CodeName,
                    CurrencyName = m.Currency.CurrencyName,
                    CurrencyShowSignOnLeft = m.Currency.ShowSignOnLeft,
                    AmountTypeName = m.AmountType.AmountTypeName,
                    CurrencyCode = m.Currency.CodeName,
                    City = m.City,
                    Created = m.Created,
                    Description = m.Description,
                    HasFlexibleHours = m.HasFlexibleHours,
                    ID = m.ID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    InternshipCategoryName = m.InternshipCategory.Name,
                    IsActive = m.IsActive,
                    IsPaid = m.IsPaid,
                    MaxDurationTypeID = m.MaxDurationTypeID,
                    MinDurationTypeID = m.MinDurationTypeID,
                    MaxDurationTypeCodeName = m.MaxDurationType.CodeName,
                    MinDurationTypeCodeName = m.MinDurationType.CodeName,
                    MaxDurationInDays = m.MaxDurationInDays,
                    MaxDurationInMonths = m.MaxDurationInMonths,
                    MaxDurationInWeeks = m.MaxDurationInWeeks,
                    MinDurationInDays = m.MinDurationInDays,
                    MinDurationInMonths = m.MinDurationInMonths,
                    MinDurationInWeeks = m.MinDurationInWeeks,
                    Requirements = m.Requirements,
                    StartDate = m.StartDate,
                    Title = m.Title,
                    Updated = m.Updated,
                    WorkingHours = m.WorkingHours,
                    CountryCode = m.Country.CountryCode,
                    CountryName = m.Country.CountryName
                });


            int cacheMinutes = 120;
            var cacheSetup = this.Services.CacheService.GetSetup<InternshipDetailModel>(this.GetSource(), cacheMinutes);

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Internship>(internshipID),
                EntityKeys.KeyDelete<Entity.Internship>(internshipID)
            };
            cacheSetup.ObjectID = internshipID;

            var internship = await this.Services.CacheService.GetOrSetAsync(async () => await internshipQuery.FirstOrDefaultAsync(), cacheSetup);

            // set default duration
            internship.MinDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MinDurationTypeCodeName);
            internship.MaxDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MaxDurationTypeCodeName);

            // set default duration values
            internship.MinDurationInDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MinDurationType, internship.MinDurationInDays, internship.MinDurationInWeeks, internship.MinDurationInMonths);
            internship.MaxDurationInDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MaxDurationType, internship.MaxDurationInDays, internship.MaxDurationInWeeks, internship.MaxDurationInMonths);

            return internship;

        }

        #endregion
    }
}
