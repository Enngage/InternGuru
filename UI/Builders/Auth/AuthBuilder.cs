using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

using UI.Base;
using Core.Context;
using UI.Builders.Auth.Views;
using UI.Builders.Auth.Forms;
using System;
using System.Collections.Generic;
using UI.Builders.Auth.Models;
using Common.Config;
using Common.Helpers;
using UI.Exceptions;
using Common.Helpers.Internship;
using UI.Builders.Services;
using Core.Exceptions;

namespace UI.Builders.Company
{
    public class AuthBuilder : BaseBuilder
    {

        #region Constructor

        public AuthBuilder(IAppContext appContext, IServicesLoader servicesLoader): base(appContext, servicesLoader){}

        #endregion

        #region Index

        public async Task<AuthIndexView> BuildIndexViewAsync()
        {
            if (!this.CurrentUser.IsAuthenticated)
            {
                return null;
            }

            var internshipsQuery = this.Services.InternshipService.GetAll()
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
            cacheSetup.Dependencies = new List<string>()
            {
                Entity.Internship.KeyUpdateAny<Entity.Internship>(),
                Entity.Internship.KeyDeleteAny<Entity.Internship>(),
                Entity.Internship.KeyCreateAny<Entity.Internship>(),
            };

            var internships = await CacheService.GetOrSet(async () => await internshipsQuery.ToListAsync(), cacheSetup);

            return new AuthIndexView()
            {
                Internships = internships
            };
        }

        #endregion

        #region Edit Profile

        public async Task<AuthEditProfileView> BuildEditProfileViewAsync()
        {
            if (!this.CurrentUser.IsAuthenticated)
            {
                throw new UIException(UIExceptionEnum.NotAuthenticated);
            }

            var currentApplicationUser = await this.Services.IdentityService.GetSingle(this.CurrentUser.Id).FirstOrDefaultAsync();

            if (currentApplicationUser == null)
            {
                throw new UIException(string.Format("Uživatel s ID {0} nebyl nalezen", this.CurrentUser.Id));
            }

            var form = new AuthEditProfileForm()
            {
                FirstName = currentApplicationUser.FirstName,
                LastName = currentApplicationUser.LastName
            };

            return new AuthEditProfileView()
            {
                ProfileForm = form
            };
        }

        public AuthEditProfileView BuildEditProfileView(AuthEditProfileForm form)
        {
            if (!this.CurrentUser.IsAuthenticated)
            {
                throw new UIException(UIExceptionEnum.NotAuthenticated);
            }

            return new AuthEditProfileView()
            {
                ProfileForm = form
            };
        }

        #endregion

        #region Avatar 

        public AuthAvatarView BuildAvatarView()
        {
            var avatarForm = new AuthAvatarUploadForm();

            return new AuthAvatarView()
            {
                AvatarForm = avatarForm
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
            var company = await Services.CompanyService.GetAll()
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
            var company = await Services.CompanyService.GetAll()
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

            var internshipQuery = this.Services.InternshipService.GetSingle(internshipID)
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
                    HasFlexibleHours = m.HasFlexibleHours ? "on" : null,
                    WorkingHours = m.WorkingHours
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
        /// Edits profile
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditProfile(AuthEditProfileForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
                {
                    throw new UIException(UIExceptionEnum.NotAuthenticated);
                }

                var applicationUser = await this.Services.IdentityService.GetAsync(this.CurrentUser.Id);

                if (applicationUser == null)
                {
                    throw new UIException(string.Format("Uživatel s ID {0} nebyl nalezen", this.CurrentUser.Id));
                }

                // set object properties
                applicationUser.FirstName = form.FirstName;
                applicationUser.LastName = form.LastName;

                await this.Services.IdentityService.UpdateAsync(applicationUser);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
            }
        }

        /// <summary>
        /// Creates new company from given form
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new company</returns>
        public async Task<int> CreateCompany(AuthAddEditCompanyForm form)
        {
            // Create company in transaction because files require CompanyID
            using (var transaction = this.AppContext.BeginTransaction())
            {
                try
                {
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

                   await Services.CompanyService.InsertAsync(company);

                    // upload files
                    if (form.Banner != null)
                    {
                        Services.FileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, Entity.Company.GetBannerFileName(company.ID), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerHeight);
                    }

                    if (form.Logo != null)
                    {
                        Services.FileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, Entity.Company.GetLogoFileName(company.ID), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
                    }

                    // commit transaction
                    transaction.Commit();

                    return company.ID;
                }
                catch (CodeNameNotUniqueException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException(string.Format("Firma {0} je již v databázi", form.CompanyName), ex);
                }
                catch (Exception ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException(UIExceptionEnum.SaveFailure, ex);
                }
            }
        }


        /// <summary>
        /// Edits company
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditCompany(AuthAddEditCompanyForm form)
        {
            using (var transaction = this.AppContext.BeginTransaction())
            {
                try
                {
                    // upload files if they are provided
                    if (form.Banner != null)
                    {
                        Services.FileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, Entity.Company.GetBannerFileName(form.ID), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerHeight);
                    }
                    if (form.Logo != null)
                    {
                        Services.FileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, Entity.Company.GetLogoFileName(form.ID), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
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

                    await Services.CompanyService.UpdateAsync(company);

                    // commit 
                    transaction.Commit();
                }
                catch (CodeNameNotUniqueException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException(string.Format("Firma {0} je již v databázi", form.CompanyName), ex);
                }
                catch (Exception ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException(UIExceptionEnum.SaveFailure, ex);
                }
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
                    throw new UIException("Stáž musí být přiřazena k firmě");
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
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours
                };

                await Services.InternshipService.InsertAsync(internship);

                return internship.ID;           
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

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
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours
                };

                await Services.InternshipService.UpdateAsync(internship);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
            }
        }

        /// <summary>
        /// Uploads avatar for current user
        /// </summary>
        /// <param name="form">Avatar form</param>
        public void UploadAvatar(AuthAvatarUploadForm form)
        {
            try
            {
                if (form?.Avatar == null)
                {
                    throw new ArgumentNullException("Nelze nahrát prázdný avatar");
                }

                if (!this.CurrentUser.IsAuthenticated)
                {
                    throw new UIException(UIExceptionEnum.NotAuthenticated);
                }

                Services.FileProvider.SaveImage(form.Avatar, FileConfig.AvatarFolderPath, Entity.ApplicationUser.GetAvatarFileName(this.CurrentUser.UserName), FileConfig.AvatarSideSize, FileConfig.AvatarSideSize);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

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

            var companyQuery = Services.CompanyService.GetAll()
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

            var companyCategoriesQuery = this.Services.CompanyCategoryService.GetAll()
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

            var internshipCategoriesQuery = this.Services.InternshipCategoryService.GetAll()
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
