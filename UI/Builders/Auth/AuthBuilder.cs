using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using System;
using System.Collections.Generic;
using PagedList;
using PagedList.EntityFramework;

using UI.Base;
using UI.Builders.Auth.Views;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Models;
using Core.Config;
using Core.Helpers;
using UI.Exceptions;
using Core.Helpers.Internship;
using UI.Builders.Services;
using Service.Exceptions;
using Entity;
using UI.UIServices;
using UI.Builders.Shared.Models;
using System.Web;
using System.IO;

namespace UI.Builders.Company
{
    public class AuthBuilder : BaseBuilder
    {

        #region Constructor

        public AuthBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Master view

        public async Task<AuthMasterModel> GetAuthMasterModelAsync()
        {
            if (!this.CurrentUser.IsAuthenticated)
            {
                return null;
            }

            return new AuthMasterModel()
            {
                Internships = await GetInternshipsAsync(),
                Conversations = await GetConversationsAsync(10),
                Theses = await GetThesesListingsAsync()
            };
        }

        #endregion

        #region Index

        public async Task<AuthIndexView> BuildIndexViewAsync(int? page)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthIndexView()
            {
                AuthMaster = authMaster,
                ConversationsPaged = await GetConversationsAsync(page),
            };
        }

        #endregion

        #region Edit Profile

        public async Task<AuthEditProfileView> BuildEditProfileViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
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
                AuthMaster = authMaster,
                ProfileForm = form
            };
        }

        public async Task<AuthEditProfileView> BuildEditProfileViewAsync(AuthEditProfileForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthEditProfileView()
            {
                AuthMaster = authMaster,
                ProfileForm = form
            };
        }

        #endregion

        #region Company Gallery

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

        #endregion

        #region Avatar 

        public async Task<AuthAvatarView> BuildAvatarViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

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
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            if (form == null)
            {
                // user haven't created any company yet
                form = new AuthAddEditCompanyForm();
            }

            // add countries, categories and company sizes
            form.Countries = await FormGetCountriesAsync();
            form.CompanySizes = await FormGetCompanySizesAsync();
            form.CompanyCategories = await FormGetCompanyCategories();

            // add guid
            if (this.CurrentCompany.IsAvailable)
            {
                form.CompanyGuid = this.CurrentCompany.CompanyGUID;
            }

            return new AuthRegisterCompanyView()
            {
                AuthMaster = authMaster,
                CompanyForm = form,
                CompanyIsCreated = form != null
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
            form.CompanyCategories = await FormGetCompanyCategories();

            // add guid
            if (this.CurrentCompany.IsAvailable)
            {
                form.CompanyGuid = this.CurrentCompany.CompanyGUID;
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
                    CompanyGuid = m.CompanyGUID,
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
                AuthMaster = authMaster,
                CompanyForm = company,
            };
        }

        public async Task<AuthRegisterCompanyView> BuildRegisterCompanyViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;

            }
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
                    CompanyGuid = m.CompanyGUID,
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
                AuthMaster = authMaster,
                CompanyForm = company,
                CompanyIsCreated = company != null
            };
        }

        #endregion

        #region Thesis

        public async Task<AuthEditThesisView> BuildEditThesisViewAsync(int thesisID)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var thesisQuery = this.Services.ThesisService.GetAll()
                .Where(m => m.ID == thesisID)
                .Select(m => new AuthAddEditThesisForm()
                {
                    ID = m.ID,
                    Amount = m.Amount,
                    CurrencyID = m.CurrencyID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    Description = m.Description,
                    IsPaid = m.IsPaid ? "on" : "",
                    IsActive = m.IsActive ? "on" : "",
                    ThesisName = m.ThesisName,
                    ThesisTypeID = m.ThesisTypeID,
                });

            var thesis = await thesisQuery.FirstOrDefaultAsync();

            // thesis with given ID does not exist
            if (thesis == null)
            {
                return null;
            }

            // initialize form values
            thesis.Currencies = await FormGetCurrenciesAsync();
            thesis.Categories = await FormGetInternshipCategoriesAsync();
            thesis.ThesisTypes = await FormGetThesisTypesAsync();

            return new AuthEditThesisView()
            {
                AuthMaster = authMaster,
                ThesisForm = thesis
            };
        }

        public async Task<AuthNewThesisView> BuildNewThesisViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var form = new AuthAddEditThesisForm()
            {
                Currencies = await FormGetCurrenciesAsync(),
                ThesisTypes = await FormGetThesisTypesAsync(),
                Categories = await FormGetInternshipCategoriesAsync(),
                IsActive = "on", // thesis is active by default
            };

            return new AuthNewThesisView()
            {
                AuthMaster = authMaster,
                ThesisForm = form,
                CanCreateThesis = this.CurrentCompany.IsAvailable
            };
        }

        public async Task<AuthEditThesisView> BuildEditThesisViewAsync(AuthAddEditThesisForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            form.Categories = await FormGetInternshipCategoriesAsync();
            form.Currencies = await FormGetCurrenciesAsync();
            form.ThesisTypes = await FormGetThesisTypesAsync();

            return new AuthEditThesisView()
            {
                AuthMaster = authMaster,
                ThesisForm = form,
            };
        }

        public async Task<AuthNewThesisView> BuildNewThesisViewAsync(AuthAddEditThesisForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            form.Categories = await FormGetInternshipCategoriesAsync();
            form.Currencies = await FormGetCurrenciesAsync();
            form.ThesisTypes = await FormGetThesisTypesAsync();

            return new AuthNewThesisView()
            {
                AuthMaster = authMaster,
                ThesisForm = form,
                CanCreateThesis = this.CurrentCompany.IsAvailable
            };
        }

        #endregion

        #region Internship

        public async Task<AuthEditInternshipView> BuildEditInternshipViewAsync(int internshipID)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

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
                    MaxDurationTypeCodeName = m.MaxDurationType.CodeName,
                    Languages = m.Languages
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
            internship.AllLanguages = await FormGetLanguagesAsync();

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
                AuthMaster = authMaster,
                InternshipForm = internship
            };
        }

        public async Task<AuthNewInternshipView> BuildNewInternshipViewAsync()
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;

            }
            var form = new AuthAddEditInternshipForm()
            {
                InternshipCategories = await FormGetInternshipCategoriesAsync(),
                AmountTypes = await FormGetInternshipAmountTypesAsync(),
                DurationTypes = await FormGetInternshipDurationsAsync(),
                Countries = await FormGetCountriesAsync(),
                Currencies = await FormGetCurrenciesAsync(),
                IsActive = "on", // IsActive is enabled by default
                AllLanguages = await FormGetLanguagesAsync()
            };

            return new AuthNewInternshipView()
            {
                AuthMaster = authMaster,
                InternshipForm = form,
                CanCreateInternship = this.CurrentCompany.IsAvailable, // user can create internship only if he created company before
            };
        }

        public async Task<AuthEditInternshipView> BuildEditInternshipViewAsync(AuthAddEditInternshipForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            form.InternshipCategories = await FormGetInternshipCategoriesAsync();
            form.AmountTypes = await FormGetInternshipAmountTypesAsync();
            form.DurationTypes = await FormGetInternshipDurationsAsync();
            form.Countries = await FormGetCountriesAsync();
            form.Currencies = await FormGetCurrenciesAsync();
            form.AllLanguages = await FormGetLanguagesAsync();

            return new AuthEditInternshipView()
            {
                AuthMaster = authMaster,
                InternshipForm = form,
            };
        }

        public async Task<AuthNewInternshipView> BuildNewInternshipViewAsync(AuthAddEditInternshipForm form)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;

            }
            form.InternshipCategories = await FormGetInternshipCategoriesAsync();
            form.AmountTypes = await FormGetInternshipAmountTypesAsync();
            form.DurationTypes = await FormGetInternshipDurationsAsync();
            form.Countries = await FormGetCountriesAsync();
            form.Currencies = await FormGetCurrenciesAsync();
            form.AllLanguages = await FormGetLanguagesAsync();

            return new AuthNewInternshipView()
            {
                AuthMaster = authMaster,
                InternshipForm = form,
                CanCreateInternship = await GetCompanyIDOfCurrentUserAsync() != 0, // user can create internship only if he created company before
            };
        }

        #endregion

        #region Conversation messages

        public async Task<AuthConversationView> BuildConversationViewAsync(string otherUserId, int? page, AuthMessageForm messageForm = null)
        {

            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

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
                AuthMaster = authMaster,
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

        /// <summary>
        /// Creates thesis
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new thesis</returns>
        public async Task<int> CreateThesis(AuthAddEditThesisForm form)
        {
            try
            {
                // verify company
                if (!this.CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Pro přidání práce musíte prvně vytvořit firmu");
                }

                var thesis = new Entity.Thesis
                {
                    ApplicationUserId = this.CurrentUser.Id,
                    CompanyID = this.CurrentCompany.CompanyID,
                    Amount = form.Amount ?? 0,
                    CurrencyID = form.CurrencyID,
                    Description = form.Description,
                    InternshipCategoryID = form.InternshipCategoryID,
                    IsActive = form.GetIsActive(),
                    IsPaid = form.GetIsPaid(),
                    ThesisTypeID = form.ThesisTypeID,
                    ThesisName = form.ThesisName,
                };

                await Services.ThesisService.InsertAsync(thesis);

                return thesis.ID;
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
        /// Edits thesis
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditThesis(AuthAddEditThesisForm form)
        {
            try
            {
                // verify company
                if (!this.CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Pro přidání práce musíte prvně vytvořit firmu");
                }

                var thesis = new Entity.Thesis
                {
                    ID = form.ID,
                    ApplicationUserId = this.CurrentUser.Id,
                    CompanyID = this.CurrentCompany.CompanyID,
                    Amount = form.Amount ?? 0,
                    CurrencyID = form.CurrencyID,
                    Description = form.Description,
                    InternshipCategoryID = form.InternshipCategoryID,
                    IsActive = form.GetIsActive(),
                    IsPaid = form.GetIsPaid(),
                    ThesisTypeID = form.ThesisTypeID,
                    ThesisName = form.ThesisName,
                };

                await Services.ThesisService.UpdateAsync(thesis);

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

        public async Task<int> CreateMessage(AuthMessageForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new ValidationException("Pro odeslání zprávy se přihlašte");
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
        /// Edits profile
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditProfile(AuthEditProfileForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException($"Uživatel není přihlášen");
                }

                var applicationUser = await this.Services.IdentityService.GetAsync(this.CurrentUser.Id);

                if (applicationUser == null)
                {
                    throw new ValidationException($"Uživatel s ID { this.CurrentUser.Id} nebyl nalezen");
                }

                // set object properties
                applicationUser.FirstName = form.FirstName;
                applicationUser.LastName = form.LastName;

                await this.Services.IdentityService.UpdateAsync(applicationUser);
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
                        Services.FileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, Entity.Company.GetBannerFileName(company.CompanyGUID), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerHeight);
                    }

                    if (form.Logo != null)
                    {
                        Services.FileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, Entity.Company.GetLogoFileName(company.CompanyGUID), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
                    }

                    // commit transaction
                    transaction.Commit();

                    return company.ID;
                }
                catch (FileUploadException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log erros
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException($"{ex.Message}", ex);
                }
                catch (CodeNameNotUniqueException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException($"{form.CompanyName} je již v databázi", ex);
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
                    if (!this.CurrentCompany.IsAvailable)
                    {
                        throw new ValidationException($"Firma, kterou chcete editovat nebyla nalezena");
                    }

                    // verify company URL
                    if (!StringHelper.IsValidUrl(form.Web))
                    {
                        throw new ValidationException($"Zadejte validní URL webu");

                    }
                    // upload files if they are provided
                    if (form.Banner != null)
                    {
                        Services.FileProvider.SaveImage(form.Banner, FileConfig.BannerFolderPath, Entity.Company.GetBannerFileName(this.CurrentCompany.CompanyGUID), FileConfig.CompanyBannerWidth, FileConfig.CompanyBannerHeight);
                    }
                    if (form.Logo != null)
                    {
                        Services.FileProvider.SaveImage(form.Logo, FileConfig.LogoFolderPath, Entity.Company.GetLogoFileName(this.CurrentCompany.CompanyGUID), FileConfig.CompanyLogoWidth, FileConfig.CompanyLogoHeight);
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
                catch (FileUploadException ex)
                {
                    // rollback
                    transaction.Rollback();

                    // log erros
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UIException($"{ex.Message}", ex);
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
                    Requirements = form.Requirements,
                    Languages = form.Languages
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
                    Requirements = form.Requirements,
                    Languages = form.Languages
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

        public void UploadCompanyGalleryFiles(HttpRequestBase request)
        {
            try
            {
                if (!this.CurrentCompany.IsAvailable)
                {
                    throw new ValidationException("Firma není dostupná");
                }

                for (int i = 0; i < request.Files.Count; i++)
                {
                    HttpPostedFileBase file = request.Files[i];

                    var folderPath = Entity.Company.GetCompanyGalleryFolderPath(this.CurrentCompany.CompanyGUID);
                    var fileNameToSave = Entity.Company.GetCompanyGalleryFileName(Guid.NewGuid()); // generate new guid for new images

                    var fileSystemPath = SystemConfig.ServerRootPath + "\\" + FileConfig.CompanyalleryImagesPath + "\\" + this.CurrentCompany.CompanyGUID;
                    CreateCompanyDirectory(fileSystemPath);

                    // save file
                    Services.FileProvider.SaveImage(file, folderPath, fileNameToSave);
                }
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
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
            catch (FileUploadException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
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

        private void CreateCompanyDirectory(string path)
        {
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                return;
            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
        }

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

        private async Task<IEnumerable<AuthThesisTypeModel>> FormGetThesisTypesAsync()
        {
            var thesisTypes = await this.Services.ThesisTypeService.GetAllCachedAsync();

            return thesisTypes.Select(m => new AuthThesisTypeModel()
            {
                ID = m.ID,
                Name = m.Name
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

        private async Task<IEnumerable<AuthInternshipLanguageModel>> FormGetLanguagesAsync()
        {
            var languages = await this.Services.LanguageService.GetAllCachedAsync();

            return languages.Select(m => new AuthInternshipLanguageModel() {
                CodeName = m.CodeName,
                LanguageName = m.LanguageName
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
                    (m.RecipientApplicationUserId == this.CurrentUser.Id || m.SenderApplicationUserId == this.CurrentUser.Id)
                    &&
                    (m.RecipientApplicationUserId == otherUserId || m.SenderApplicationUserId == otherUserId))
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
        /// <param name="page">Page number</param>
        /// <param name="topN">Top N</param>
        /// <returns>Collection of messages of current user</returns>
        private async Task<IEnumerable<AuthConversationModel>> GetConversationsAsync(int topN)
        {
            return (await GetConversationsAsync(null)).Take(topN);
        }

        /// <summary>
        /// Gets messages of current user
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="topN">Top N</param>
        /// <returns>Collection of messages of current user</returns>
        private async Task<IPagedList<AuthConversationModel>> GetConversationsAsync(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // get both incoming and outgoming messages as well as messages targeted for given company
            var messagesQuery = this.Services.MessageService.GetAll()
                   .Where(m =>
                    (m.RecipientApplicationUserId == this.CurrentUser.Id || m.SenderApplicationUserId == this.CurrentUser.Id))
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
            var cacheSetupMessages = this.Services.CacheService.GetSetup<AuthMessageModel>(this.GetSource(), cacheMinutes);
            cacheSetupMessages.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Message>(),
                EntityKeys.KeyDeleteAny<Entity.Message>(),
                EntityKeys.KeyCreateAny<Entity.Message>(),
            };
            cacheSetupMessages.ObjectStringID = this.CurrentUser.Id;

            // get all messages for this user
            var allMessages = await this.Services.CacheService.GetOrSet(async () => await messagesQuery.ToListAsync(), cacheSetupMessages);

            // get conversations from messages
            var cacheSetupConversations = this.Services.CacheService.GetSetup<AuthConversationModel>(this.GetSource(), cacheMinutes);
            cacheSetupConversations.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Message>(),
                EntityKeys.KeyDeleteAny<Entity.Message>(),
                EntityKeys.KeyCreateAny<Entity.Message>(),
            };
            cacheSetupConversations.ObjectStringID = this.CurrentUser.Id;

            var conversations = this.Services.CacheService.GetOrSet(() => FilterConversationMessages(allMessages), cacheSetupConversations);

            return conversations.ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Creates list of conversations based on given messages
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Conversation list</returns>
        private IEnumerable<AuthConversationModel> FilterConversationMessages(IEnumerable<AuthMessageModel> messages)
        {
            var conversationList = new List<AuthConversationModel>();

            foreach (var message in messages)
            {
                var existingConversationMessage = conversationList.Where(m => m.RecipientApplicationUserId == this.CurrentUser.Id || m.SenderApllicationUserId == this.CurrentUser.Id)
                    .FirstOrDefault();

                if (existingConversationMessage == null)
                {
                    // add message to conversations
                    conversationList.Add(new AuthConversationModel()
                    {
                        CurrentUserId = this.CurrentUser.Id,
                        ID = message.ID,
                        RecipientFirstName = message.RecipientFirstName,
                        RecipientLastName = message.RecipientLastName,
                        SenderFirstName = message.SenderFirstName,
                        SenderLastName = message.SenderLastName,
                        IsRead = message.IsRead,
                        MessageCreated = message.MessageCreated,
                        MessageText = message.MessageText,
                        RecipientApplicationUserId = message.RecipientApplicationUserId,
                        RecipientApplicationUserName = message.RecipientApplicationUserName,
                        SenderApllicationUserId = message.SenderApllicationUserId,
                        SenderApplicationUserName = message.SenderApplicationUserName,
                        Subject = message.Subject
                    });
                }
            }

            return conversationList;
        }

        /// <summary>
        /// Gets internships of current user
        /// </summary>
        /// <returns>Collection of internships of current user</returns>
        private async Task<IEnumerable<AuthInternshipListingModel>> GetInternshipsAsync()
        {
            var internshipsQuery = this.Services.InternshipService.GetAll()
                     .Select(m => new AuthInternshipListingModel()
                     {
                         ID = m.ID,
                         Title = m.Title,
                         Created = m.Created,
                         IsActive = m.IsActive,
                         CodeName = m.CodeName,
                         CompanyID = m.CompanyID,
                         ApplicationUserId = m.ApplicationUserId
                     });

            if (this.CurrentCompany.IsAvailable)
            {
                internshipsQuery = internshipsQuery.Where(m => m.ApplicationUserId == this.CurrentUser.Id || m.CompanyID == this.CurrentCompany.CompanyID);
            }
            else
            {
                internshipsQuery = internshipsQuery.Where(m => m.ApplicationUserId == this.CurrentUser.Id);
            }

            internshipsQuery = internshipsQuery.OrderByDescending(m => m.Created);

            int cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthInternshipListingModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
            };
            cacheSetup.ObjectStringID = this.CurrentUser.Id + "_" + this.CurrentCompany.CompanyID;

            return await this.Services.CacheService.GetOrSet(async () => await internshipsQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets internships of current company/user
        /// </summary>
        /// <returns>Collection of internships</returns>
        private async Task<IEnumerable<AuthThesisListingModel>> GetThesesListingsAsync()
        {
            var thesisQuery = this.Services.ThesisService.GetAll()
                .Select(m => new AuthThesisListingModel()
                {
                    ID = m.ID,
                    IsActive = m.IsActive,
                    ThesisName = m.ThesisName,
                    Created = m.Created,
                    CodeName = m.CodeName,
                    ApplicationUserId = m.ApplicationUserId,
                    CompanyID = m.CompanyID
                });

            if (this.CurrentCompany.IsAvailable)
            {
                thesisQuery = thesisQuery.Where(m => m.ApplicationUserId == this.CurrentUser.Id || m.CompanyID == this.CurrentCompany.CompanyID);
            }
            else
            {
                thesisQuery = thesisQuery.Where(m => m.ApplicationUserId == this.CurrentUser.Id);
            }

            thesisQuery = thesisQuery.OrderByDescending(m => m.Created);

            int cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<AuthThesisListingModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
            };
            cacheSetup.ObjectStringID = this.CurrentUser.Id + "_" + this.CurrentCompany.CompanyID;

            return await this.Services.CacheService.GetOrSet(async () => await thesisQuery.ToListAsync(), cacheSetup);
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
