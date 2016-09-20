using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UI.Base;
using Core.Context;
using UI.Builders.Internship.Views;
using UI.Builders.Internship.Models;
using PagedList.EntityFramework;
using UI.Builders.Services;
using System.Collections;
using System.Data.Entity;
using PagedList;
using Common.Extensions;

namespace UI.Builders.Internship
{
    public class InternshipBuilder : BaseBuilder
    {

        #region Constructor

        public InternshipBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader){}

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

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets all internships from DB or cache
        /// </summary>
        /// <returns>Collection of all internships</returns>
        private async Task<IEnumerable<InternshipBrowseModel>> GetAllInternshipsAsync()
        {
            int cacheMinutes = 60;

            var cacheSetup = CacheService.GetSetup<InternshipBrowseModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Internship.KeyCreateAny<Entity.Internship>(),
                Entity.Internship.KeyDeleteAny<Entity.Internship>(),
                Entity.Internship.KeyUpdateAny<Entity.Internship>()
            };

            var internshipsQuery = Services.InternshipService.GetAll()
                .Where(m => m.IsActive == true)
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
                    MaxDurationType = m.MaxDurationType,
                    MinDurationType = m.MinDurationType,
                    Description = m.Description,
                    Requirements = m.Requirements
                });

            return await CacheService.GetOrSetAsync(async () => await internshipsQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets internship categories and stores the result in cache
        /// </summary>
        /// <returns>Collection of internship categories</returns>
        private async Task<IEnumerable<InternshipCategoryModel>> GetInternshipCategoriesAsync()
        {
            int cacheMinutes = 120;

            var internshipCategoryQuery = this.Services.InternshipCategoryService.GetAll()
                .OrderByDescending(m => m.Name)
                .Select(m => new InternshipCategoryModel()
                {
                    CategoryID = m.ID,
                    CategoryName = m.Name,
                    CodeName = m.CodeName
                });

            var cacheSetup = this.CacheService.GetSetup<IEnumerable<InternshipCategoryModel>>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.InternshipCategory.KeyCreateAny<Entity.InternshipCategory>(),
                Entity.InternshipCategory.KeyUpdateAny<Entity.InternshipCategory>(),
                Entity.InternshipCategory.KeyDeleteAny<Entity.InternshipCategory>()
            };

            var internshipCategories = await this.CacheService.GetOrSetAsync(async () => await internshipCategoryQuery.ToListAsync(), cacheSetup);

            return internshipCategories;
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
            var cacheSetup = this.CacheService.GetSetup<int>(this.GetSource(), cacheMinutes);

            cacheSetup.Dependencies = new List<string>()
            {
                Entity.InternshipCategory.KeyCreateAny<Entity.InternshipCategory>(),
                Entity.InternshipCategory.KeyUpdateAny<Entity.InternshipCategory>(),
                Entity.InternshipCategory.KeyDeleteAny<Entity.InternshipCategory>()
            };
            cacheSetup.ObjectStringID = categoryCodeName;

            var category = await this.CacheService.GetOrSetAsync(async () => await categoryQuery.FirstOrDefaultAsync(), cacheSetup);

            return category == null ? 0 : category.CategoryID;
        }

        #endregion
    }
}
