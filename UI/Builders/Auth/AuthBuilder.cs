using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

using UI.Abstract;
using Cache;
using Core.Context;
using Core.Services;
using UI.Builders.Auth.Views;
using UI.Builders.Auth.Forms;
using System;
using System.Collections.Generic;
using UI.Builders.Auth.Models;
using UI.Files;
using Common.Config;
using Common.Helpers;
using UI.Exceptions;
using Common.Helpers.Internship;

namespace UI.Builders.Company
{
    public class AuthBuilder : BuilderAbstract
    {
        #region Services

        ICompanyService companyService;
        IInternshipService internshipService;
        ICompanyCategoryService companyCategoryService;
        IInternshipCategoryService internshipCategoryService;
        IFileProvider fileProvider;

        #endregion

        #region Constructor

        public AuthBuilder(
            IAppContext appContext,
            ICacheService cacheService,
            ICompanyService companyService,
            IIdentityService identityService,
            ILogService logService,
            ICompanyCategoryService companyCategoryService,
            IInternshipCategoryService internshipCategoryService,
            IInternshipService internshipService,
            IFileProvider fileProvider)
            : base(
                appContext,
                cacheService,
                identityService,
                logService)
        {
            this.companyService = companyService;
            this.internshipService = internshipService;
            this.companyCategoryService = companyCategoryService;
            this.internshipCategoryService = internshipCategoryService;
            this.fileProvider = fileProvider;
        }

        #endregion

        #region Actions

        public async Task<AuthIndexView> BuildIndexViewAsync()
        {
            var currentUserId = this.CurrentUser.Id;

            // check if user has created company
            var company = await companyService.GetAll()
                .Where(m => m.ApplicationUserId == currentUserId)
                .Take(1)
                .Select(m => new
                {
                    ID = m.ID
                })
                .FirstOrDefaultAsync();

            return new AuthIndexView()
            {
                CompanyIsCreated = company != null
            };
        }

        #endregion

        #region Company 

        public async Task<AuthRegisterCompanyView> BuildRegisterCompanyViewAsync(AuthAddEditCompanyForm form)
        {
            if (form == null)
            {
                // user haven't created any company yet
                form = new AuthAddEditCompanyForm();
            }

            // add countries, categories and company sizes
            form.Countries = Common.Helpers.CountryHelper.GetCountries();
            form.AllowedCompanySizes = Common.Helpers.InternshipHelper.GetAllowedCompanySizes();
            form.CompanyCategories = await GetCompanyCategoriesAsync();

            return new AuthRegisterCompanyView()
            {
                CompanyForm = form,
                CompanyIsCreated = form != null
            };
        }

        public async Task<AuthEditCompanyView> BuildEditCompanyViewAsync(AuthAddEditCompanyForm form)
        {
            if (form == null)
            {
                // invalid form data
                return null;
            }

            // add countries, categories and company sizes
            form.Countries = Common.Helpers.CountryHelper.GetCountries();
            form.AllowedCompanySizes = Common.Helpers.InternshipHelper.GetAllowedCompanySizes();
            form.CompanyCategories = await GetCompanyCategoriesAsync();

            return new AuthEditCompanyView()
            {
                CompanyForm = form,
            };
        }

        public async Task<AuthEditCompanyView> BuildEditCompanyViewAsync()
        {
            var currentUserId = this.CurrentUser.Id;

            // get company assigned to user
            var company = await companyService.GetAll()
                .Where(m => m.ApplicationUserId == currentUserId)
                .Take(1)
                .Select(m => new AuthAddEditCompanyForm()
                {
                    Address = m.Address,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    CompanySize = m.CompanySize,
                    Country = m.Country,
                    Facebook = m.Facebook,
                    ID = m.ID,
                    Lat = m.Lat,
                    Lng = m.Lng,
                    LinkedIn = m.LinkedIn,
                    PublicEmail = m.PublicEmail,
                    LongDescription = m.LongDescription,
                    Twitter = m.Twitter,
                    Web = m.Web,
                    CompanyCategoryID = m.CompanyCategoryID,
                    YearFounded = m.YearFounded
                })
                .FirstOrDefaultAsync();

            if (company == null)
            {
                // user haven't created any company yet
                return null;
            }

            // add countries, categories and company sizes
            company.Countries = Common.Helpers.CountryHelper.GetCountries();
            company.AllowedCompanySizes = Common.Helpers.InternshipHelper.GetAllowedCompanySizes();
            company.CompanyCategories = await GetCompanyCategoriesAsync();

            return new AuthEditCompanyView()
            {
                CompanyForm = company,
            };
        }

        public async Task<AuthRegisterCompanyView> BuildRegisterCompanyViewAsync()
        {
            var currentUserId = this.CurrentUser.Id;

            // get company assigned to user
            var company = await companyService.GetAll()
                .Where(m => m.ApplicationUserId == currentUserId)
                .Take(1)
                .Select(m => new AuthAddEditCompanyForm()
                {
                    Address = m.Address,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    CompanySize = m.CompanySize,
                    Country = m.Country,
                    Facebook = m.Facebook,
                    ID = m.ID,
                    Lat = m.Lat,
                    Lng = m.Lng,
                    LinkedIn = m.LinkedIn,
                    PublicEmail = m.PublicEmail,
                    LongDescription = m.LongDescription,
                    Twitter = m.Twitter,
                    Web = m.Web,
                    CompanyCategoryID = m.CompanyCategoryID,
                    YearFounded = m.YearFounded
                })
                .FirstOrDefaultAsync();

            if (company == null)
            {
                // user haven't created any company yet
                company = new AuthAddEditCompanyForm();
            }

            // add countries, categories and company sizes
            company.Countries = Common.Helpers.CountryHelper.GetCountries();
            company.AllowedCompanySizes = Common.Helpers.InternshipHelper.GetAllowedCompanySizes();
            company.CompanyCategories = await GetCompanyCategoriesAsync();

            return new AuthRegisterCompanyView()
            {
                CompanyForm = company,
                CompanyIsCreated = company != null
            };
        }

        #endregion

        #region Internship

        public async Task<AuthNewInternshipView> BuildNewInternshipViewAsync()
        {
            var form = new AuthAddEditInternshipForm()
            {
                InternshipCategories = await GetInternshipCategoriesAsync(),
                AmountTypes = InternshipHelper.GetAmountTypes(),
                DurationTypes = InternshipHelper.GetInternshipDurations(),
                Countries = CountryHelper.GetCountries(),
                Currencies = CurrencyHelper.GetCurrencies()
            };

            return new AuthNewInternshipView()
            {
                InternshipForm = form,
                CanCreateInternship = await GetCompanyIDOfCurrentUserAsync() != 0, // user can create internship only if he created company before
            };
        }

        public async Task<AuthNewInternshipView> BuildNewInternshipViewAsync(AuthAddEditInternshipForm form)
        {

            form.InternshipCategories = await GetInternshipCategoriesAsync();
            form.AmountTypes = InternshipHelper.GetAmountTypes();
            form.DurationTypes = InternshipHelper.GetInternshipDurations();
            form.Countries = CountryHelper.GetCountries();
            form.Currencies = CurrencyHelper.GetCurrencies();

            return new AuthNewInternshipView()
            {
                InternshipForm = form,
                CanCreateInternship = await GetCompanyIDOfCurrentUserAsync() != 0, // user can create internship only if he created company before
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates new company from given form
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new company</returns>
        public async Task<int> CreateCompany(AuthAddEditCompanyForm form)
        {
            try
            {
                // try to upload files before adding database record of company
                fileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, Entity.Company.GetBannerFileName(form.CompanyName), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerWidth);
                fileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, Entity.Company.GetLogoFileName(form.CompanyName), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);

                var company = new Entity.Company
                {
                    ApplicationUserId = this.CurrentUser.Id,
                    Address = form.Address,
                    City = form.City,
                    CompanyName = form.CompanyName,
                    CompanySize = form.CompanySize,
                    Country = form.Country,
                    Facebook = form.Facebook,
                    Lat = form.Lat,
                    LinkedIn = form.LinkedIn,
                    Lng = form.Lng,
                    LongDescription = form.LongDescription,
                    PublicEmail = form.PublicEmail,
                    Twitter = form.Twitter,
                    Web = form.Web,
                    YearFounded = form.YearFounded,
                    CompanyCategoryID = form.CompanyCategoryID
                };

                await companyService.InsertAsync(company);

                return company.ID;
            }
            catch (Exception ex)
            {
                // log error
                LogService.LogException(ex);

                // re-throw
                throw;
            }
        }

        /// <summary>
        /// Edits company
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditCompany(AuthAddEditCompanyForm form)
        {
            try
            {
                // try to upload files before adding database record of company
                if (form.Banner != null)
                {
                    fileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, StringHelper.GetCodeName(form.CompanyName), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerWidth);
                }
                if (form.Logo != null)
                {
                    fileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, StringHelper.GetCodeName(form.CompanyName), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
                }

                var company = new Entity.Company
                {
                    ID = form.ID,
                    ApplicationUserId = this.CurrentUser.Id,
                    Address = form.Address,
                    City = form.City,
                    CompanyName = form.CompanyName,
                    CompanySize = form.CompanySize,
                    Country = form.Country,
                    Facebook = form.Facebook,
                    Lat = form.Lat,
                    LinkedIn = form.LinkedIn,
                    Lng = form.Lng,
                    LongDescription = form.LongDescription,
                    PublicEmail = form.PublicEmail,
                    Twitter = form.Twitter,
                    Web = form.Web,
                    YearFounded = form.YearFounded,
                    CompanyCategoryID = form.CompanyCategoryID
                };

                await companyService.UpdateAsync(company);
            }
            catch (Exception ex)
            {
                // log error
                LogService.LogException(ex);

                // re-throw
                throw;
            }
        }

        /// <summary>
        /// Creates new internship from given form
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new internship</returns>
        public async Task<int> CreateInternship(AuthAddEditInternshipForm form)
        {
            try
            {
                var companyIDOfCurrentUser = await GetCompanyIDOfCurrentUserAsync();

                if (companyIDOfCurrentUser == 0)
                {
                    // we cannot create internship without assigned company
                    throw new UIException("Internship has invalid company assigned");
                }

                // Get enums for duration type
                var maxDurationTypeEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(form.MaxDurationType);
                var minDurationTypeEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(form.MinDurationType);

                var internship = new Entity.Internship
                {
                    Amount = form.Amount,
                    AmountType = form.AmountType,
                    City = form.City,
                    Country = form.Country,
                    StartDate = form.StartDate,
                    Title = form.Title,
                    IsPaid = true, //TODO
                    CompanyID = companyIDOfCurrentUser,
                    InternshipCategoryID = form.InternshipCategoryID,
                    Currency = form.Currency,
                    Description = form.Description,
                    MaxDurationInDays = form.GetDurationInDays(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInWeeks = form.GetDurationInWeeks(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInMonths = form.GetDurationInMonths(maxDurationTypeEnum, form.MaxDuration),
                    MinDurationInDays = form.GetDurationInDays(minDurationTypeEnum, form.MinDuration),
                    MinDurationInWeeks = form.GetDurationInWeeks(minDurationTypeEnum, form.MinDuration),
                    MinDurationInMonths = form.GetDurationInMonths(minDurationTypeEnum, form.MinDuration),
                };

                await internshipService.InsertAsync(internship);

                return internship.ID;
            }
            catch (Exception ex)
            {
                // log error
                LogService.LogException(ex);

                // re-throw
                throw;
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets company ID of current user
        /// </summary>
        /// <returns>CompanyID of current user or 0 if user is not logged or hasn't created any company</returns>
        private async Task<int> GetCompanyIDOfCurrentUserAsync()
        {
            if (!this.CurrentUser.IsAuthenticated)
            {
                return 0;
            }

            var company = await companyService.GetAll()
                .Where(m => m.ApplicationUserId == this.CurrentUser.Id)
                .Take(1)
                .Select(m => m.ID)
                .FirstOrDefaultAsync();

            return company;
        }

        /// <summary>
        /// Gets company categories and caches the result
        /// </summary>
        /// <returns>Collection of company categories</returns>
        private async Task<IEnumerable<AuthCompanyCategoryModel>> GetCompanyCategoriesAsync()
        {
            var cacheMinutes = 60;
            var cacheSetup = this.CacheService.GetSetup<AuthCompanyCategoryModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.CompanyCategory.KeyCreateAny<Entity.CompanyCategory>(),
                Entity.CompanyCategory.KeyDeleteAny<Entity.CompanyCategory>(),
                Entity.CompanyCategory.KeyUpdateAny<Entity.CompanyCategory>(),
            };

            var companyCategoriesQuery = this.companyCategoryService.GetAll()
                .Select(m => new AuthCompanyCategoryModel()
                {
                    CompanyCategoryID = m.ID,
                    CompanyCategoryName = m.Name
                });

            var companyCategories = await this.CacheService.GetOrSetAsync(async () => await companyCategoriesQuery.ToListAsync(), cacheSetup);

            return companyCategories;
        }

        /// <summary>
        /// Gets internship categories and caches the result
        /// </summary>
        /// <returns>Collection of internship categories</returns>
        private async Task<IEnumerable<AuthInternshipCategoryModel>> GetInternshipCategoriesAsync()
        {
            var cacheMinutes = 60;
            var cacheSetup = this.CacheService.GetSetup<AuthInternshipCategoryModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.InternshipCategory.KeyCreateAny<Entity.InternshipCategory>(),
                Entity.InternshipCategory.KeyDeleteAny<Entity.InternshipCategory>(),
                Entity.InternshipCategory.KeyUpdateAny<Entity.InternshipCategory>(),
            };

            var internshipCategoriesQuery = this.internshipCategoryService.GetAll()
                .Select(m => new AuthInternshipCategoryModel()
                {
                    InternshipCategoryID = m.ID,
                    InternshipCategoryName = m.Name
                });

            var internshipCategories = await this.CacheService.GetOrSetAsync(async () => await internshipCategoriesQuery.ToListAsync(), cacheSetup);

            return internshipCategories;
        }

        #endregion
    }
}
