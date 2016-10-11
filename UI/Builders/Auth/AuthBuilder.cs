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
using PagedList;
using PagedList.EntityFramework;
using Entity;

namespace UI.Builders.Company
{
    public class AuthBuilder : BaseBuilder
    {

        #region Constructor

        public AuthBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Index

        public async Task<AuthIndexView> BuildIndexViewAsync(int? page)
        {
            if (!this.CurrentUser.IsAuthenticated)
            {
                return null;
            }

            return new AuthIndexView()
            {
                Internships = await GetInternshipsAsync(),
                Messages = await GetMessagesAsync(page),
                NotReadMessagesCount = await GetNotReadMessagesOfCurrentUserAsync()
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
            form.Countries = await FormGetCountriesAsync();
            form.CompanySizes = await FormGetCompanySizesAsync();
            form.CompanyCategories = await FormGetCompanyCategories();

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
            form.Countries = await FormGetCountriesAsync();
            form.CompanySizes = await FormGetCompanySizesAsync();
            form.CompanyCategories = await FormGetCompanyCategories();

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
                    CompanySizeID = m.CompanySizeID,
                    CompanySizeName = m.CompanySize.CompanySizeName,
                    CountryID = m.CountryID,
                    CountryCode = m.Country.CountryCode,
                    CountryName = m.Country.CountryName,
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
            company.Countries = await FormGetCountriesAsync();
            company.CompanySizes = await FormGetCompanySizesAsync();
            company.CompanyCategories = await FormGetCompanyCategories();

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
                    CompanySizeID = m.CompanySizeID,
                    CountryID = m.CountryID,
                    CountryName = m.Country.CountryName,
                    CountryCode = m.Country.CountryCode,
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
            company.Countries = await FormGetCountriesAsync();
            company.CompanySizes = await FormGetCompanySizesAsync();
            company.CompanyCategories = await FormGetCompanyCategories();
            
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
                    AmountTypeID = m.AmountTypeID,
                    City = m.City,
                    CountryID = m.CountryID,
                    CurrencyID = m.CurrencyID,
                    Description = m.Description,
                    ID = m.ID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    IsPaid = m.IsPaid ? "on" : "",
                    MaxDurationTypeID = m.MaxDurationTypeID,
                    MinDurationTypeID = m.MinDurationTypeID,
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
                    WorkingHours = m.WorkingHours,
                    Requirements = m.Requirements,
                    MinDurationTypeCodeName = m.MinDurationType.CodeName,
                    MaxDurationTypeCodeName = m.MaxDurationType.CodeName
                });

            var internship = await internshipQuery.FirstOrDefaultAsync();

            // internship was not found
            if (internship == null)
            {
                return null;
            }

            // initialize form values
            internship.MaxDurationTypeEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MaxDurationTypeCodeName);
            internship.MinDurationTypeEnum = EnumHelper.ParseEnum<InternshipDurationTypeEnum>(internship.MinDurationTypeCodeName);
            internship.InternshipCategories = await FormGetInternshipCategoriesAsync();
            internship.AmountTypes = await FormGetInternshipAmountTypesAsync();
            internship.DurationTypes = await FormGetInternshipDurationsAsync();
            internship.Countries = await FormGetCountriesAsync();
            internship.Currencies = await FormGetCurrenciesAsync();

            // set default duration
            var minDurationEnum = internship.MinDurationTypeEnum;
            var maxDurationEnum = internship.MaxDurationTypeEnum;

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
                InternshipCategories = await FormGetInternshipCategoriesAsync(),
                AmountTypes = await FormGetInternshipAmountTypesAsync(),
                DurationTypes = await FormGetInternshipDurationsAsync(),
                Countries = await FormGetCountriesAsync(),
                Currencies = await FormGetCurrenciesAsync(),
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

            form.InternshipCategories = await FormGetInternshipCategoriesAsync();
            form.AmountTypes = await FormGetInternshipAmountTypesAsync();
            form.DurationTypes = await FormGetInternshipDurationsAsync();
            form.Countries = await FormGetCountriesAsync();
            form.Currencies = await FormGetCurrenciesAsync();

            return new AuthEditInternshipView()
            {
                InternshipForm = form,
            };
        }

        public async Task<AuthNewInternshipView> BuildNewInternshipViewAsync(AuthAddEditInternshipForm form)
        {

            form.InternshipCategories = await FormGetInternshipCategoriesAsync();
            form.AmountTypes = await FormGetInternshipAmountTypesAsync();
            form.DurationTypes = await FormGetInternshipDurationsAsync();
            form.Countries = await FormGetCountriesAsync();
            form.Currencies = await FormGetCurrenciesAsync();

            return new AuthNewInternshipView()
            {
                InternshipForm = form,
                CanCreateInternship = await GetCompanyIDOfCurrentUserAsync() != 0, // user can create internship only if he created company before
            };
        }

        #endregion

        #region Conversation messages

        public async Task<AuthConversationView> BuildConversationViewAsync(string otherUserId, int? page, AuthMessageForm messageForm = null)
        {
            var defaultMessageForm = new AuthMessageForm()
            {
                RecipientApplicationUserId = otherUserId
            };

            // don't show anything if recipient == current user. It should always be the other user
            if (otherUserId.Equals(this.CurrentUser.Id, StringComparison.OrdinalIgnoreCase))
            {
                // to do maybe
            }

            // get other user and check if he exists
            var otherUser = await GetMessageUserAsync(otherUserId);

            if (otherUser == null)
            {
                return null;
            }

            // get messages 
            var messages = await GetConversationMessagesAsync(otherUserId, page);

            // mark messages as read if the last message's recipient is current user
            var lastMessage = messages.FirstOrDefault();
            if (lastMessage != null)
            {
                if (lastMessage.RecipientApplicationUserId.Equals(this.CurrentUser.Id, StringComparison.OrdinalIgnoreCase))
                {
                    await this.Services.MessageService.MarkMessagesAsRead(this.CurrentUser.Id, otherUserId);

                    // mark all loaded messages as read
                    foreach (var message in messages)
                    {
                        message.IsRead = true;
                    }
                }
            }

            return new AuthConversationView()
            {
                Messages = messages,
                ConversationUser = otherUser,
                Me = new AuthMessageUserModel()
                {
                    FirstName = this.CurrentUser.FirstName,
                    LastName = this.CurrentUser.LastName,
                    UserID = this.CurrentUser.Id,
                    UserName = this.CurrentUser.UserName
                },
                MessageForm = messageForm == null ? defaultMessageForm : messageForm
            };
        }

        #endregion

        #region Methods

        public async Task<int> CreateMessage(AuthMessageForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new UIException("Pro odeslání zprávy se přihlašte");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = this.CurrentUser.Id,
                    RecipientCompanyID = form.CompanyID,
                    RecipientApplicationUserId = form.RecipientApplicationUserId,
                    MessageText = form.Message,
                    Subject = null, // no subject needed
                    IsRead = false,
                };

                return await this.Services.MessageService.InsertAsync(message);
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

                    // verify company URL
                    if (!StringHelper.IsValidUrl(form.Web))
                    {
                        throw new ValidationException($"Zadejte validní URL webu");
                    }

                    var company = new Entity.Company
                    {
                        ApplicationUserId = this.CurrentUser.Id,
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
                catch (ValidationException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException(ex.Message, ex);
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
                    // verify company URL
                    if (!StringHelper.IsValidUrl(form.Web))
                    {
                        throw new ValidationException($"Zadejte validní URL webu");

                    }
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
                catch (CodeNameNotUniqueException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException($"Firma {form.CompanyName} je již v databázi", ex);
                }
                catch (ValidationException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException(ex.Message, ex);
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
                    throw new ValidationException("Stáž musí být přiřazena k firmě");
                }

                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can create internship
                    throw new ValidationException("Nelze vytvořit stáž bez příhlášení");
                }

                // Get enums from duration ID to calculate durations
                var maxDurationTypeEnum = ((await GetDurationTypeAsync(form.MaxDurationTypeID)).DurationTypeEnum);
                var minDurationTypeEnum = ((await GetDurationTypeAsync(form.MinDurationTypeID)).DurationTypeEnum);

                var internship = new Entity.Internship
                {
                    Amount = form.Amount,
                    AmountTypeID = form.AmountTypeID,
                    City = form.City,
                    CountryID = form.CountryID,
                    StartDate = form.StartDate,
                    Title = form.Title,
                    IsPaid = form.GetIsPaid(),
                    CompanyID = companyIDOfCurrentUser,
                    InternshipCategoryID = form.InternshipCategoryID,
                    CurrencyID = form.CurrencyID,
                    Description = form.Description,
                    MaxDurationInDays = form.GetDurationInDays(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInWeeks = form.GetDurationInWeeks(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInMonths = form.GetDurationInMonths(maxDurationTypeEnum, form.MaxDuration),
                    MinDurationInDays = form.GetDurationInDays(minDurationTypeEnum, form.MinDuration),
                    MinDurationInWeeks = form.GetDurationInWeeks(minDurationTypeEnum, form.MinDuration),
                    MinDurationInMonths = form.GetDurationInMonths(minDurationTypeEnum, form.MinDuration),
                    MinDurationTypeID = form.MinDurationTypeID,
                    MaxDurationTypeID = form.MaxDurationTypeID,
                    IsActive = form.GetIsActive(),
                    ApplicationUserId = this.CurrentUser.Id,
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours,
                    Requirements = form.Requirements
                };

                await Services.InternshipService.InsertAsync(internship);

                return internship.ID;
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
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
                    throw new ValidationException("Nelze vytvořit stáž bez firmy");
                }

                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can create internship
                    throw new ValidationException("Nelze vytvořit stáž bez příhlášení");
                }

                // Get enums from duration ID to calculate durations
                var maxDurationTypeEnum = ((await GetDurationTypeAsync(form.MaxDurationTypeID)).DurationTypeEnum);
                var minDurationTypeEnum = ((await GetDurationTypeAsync(form.MinDurationTypeID)).DurationTypeEnum);

                var internship = new Entity.Internship
                {
                    ID = form.ID,
                    Amount = form.Amount,
                    AmountTypeID = form.AmountTypeID,
                    City = form.City,
                    CountryID = form.CountryID,
                    StartDate = form.StartDate,
                    Title = form.Title,
                    IsPaid = form.GetIsPaid(), //TODO
                    CompanyID = companyIDOfCurrentUser,
                    InternshipCategoryID = form.InternshipCategoryID,
                    CurrencyID = form.CurrencyID,
                    Description = form.Description,
                    MaxDurationInDays = form.GetDurationInDays(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInWeeks = form.GetDurationInWeeks(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInMonths = form.GetDurationInMonths(maxDurationTypeEnum, form.MaxDuration),
                    MinDurationInDays = form.GetDurationInDays(minDurationTypeEnum, form.MinDuration),
                    MinDurationInWeeks = form.GetDurationInWeeks(minDurationTypeEnum, form.MinDuration),
                    MinDurationInMonths = form.GetDurationInMonths(minDurationTypeEnum, form.MinDuration),
                    MinDurationTypeID = form.MinDurationTypeID,
                    MaxDurationTypeID = form.MaxDurationTypeID,
                    IsActive = form.GetIsActive(),
                    ApplicationUserId = this.CurrentUser.Id,
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours,
                    Requirements = form.Requirements
                };

                await Services.InternshipService.UpdateAsync(internship);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
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
                    throw new ValidationException("Nelze nahrát prázdný avatar");
                }

                if (!this.CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException(UIExceptionEnum.NotAuthenticated.ToString());
                }

                Services.FileProvider.SaveImage(form.Avatar, FileConfig.AvatarFolderPath, Entity.ApplicationUser.GetAvatarFileName(this.CurrentUser.UserName), FileConfig.AvatarSideSize, FileConfig.AvatarSideSize);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
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

        private async Task<AuthInternshipDurationType> GetDurationTypeAsync(int durationID)
        {
            var durationTypes = await this.FormGetInternshipDurationsAsync();
            return durationTypes
                .Where(m => m.ID == durationID)
                .SingleOrDefault();
        }

        private async Task<IEnumerable<AuthInternshipAmountType>> FormGetInternshipAmountTypesAsync()
        {
            var internshipAmountTypes = await this.Services.InternshipAmountTypeService.GetAllCachedAsync();

            return internshipAmountTypes.Select(m => new AuthInternshipAmountType()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                AmountTypeName = m.AmountTypeName
            });
        }

        private async Task<IEnumerable<AuthInternshipDurationType>> FormGetInternshipDurationsAsync()
        {
            var durationTypes = await this.Services.InternshipDurationTypeService.GetAllCachedAsync();

            return durationTypes.Select(m => new AuthInternshipDurationType()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                DurationName = m.DurationName,
                DurationTypeEnum = m.DurationTypeEnum
            });
        }

        private async Task<IEnumerable<AuthCurrencyModel>> FormGetCurrenciesAsync()
        {
            var currencies = await this.Services.CurrencyService.GetAllCachedAsync();

            return currencies.Select(m => new AuthCurrencyModel()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                CurrencyName = m.CurrencyName
            });
        }

        private async Task<IEnumerable<AuthCountryModel>> FormGetCountriesAsync()
        {
            var countries = await this.Services.CountryService.GetAllCachedAsync();

            return countries.Select(m => new AuthCountryModel()
            {
                ID = m.ID,
                CountryCode = m.CountryCode,
                CountryName = m.CountryName,
                Icon = m.Icon
            });
        }

        private async Task<IEnumerable<AuthCompanySize>> FormGetCompanySizesAsync()
        {
            var companySizes = await this.Services.CompanySizeService.GetAllCachedAsync();

            return companySizes.Select(m => new AuthCompanySize()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                CompanySizeName = m.CompanySizeName
            });
        }

        /// <summary>
        /// Gets total number of not read messages of current user
        /// </summary>
        /// <param name="userID">ID of user whose messages will be retrieved</param>
        /// <returns>Number of not read messages</returns>
        private async Task<int> GetNotReadMessagesOfCurrentUserAsync()
        {
            var messagesQuery = this.Services.MessageService.GetAll()
               .Where(m => m.RecipientApplicationUserId == this.CurrentUser.Id && !m.IsRead)
               .Select(m => m.ID);

            int cacheMinutes = 30;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthMessageModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Message>(),
                EntityKeys.KeyDeleteAny<Entity.Message>(),
                EntityKeys.KeyCreateAny<Entity.Message>(),
            };
            cacheSetup.ObjectStringID = this.CurrentUser.Id;

            var result = await this.Services.CacheService.GetOrSet(async () => await messagesQuery.ToListAsync(), cacheSetup);

            return result.Count();
        }

        /// <summary>
        /// Gets conversations for current user
        /// </summary>
        /// <param name="otherUserId">ID of the other user (NEVER current user)</param>
        /// <param name="page">Page number</param>
        /// <returns>Collection of messages of current user with other user</returns>
        private async Task<IPagedList<AuthMessageModel>> GetConversationMessagesAsync(string otherUserId, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var messagesQuery = this.Services.MessageService.GetAll()
               .Where(m =>
                    (m.RecipientApplicationUserId == otherUserId || m.RecipientApplicationUserId == this.CurrentUser.Id)
                    &&
                    (m.SenderApplicationUserId == otherUserId || m.SenderApplicationUserId == this.CurrentUser.Id))
               .OrderByDescending(m => m.MessageCreated)
               .Select(m => new AuthMessageModel()
               {
                   ID = m.ID,
                   MessageCreated = m.MessageCreated,
                   SenderApllicationUserId = m.SenderApplicationUserId,
                   SenderApplicationUserName = m.SenderApplicationUser.UserName,
                   RecipientApplicationUserId = m.RecipientApplicationUserId,
                   RecipientApplicationUserName = m.RecipientApplicationUser.UserName,
                   CompanyID = m.RecipientCompanyID,
                   CompanyName = m.Company.CompanyName,
                   MessageText = m.MessageText,
                   CurrentUserId = this.CurrentUser.Id,
                   IsRead = m.IsRead,
                   SenderFirstName = m.SenderApplicationUser.FirstName,
                   SenderLastName = m.SenderApplicationUser.LastName,
                   RecipientFirstName = m.RecipientApplicationUser.FirstName,
                   RecipientLastName = m.RecipientApplicationUser.LastName,
                   Subject = m.Subject,
               });

            int cacheMinutes = 30;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthMessageModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Message>(),
                EntityKeys.KeyDeleteAny<Entity.Message>(),
                EntityKeys.KeyCreateAny<Entity.Message>(),
            };
            cacheSetup.ObjectStringID = this.CurrentUser.Id + "_" + otherUserId;
            cacheSetup.PageNumber = pageNumber;
            cacheSetup.PageSize = pageSize;

            return await this.Services.CacheService.GetOrSet(async () => await messagesQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);
        }

        /// <summary>
        /// Gets name of given user
        /// </summary>
        /// <param name="applicationUserId">ID of user</param>
        /// <returns>Name of given user</returns>
        private async Task<AuthMessageUserModel> GetMessageUserAsync(string applicationUserId)
        {
            int cacheMinutes = 30;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthMessageUserModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Message>(),
            };
            cacheSetup.ObjectStringID = applicationUserId;

            var userQuery = this.Services.IdentityService.GetSingle(applicationUserId)
                .Select(m => new AuthMessageUserModel()
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    UserID = m.Id,
                    UserName = m.UserName
                });

            return await this.Services.CacheService.GetOrSet(async () => await userQuery.FirstOrDefaultAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets messages of current user
        /// </summary>
        /// <returns>Collection of messages of current user</returns>
        private async Task<IPagedList<AuthMessageModel>> GetMessagesAsync(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // get both incoming and outgoming messages as well as messages targeted for given company
            var messagesQuery = this.Services.MessageService.GetAll()
               .Where(m => m.SenderApplicationUserId == this.CurrentUser.Id || m.Company.ApplicationUserId == this.CurrentUser.Id || m.RecipientApplicationUserId == this.CurrentUser.Id)
               .OrderByDescending(m => m.MessageCreated)
               .Select(m => new AuthMessageModel()
               {
                   ID = m.ID,
                   MessageCreated = m.MessageCreated,
                   SenderApllicationUserId = m.SenderApplicationUserId,
                   SenderApplicationUserName = m.SenderApplicationUser.UserName,
                   RecipientApplicationUserId = m.RecipientApplicationUserId,
                   RecipientApplicationUserName = m.RecipientApplicationUser.UserName,
                   CompanyID = m.RecipientCompanyID,
                   CompanyName = m.Company.CompanyName,
                   MessageText = m.MessageText,
                   CurrentUserId = this.CurrentUser.Id,
                   IsRead = m.IsRead,
                   SenderFirstName = m.SenderApplicationUser.FirstName,
                   SenderLastName = m.SenderApplicationUser.LastName,
                   RecipientFirstName = m.RecipientApplicationUser.FirstName,
                   RecipientLastName = m.RecipientApplicationUser.LastName,
                   Subject = m.Subject,
               });

            int cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthMessageModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Message>(),
                EntityKeys.KeyDeleteAny<Entity.Message>(),
                EntityKeys.KeyCreateAny<Entity.Message>(),
            };
            cacheSetup.ObjectStringID = this.CurrentUser.Id;
            cacheSetup.PageNumber = pageNumber;
            cacheSetup.PageSize = pageSize;

            return await this.Services.CacheService.GetOrSet(async () => await messagesQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);
        }

        /// <summary>
        /// Gets internships of current user
        /// </summary>
        /// <returns>Collection of internships of current user</returns>
        private async Task<IEnumerable<AuthInternshipListingModel>> GetInternshipsAsync()
        {
            var internshipsQuery = this.Services.InternshipService.GetAll()
               .Where(m => m.ApplicationUserId == this.CurrentUser.Id)
               .OrderByDescending(m => m.Created)
               .Select(m => new AuthInternshipListingModel()
               {
                   ID = m.ID,
                   Title = m.Title,
                   Created = m.Created,
                   IsActive = m.IsActive,
                   CodeName = m.CodeName
               });

            int cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthInternshipListingModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
            };
            cacheSetup.ObjectStringID = this.CurrentUser.Id;

            return await this.Services.CacheService.GetOrSet(async () => await internshipsQuery.ToListAsync(), cacheSetup);
        }

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
            var cacheSetup = this.Services.CacheService.GetSetup<int>(GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Company>(),
                EntityKeys.KeyDeleteAny<Entity.Company>(),
            };

            var company = await this.Services.CacheService.GetOrSet(async () => await companyQuery.FirstOrDefaultAsync(), cacheSetup);

            return company;
        }

        /// <summary>
        /// Gets company categories and caches the result
        /// </summary>
        /// <returns>Collection of company categories</returns>
        private async Task<IEnumerable<AuthCompanyCategoryModel>> FormGetCompanyCategories()
        {
            var cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthCompanyCategoryModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.CompanyCategory>(),
                EntityKeys.KeyDeleteAny<Entity.CompanyCategory>(),
                EntityKeys.KeyUpdateAny<Entity.CompanyCategory>(),
            };

            var companyCategoriesQuery = this.Services.CompanyCategoryService.GetAll()
                .Select(m => new AuthCompanyCategoryModel()
                {
                    CompanyCategoryID = m.ID,
                    CompanyCategoryName = m.Name
                });

            var companyCategories = await this.Services.CacheService.GetOrSetAsync(async () => await companyCategoriesQuery.ToListAsync(), cacheSetup);

            return companyCategories;
        }

        /// <summary>
        /// Gets internship categories and caches the result
        /// </summary>
        /// <returns>Collection of internship categories</returns>
        private async Task<IEnumerable<AuthInternshipCategoryModel>> FormGetInternshipCategoriesAsync()
        {
            var cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthInternshipCategoryModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.InternshipCategory>(),
                EntityKeys.KeyDeleteAny<Entity.InternshipCategory>(),
                EntityKeys.KeyUpdateAny<Entity.InternshipCategory>(),
            };

            var internshipCategoriesQuery = this.Services.InternshipCategoryService.GetAll()
                .Select(m => new AuthInternshipCategoryModel()
                {
                    InternshipCategoryID = m.ID,
                    InternshipCategoryName = m.Name
                });

            var internshipCategories = await this.Services.CacheService.GetOrSetAsync(async () => await internshipCategoriesQuery.ToListAsync(), cacheSetup);

            return internshipCategories;
        }

        #endregion
    }
}
