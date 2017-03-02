using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Config;
using Core.Helpers;
using Service.Exceptions;
using Service.Extensions;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;
using UI.UIServices;

namespace UI.Builders.Auth
{
    public class AuthCompanyBuilder : AuthMasterBuilder
    {
        #region Constructor

        public AuthCompanyBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Actions 

        public async Task<CompanyGalleryView> BuildCompanyGalleryViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new CompanyGalleryView()
            {
                AuthMaster = authMaster
            };
        }

        public async Task<AuthRegisterCompanyView> BuildRegisterCompanyViewAsync(AuthAddEditCompanyForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();
            var companyCreated = false;

            if (authMaster == null)
            {
                return null;
            }

            if (form == null)
            {
                // user haven't created any company yet
                form = new AuthAddEditCompanyForm();
            }
            else
            {
                companyCreated = true;
            }

            // add countries, categories and company sizes
            form.Countries = await FormGetCountriesAsync();
            form.CompanySizes = await FormGetCompanySizesAsync();
            form.CompanyCategories = await FormGetCompanyCategoriesAsync();

            // add guid
            if (CurrentCompany.IsAvailable)
            {
                form.CompanyGuid = CurrentCompany.CompanyGuid;
            }

            return new AuthRegisterCompanyView()
            {
                AuthMaster = authMaster,
                CompanyForm = form,
                CompanyIsCreated = companyCreated
            };
        }

        public async Task<AuthEditCompanyView> BuildEditCompanyViewAsync(AuthAddEditCompanyForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            if (form == null)
            {
                // invalid form data
                return null;
            }

            // add countries, categories and company sizes
            form.Countries = await FormGetCountriesAsync();
            form.CompanySizes = await FormGetCompanySizesAsync();
            form.CompanyCategories = await FormGetCompanyCategoriesAsync();

            // add guid
            if (CurrentCompany.IsAvailable)
            {
                form.CompanyGuid = CurrentCompany.CompanyGuid;
            }

            return new AuthEditCompanyView()
            {
                AuthMaster = authMaster,
                CompanyForm = form,
            };
        }

        public async Task<AuthEditCompanyView> BuildEditCompanyViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            // get company assigned to user
            var company = await Services.CompanyService.GetAll()
                .ForUser(CurrentUser.Id)
                .Take(1)
                .Select(m => new AuthAddEditCompanyForm()
                {
                    Address = m.Address,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    CompanySizeID = m.CompanySizeID,
                    CompanySizeName = m.CompanySize.CompanySizeName,
                    CountryID = m.CountryID,
                    CountryCode = m.Country.CountryCode,
                    CountryName = m.Country.CountryName,
                    Facebook = m.Facebook,
                    ID = m.ID,
                    CompanyGuid = m.Guid,
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
            company.Countries = await FormGetCountriesAsync();
            company.CompanySizes = await FormGetCompanySizesAsync();
            company.CompanyCategories = await FormGetCompanyCategoriesAsync();

            return new AuthEditCompanyView()
            {
                AuthMaster = authMaster,
                CompanyForm = company,
            };
        }

        public async Task<AuthRegisterCompanyView> BuildRegisterCompanyViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();
            var comapnyIsCreated = false;

            if (authMaster == null)
            {
                return null;
            }

            // get company assigned to user
            var company = await Services.CompanyService.GetAll()
                .ForUser(CurrentUser.Id)
                .Take(1)
                .Select(m => new AuthAddEditCompanyForm()
                {
                    Address = m.Address,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    CompanySizeID = m.CompanySizeID,
                    CountryID = m.CountryID,
                    CountryName = m.Country.CountryName,
                    CountryCode = m.Country.CountryCode,
                    Facebook = m.Facebook,
                    ID = m.ID,
                    CompanyGuid = m.Guid,
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
                company = new AuthAddEditCompanyForm();
            }
            else
            {
                comapnyIsCreated = true;
            }

            // add countries, categories and company sizes
            company.Countries = await FormGetCountriesAsync();
            company.CompanySizes = await FormGetCompanySizesAsync();
            company.CompanyCategories = await FormGetCompanyCategoriesAsync();

            return new AuthRegisterCompanyView()
            {
                AuthMaster = authMaster,
                CompanyForm = company,
                CompanyIsCreated = comapnyIsCreated
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
            var companyGuidShared = Guid.Empty;

            // Create company in transaction because files require CompanyID
            using (var transaction = AppContext.BeginTransaction())
            {
                try
                {
                    // verify company URL
                    if (!StringHelper.IsValidUrl(form.Web))
                    {
                        throw new ValidationException("URL webu není validní. (zkontrolujte protokol)");
                    }

                    // verify country
                    if (!await IsValidCountry(form.CountryID))
                    {
                        throw new ValidationException("Vyplň stát");
                    }

                    var company = new Entity.Company
                    {
                        Address = form.Address,
                        City = form.City,
                        CompanyName = form.CompanyName,
                        CompanySizeID = form.CompanySizeID,
                        CountryID = form.CountryID,
                        Facebook = form.Facebook,
                        Lat = form.Lat,
                        LinkedIn = form.LinkedIn,
                        Lng = form.Lng,
                        LongDescription = form.LongDescription,
                        PublicEmail = form.PublicEmail,
                        Twitter = form.Twitter,
                        Web = form.Web,
                        YearFounded = form.YearFounded,
                        CompanyCategoryID = form.CompanyCategoryID,
                    };

                    await Services.CompanyService.InsertAsync(company);

                    // prepare company folder
                    PrepareCompanyDirectories(company.Guid);
                    companyGuidShared = company.Guid;

                    // upload files
                    if (form.Banner != null)
                    {
                        Services.FileProvider.SaveImage(form.Banner, Entity.Company.GetCompanyBannerFolderPath(company.Guid), Entity.Company.GetBannerFileName(), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerHeight);
                    }

                    if (form.Logo != null)
                    {
                        Services.FileProvider.SaveImage(form.Logo, Entity.Company.GetCompanyLogoFolderPath(company.Guid), Entity.Company.GetLogoFileName(), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
                    }

                    // commit transaction
                    transaction.Commit();

                    return company.ID;
                }
                catch (FileUploadException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // cleanup directory
                    if (companyGuidShared != Guid.Empty)
                    {
                        CleanupCompanyDirectories(companyGuidShared);
                    }

                    // log erros
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException($"{ex.Message}", ex);
                }
                catch (CodeNameNotUniqueException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // cleanup directory
                    if (companyGuidShared != Guid.Empty)
                    {
                        CleanupCompanyDirectories(companyGuidShared);
                    }

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException($"{form.CompanyName} je již v databázi", ex);
                }
                catch (ValidationException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // cleanup directory
                    if (companyGuidShared != Guid.Empty)
                    {
                        CleanupCompanyDirectories(companyGuidShared);
                    }

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    // rollback
                    transaction.Rollback();

                    // cleanup directory
                    if (companyGuidShared != Guid.Empty)
                    {
                        CleanupCompanyDirectories(companyGuidShared);
                    }

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(UiExceptionEnum.SaveFailure, ex);
                }
            }
        }


        /// <summary>
        /// Edits company
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditCompany(AuthAddEditCompanyForm form)
        {
            using (var transaction = AppContext.BeginTransaction())
            {
                try
                {
                    if (!CurrentCompany.IsAvailable)
                    {
                        throw new ValidationException($"Firma, kterou chcete editovat nebyla nalezena");
                    }

                    // verify company URL
                    if (!StringHelper.IsValidUrl(form.Web))
                    {
                        throw new ValidationException($"Zadejte validní URL webu");
                    }

                    // verify country
                    if (!await IsValidCountry(form.CountryID))
                    {
                        throw new ValidationException("Vyplňte stát");
                    }

                    // upload files if they are provided
                    if (form.Banner != null)
                    {
                        Services.FileProvider.SaveImage(form.Banner, Entity.Company.GetCompanyBannerFolderPath(CurrentCompany.CompanyGuid), Entity.Company.GetBannerFileName(), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerHeight);
                    }
                    if (form.Logo != null)
                    {
                        Services.FileProvider.SaveImage(form.Logo, Entity.Company.GetCompanyLogoFolderPath(CurrentCompany.CompanyGuid), Entity.Company.GetLogoFileName(), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
                    }

                    var company = new Entity.Company
                    {
                        ID = form.ID,
                        Address = form.Address,
                        City = form.City,
                        CompanyName = form.CompanyName,
                        CompanySizeID = form.CompanySizeID,
                        CountryID = form.CountryID,
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
                catch (FileUploadException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log erros
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException($"{ex.Message}", ex);
                }
                catch (CodeNameNotUniqueException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException($"Firma {form.CompanyName} je již v databázi", ex);
                }
                catch (ValidationException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(UiExceptionEnum.SaveFailure, ex);
                }
            }
        }

        #endregion

        #region Helper methods

        private async Task<bool> IsValidCountry(int countryID)
        {
            return (await FormGetCountriesAsync()).FirstOrDefault(m => m.ID == countryID) != null;
        }

        private void PrepareCompanyDirectories(Guid companyGuid)
        {
            // create base folder
            var baseFolderPath = SystemConfig.ServerRootPath + "\\" + Entity.Company.GetCompanyBaseFolderPath(companyGuid);
            Directory.CreateDirectory(baseFolderPath);

            // gallery folder
            var galleryPath = SystemConfig.ServerRootPath + "\\" + Entity.Company.GetCompanyGalleryFolderPath(companyGuid);
            Directory.CreateDirectory(galleryPath);

            // logo folder
            var logoPath = SystemConfig.ServerRootPath + "\\" + Entity.Company.GetCompanyLogoFolderPath(companyGuid);
            Directory.CreateDirectory(logoPath);

            // banner 
            var bannerPath = SystemConfig.ServerRootPath + "\\" + Entity.Company.GetCompanyBannerFolderPath(companyGuid);
            Directory.CreateDirectory(bannerPath);
        }

        private void CleanupCompanyDirectories(Guid companyGuid)
        {
            // delete base folder
            try
            {
                var baseFolderPath = SystemConfig.ServerRootPath + "\\" + Entity.Company.GetCompanyBaseFolderPath(companyGuid);
                Directory.Delete(baseFolderPath, true);
            }
            catch (DirectoryNotFoundException)
            {
                // nothing is needed to do here
            }
        }

        #endregion

    }
}
