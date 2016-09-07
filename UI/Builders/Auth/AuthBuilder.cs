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

namespace UI.Builders.Company
{
    public class AuthBuilder : BuilderAbstract
    {
        #region Services

        ICompanyService companyService;
        ICompanyCategoryService companyCategoryService;
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
            IFileProvider fileProvider)
            : base(
                appContext,
                cacheService,
                identityService,
                logService)
        {
            this.companyService = companyService;
            this.companyCategoryService = companyCategoryService;
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
            form.CompanyCategories = await GetCompanyCategories();

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
            form.CompanyCategories = await GetCompanyCategories();

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
                    ShortDescription = m.ShortDescription,
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
            company.CompanyCategories = await GetCompanyCategories();

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
                    ShortDescription = m.ShortDescription,
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
            company.CompanyCategories = await GetCompanyCategories();

            return new AuthRegisterCompanyView()
            {
                CompanyForm = company,
                CompanyIsCreated = company != null
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
                fileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, StringHelper.GetCodeName(form.CompanyName), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerWidth);
                fileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, StringHelper.GetCodeName(form.CompanyName), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);

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
                    ShortDescription = form.ShortDescription,
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
                fileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, StringHelper.GetCodeName(form.CompanyName), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerWidth);
                fileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, StringHelper.GetCodeName(form.CompanyName), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);

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
                    ShortDescription = form.ShortDescription,
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

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets company categories and caches the result
        /// </summary>
        /// <returns>Collection of company categories</returns>
        private async Task<IEnumerable<AuthCompanyCategoryModel>> GetCompanyCategories()
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

        #endregion
    }
}
