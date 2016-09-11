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
            if (!this.CurrentUser.IsAuthenticated)
            {
                return null;
            }

            var internshipsQuery = internshipService.GetAll()
                .Where(m => m.ApplicationUserId == this.CurrentUser.Id)
                .OrderByDescending(m => m.Created)
                .Select(m => new AuthInternshipListingModel()
                {
                    ID = m.ID,
                    Title = m.Title,
                    Created = m.Created,
                    IsActive = m.IsActive
                });

            int cacheMinutes = 60;
            var cacheSetup = CacheService.GetSetup<AuthInternshipListingModel>(this.GetSource(), cacheMinutes);

            var internships = await CacheService.GetOrSet(async () => await internshipsQuery.ToListAsync(), cacheSetup);

            return new AuthIndexView()
            {
                CompanyIsCreated = await GetCompanyIDOfCurrentUserAsync() != 0,
                Internships = internships
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

        public async Task<AuthEditInternshipView> BuildEditInternshipViewAsync(int internshipID)
        {
            var companyIDOfCurrentUser = await GetCompanyIDOfCurrentUserAsync();

            var internshipQuery = this.internshipService.GetSingle(internshipID)
                .Where(m => m.CompanyID == companyIDOfCurrentUser) // only user assigned to company can edit the internship (otherwise other users could edit the internship)
                .Select(m => new AuthAddEditInternshipForm()
                {
                    Amount = m.Amount,
                    AmountType = m.AmountType,
                    City = m.City,
                    Country = m.Country,
                    Currency = m.Currency,
                    Description = m.Description,
                    ID = m.ID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    IsPaid = m.IsPaid ? "on" : "",
                    MaxDurationType = m.MaxDurationType,
                    MinDurationType = m.MinDurationType,
                    StartDate = m.StartDate,
                    Title = m.Title,
                    MinDurationInDays = m.MinDurationInDays,
                    MinDurationInMonths = m.MinDurationInMonths,
                    MinDurationInWeeks = m.MinDurationInWeeks,
                    MaxDurationInDays = m.MaxDurationInDays,
                    MaxDurationInMonths = m.MaxDurationInMonths,
                    MaxDurationInWeeks = m.MaxDurationInWeeks,
                    IsActive = m.IsActive ? "on" : null,
                });

            var internship = await internshipQuery.FirstOrDefaultAsync();

            // internship was not found
            if (internship == null)
            {
                return null;
            }

            // initialize form values
            internship.InternshipCategories = await GetInternshipCategoriesAsync();
            internship.AmountTypes = InternshipHelper.GetAmountTypes();
            internship.DurationTypes = InternshipHelper.GetInternshipDurations();
            internship.Countries = CountryHelper.GetCountries();
            internship.Currencies = CurrencyHelper.GetCurrencies();

            // set default duration
            var minDurationEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MinDurationType);
            var maxDurationEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MaxDurationType);

            if (minDurationEnum == InternshipDurationTypeEnum.Days)
            {
                internship.MinDuration = internship.MinDurationInDays;
            }
            else if (minDurationEnum == InternshipDurationTypeEnum.Weeks)
            {
                internship.MinDuration = internship.MinDurationInWeeks;
            }
            else if (minDurationEnum == InternshipDurationTypeEnum.Months)
            {
                internship.MinDuration = internship.MinDurationInMonths;
            }

            if (maxDurationEnum == InternshipDurationTypeEnum.Days)
            {
                internship.MaxDuration = internship.MaxDurationInDays;
            }
            else if (maxDurationEnum == InternshipDurationTypeEnum.Weeks)
            {
                internship.MaxDuration = internship.MaxDurationInWeeks;
            }
            else if (maxDurationEnum == InternshipDurationTypeEnum.Months)
            {
                internship.MaxDuration = internship.MaxDurationInMonths;
            }

            return new AuthEditInternshipView()
            {
                InternshipForm = internship
            };
        }

        public async Task<AuthNewInternshipView> BuildNewInternshipViewAsync()
        {
            var form = new AuthAddEditInternshipForm()
            {
                InternshipCategories = await GetInternshipCategoriesAsync(),
                AmountTypes = InternshipHelper.GetAmountTypes(),
                DurationTypes = InternshipHelper.GetInternshipDurations(),
                Countries = CountryHelper.GetCountries(),
                Currencies = CurrencyHelper.GetCurrencies(),
                IsActive = "on", // IsActive is enabled by default
            };

            return new AuthNewInternshipView()
            {
                InternshipForm = form,
                CanCreateInternship = await GetCompanyIDOfCurrentUserAsync() != 0, // user can create internship only if he created company before
            };
        }

        public async Task<AuthEditInternshipView> BuildEditInternshipViewAsync(AuthAddEditInternshipForm form)
        {

            form.InternshipCategories = await GetInternshipCategoriesAsync();
            form.AmountTypes = InternshipHelper.GetAmountTypes();
            form.DurationTypes = InternshipHelper.GetInternshipDurations();
            form.Countries = CountryHelper.GetCountries();
            form.Currencies = CurrencyHelper.GetCurrencies();

            return new AuthEditInternshipView()
            {
                InternshipForm = form,
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
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
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
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
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

                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can create internship
                    throw new UIException("Nelze vytvořit stáž bez příhlášení");
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
                    IsPaid = form.GetIsPaid(), 
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
                    MinDurationType = form.MinDurationType,
                    MaxDurationType = form.MaxDurationType,
                    IsActive = form.GetIsActive(),
                    ApplicationUserId = this.CurrentUser.Id,
                };

                await internshipService.InsertAsync(internship);

                return internship.ID;
            }
            catch (Exception ex)
            {
                // log error
                LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
            }
        }

        /// <summary>
        /// Edits internship from given form
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditInternship(AuthAddEditInternshipForm form)
        {
            try
            {
                var companyIDOfCurrentUser = await GetCompanyIDOfCurrentUserAsync();

                if (companyIDOfCurrentUser == 0)
                {
                    // we cannot create internship without assigned company
                    throw new UIException("Nelze vytvořit stáž bez firmy");
                }

                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can create internship
                    throw new UIException("Nelze vytvořit stáž bez příhlášení");
                }

                // Get enums for duration type
                var maxDurationTypeEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(form.MaxDurationType);
                var minDurationTypeEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(form.MinDurationType);

                var internship = new Entity.Internship
                {
                    ID = form.ID,
                    Amount = form.Amount,
                    AmountType = form.AmountType,
                    City = form.City,
                    Country = form.Country,
                    StartDate = form.StartDate,
                    Title = form.Title,
                    IsPaid = form.GetIsPaid(), //TODO
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
                    MinDurationType = form.MinDurationType,
                    MaxDurationType = form.MaxDurationType,
                    IsActive = form.GetIsActive(),
                    ApplicationUserId = this.CurrentUser.Id,
                };

                await internshipService.UpdateAsync(internship);
            }
            catch (Exception ex)
            {
                // log error
                LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
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

            var companyQuery = companyService.GetAll()
                .Where(m => m.ApplicationUserId == this.CurrentUser.Id)
                .Take(1)
                .Select(m => m.ID);

            int cacheMinutes = 30;
            var cacheSetup = CacheService.GetSetup<int>(GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Company.KeyCreateAny<Entity.Company>(),
                Entity.Company.KeyDeleteAny<Entity.Company>(),
            };

            var company = await CacheService.GetOrSet(async () => await companyQuery.FirstOrDefaultAsync(), cacheSetup);

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
