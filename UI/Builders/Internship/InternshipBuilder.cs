using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using PagedList;
using System.Data.Entity;

using UI.Base;
using UI.Builders.Internship.Views;
using UI.Builders.Internship.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using Core.Extensions;
using Core.Helpers;
using Core.Helpers.Internship;
using Entity.Base;
using Service.Services.Activities.Enums;

namespace UI.Builders.Internship
{
    public class InternshipBuilder : BaseBuilder
    {

        #region Constructor

        public InternshipBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<InternshipNotFoundView> BuildNotFoundViewAsync()
        {
            return new InternshipNotFoundView()
            {
                LatestInternships = (await GetAllInternshipsAsync()).OrderByDescending(m => m.ID).Take(10) // take 10 latest internships
            };
        }

        public async Task<InternshipBrowseView> BuildBrowseViewAsync(int? page, string category, string search, string city)
        {
            var pageSize = 30;
            var pageNumber = (page ?? 1);
            var isSearchQuery = !string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(city);

            // get all internships and store them in cache (filtering will be faster)
            var internships = await GetAllInternshipsAsync();

            // filter by category
            if (!string.IsNullOrEmpty(category))
            {
                var categoryID = await GetCategoryIDAsync(category);
                if (categoryID != 0)
                {
                    internships = internships.Where(m => m.InternshipCategoryID == categoryID);
                }
            }

            // search filter
            if (isSearchQuery)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    internships = internships.Where(m => m.Description.Contains(search, StringComparison.OrdinalIgnoreCase) || m.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || m.CompanyName.Contains(search, StringComparison.OrdinalIgnoreCase));
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
                Internship = internship,                
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Logs activity for viewing internship
        /// </summary>
        /// <param name="internshipID">internshipID</param>
        /// <param name="companyID">companyID</param>
        public async Task<int> LogInternshipViewActivityAsync(int internshipID, int companyID)
        {
            if (internshipID == 0)
            {
                return 0;
            }

            var activityUserId = this.CurrentUser.IsAuthenticated ? this.CurrentUser.Id : null;

            return await this.Services.ActivityService.LogActivity(ActivityTypeEnum.ViewInternshipDetail, companyID, activityUserId, internshipID);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets internship categories
        /// Gets only caregories that are not empty
        /// Orderes categories by the number of internships in them
        /// </summary>
        /// <returns>Internship categories</returns>
        private async Task<IEnumerable<InternshipCategoryModel>> GetInternshipCategoriesAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<InternshipCategoryModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<InternshipCategory>(),
                EntityKeys.KeyDeleteAny<InternshipCategory>(),
                EntityKeys.KeyUpdateAny<InternshipCategory>(),
            };

            var categoriesQuery = Services.InternshipCategoryService.GetAll()
                .Select(m => new InternshipCategoryModel()
                {
                    CategoryID = m.ID,
                    CategoryName = m.Name,
                    CodeName = m.CodeName,
                    InternshipCount = m.Internships.Where(s => s.IsActive).Count()
                })
                .Where(m => m.InternshipCount > 0)
                .OrderByDescending(m => m.InternshipCount);

            return await Services.CacheService.GetOrSetAsync(async () => await categoriesQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets all internships from DB or cache
        /// </summary>
        /// <returns>Collection of all internships</returns>
        private async Task<IEnumerable<InternshipBrowseModel>> GetAllInternshipsAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<InternshipBrowseModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var internshipsQuery = Services.InternshipService.GetAll()
                .Where(m => m.IsActive)
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
                    CompanyGuid = m.Company.CompanyGuid,
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

            var internships = await Services.CacheService.GetOrSetAsync(async () => await internshipsQuery.ToListAsync(), cacheSetup);

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

            var categoryQuery = Services.InternshipCategoryService.GetAll()
                .Where(m => m.CodeName == categoryCodeName)
                .Select(s => new
                {
                    CategoryID = s.ID
                })
                .Take(1);

            const int cacheMinutes = 120;
            var cacheSetup = Services.CacheService.GetSetup<int>(GetSource(), cacheMinutes);

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<InternshipCategory>(),
                EntityKeys.KeyUpdateAny<InternshipCategory>(),
                EntityKeys.KeyDeleteAny<InternshipCategory>()
            };
            cacheSetup.ObjectStringID = categoryCodeName;

            var category = await Services.CacheService.GetOrSetAsync(async () => await categoryQuery.FirstOrDefaultAsync(), cacheSetup);

            return category?.CategoryID ?? 0;
        }

        /// <summary>
        /// Gets Internship detail model from db/cache
        /// </summary>
        /// <param name="internshipID">Internship ID</param>
        /// <returns>Internship or null if none is found</returns>
        private async Task<InternshipDetailModel> GetInternshipDetailModelAsync(int internshipID)
        {
            var internshipQuery = Services.InternshipService.GetSingle(internshipID)
                .Select(m => new InternshipDetailModel()
                {
                    Company = new InternshipDetailCompanyModel()
                    {
                        CompanyCodeName = m.Company.CodeName,
                        CompanyID = m.CompanyID,
                        CompanyGuid = m.Company.CompanyGuid,
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
                    CodeName = m.CodeName,
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
                    CountryName = m.Country.CountryName,
                    Languages = m.Languages,
                    HomeOfficeOptionID = m.HomeOfficeOptionID,
                    HomeOfficeOption = new InternshipHomeOfficeOptionModel()
                    {
                       HomeOfficeName = m.HomeOfficeOption.HomeOfficeName,
                       IconClass = m.HomeOfficeOption.IconClass
                    },
                    StudentStatusOptionID = m.StudentStatusOptionID,
                    StudentStatusOption = new InternshipStudentStatusOptionModel()
                    {
                        StudentStatusName = m.StudentStatusOption.StatusName,
                        IconClass = m.StudentStatusOption.IconClass
                    }
                });


            var cacheMinutes = 120;
            var cacheSetup = Services.CacheService.GetSetup<InternshipDetailModel>(GetSource(), cacheMinutes);

            // get company ID so that cache of internship is cleared when company changes
            var companyID = await GetCompanyIDOfInternshipAsync(internshipID);

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Internship>(internshipID),
                EntityKeys.KeyDelete<Entity.Internship>(internshipID),
                EntityKeys.KeyUpdate<Entity.Company>(companyID),
                EntityKeys.KeyDelete<Entity.Company>(companyID)
            };
            cacheSetup.ObjectID = internshipID;

            var internship = await Services.CacheService.GetOrSetAsync(async () => await internshipQuery.FirstOrDefaultAsync(), cacheSetup);

            // return null if internship was not found
            if (internship == null)
            {
                return null;
            }

            // set default duration
            internship.MinDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MinDurationTypeCodeName);
            internship.MaxDurationType = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MaxDurationTypeCodeName);

            // set default duration values
            internship.MinDurationInDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MinDurationType, internship.MinDurationInDays, internship.MinDurationInWeeks, internship.MinDurationInMonths);
            internship.MaxDurationInDefaultValue = InternshipHelper.GetInternshipDurationDefaultValue(internship.MaxDurationType, internship.MaxDurationInDays, internship.MaxDurationInWeeks, internship.MaxDurationInMonths);

            // set required languages
            internship.RequiredLanguages = (await Services.LanguageService.GetLanguagesFromCommaSeparatedStringAsync(internship.Languages))
                .Select(m => new InternshipLanguageModel()
                {
                    CodeName = m.CodeName,
                    IconClass = m.IconClass,
                    LanguageName = m.LanguageName
                });

            // add activity stats
            internship.ActivityStats = await GetInternshipActivityStatsAsync(internshipID);

            return internship;

        }

        /// <summary>
        /// Gets CompanyID of given internship. Value is cached.
        /// </summary>
        /// <param name="internshipID">internshipID</param>
        /// <returns>CompanyID or 0 if company is not fond</returns>
        private async Task<int> GetCompanyIDOfInternshipAsync(int internshipID)
        {
            const int cacheMinutes = 120;

            var cacheSetup = Services.CacheService.GetSetup<IntWrapper>(GetSource(), cacheMinutes);
            cacheSetup.ObjectID = internshipID;

            var query = Services.InternshipService.GetAll()
                .Where(m => m.ID == internshipID)
                .Select(m => new IntWrapper()
                {
                    Value = m.CompanyID
                });

            var result = await Services.CacheService.GetOrSetAsync(async () => await query.FirstOrDefaultAsync(), cacheSetup);

            return result?.Value ?? 0;
        }

        /// <summary>
        /// Gets internship activity stats
        /// </summary>
        /// <param name="internshipID">internshipId</param>
        /// <returns>Activity stats</returns>
        private async Task<InternshipActivityStats> GetInternshipActivityStatsAsync(int internshipID)
        {
            var cacheSetup = Services.CacheService.GetSetup<InternshipActivityStats>(GetSource());
            cacheSetup.ObjectID = internshipID;

            var internshipFormSubmissionsCount = await Services.CacheService.GetOrSetAsync(async () => await Services.ActivityService.GetInternshipFormSubmissions(internshipID).CountAsync(), cacheSetup);

            return new InternshipActivityStats()
            {
                InternshipSubmissionsCount = internshipFormSubmissionsCount
            };
        }

        #endregion
    }
}
