using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Core.Config;
using Core.Helpers;
using Core.Helpers.Internship;
using Entity;
using Entity.Base;
using PagedList;
using PagedList.EntityFramework;
using Service.Exceptions;
using UI.Base;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Models;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;
using UI.UIServices;

namespace UI.Builders.Auth
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
            if (!CurrentUser.IsAuthenticated)
            {
                return null;
            }

            // NOTE: Inneficient - call only when user is created in future
            PrepareUserDirectories(CurrentUser.Id);

            return new AuthMasterModel()
            {
                Internships = await GetInternshipsAsync(),
                Conversations = await GetConversationsAsync(10),
                Theses = await GetThesesListingsAsync(),
                
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

            var currentApplicationUser = await Services.IdentityService.GetSingle(CurrentUser.Id).FirstOrDefaultAsync();

            if (currentApplicationUser == null)
            {
                throw new UiException($"Uživatel s ID {CurrentUser.Id} nebyl nalezen");
            }

            var form = new AuthEditProfileForm()
            {
                FirstName = currentApplicationUser.FirstName,
                LastName = currentApplicationUser.LastName,
                Nickname = currentApplicationUser.Nickname
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

        #region Internships

        public async Task<AuthIndexView> BuildInternshipsViewAsync(int? page)
        {
            return await BuildIndexViewAsync(page);
        }

        #endregion

        #region Theses

        public async Task<AuthIndexView> BuildThesesVieAsync(int? page)
        {
            return await BuildIndexViewAsync(page);
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
                AuthMaster =  authMaster,
                AvatarForm = avatarForm
            };
        }

        #endregion

        #region Company 

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
            form.CompanyCategories = await FormGetCompanyCategories();

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
            form.CompanyCategories = await FormGetCompanyCategories();

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

            var currentUserId = CurrentUser.Id;

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
            var comapnyIsCreated = false;

            if (authMaster == null)
            {
                return null;
            }

            var currentUserId = CurrentUser.Id;

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
            company.CompanyCategories = await FormGetCompanyCategories();

            return new AuthRegisterCompanyView()
            {
                AuthMaster = authMaster,
                CompanyForm = company,
                CompanyIsCreated = comapnyIsCreated
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

            var thesisQuery = Services.ThesisService.GetAll()
                .Where(m => m.ID == thesisID)
                .Select(m => new AuthAddEditThesisForm()
                {
                    ID = m.ID,
                    Amount = m.Amount,
                    CurrencyID = m.CurrencyID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    Description = m.Description,
                    IsPaid = m.IsPaid ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
                    IsActive = m.IsActive ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
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
                IsActive = Helpers.InputHelper.ValueOfEnabledCheckboxStatic, // thesis is active by default
            };

            return new AuthNewThesisView()
            {
                AuthMaster = authMaster,
                ThesisForm = form,
                CanCreateThesis = CurrentCompany.IsAvailable
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
                CanCreateThesis = CurrentCompany.IsAvailable
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

            var internshipQuery = Services.InternshipService.GetSingle(internshipID)
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
                    IsPaid = m.IsPaid ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
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
                    IsActive = m.IsActive ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : null,
                    HasFlexibleHours = m.HasFlexibleHours ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : null,
                    WorkingHours = m.WorkingHours,
                    Requirements = m.Requirements,
                    MinDurationTypeCodeName = m.MinDurationType.CodeName,
                    MaxDurationTypeCodeName = m.MaxDurationType.CodeName,
                    Languages = m.Languages,
                    HomeOfficeOptionID = m.HomeOfficeOptionID,
                    StudentStatusOptionID = m.StudentStatusOptionID
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
            internship.StudentStatusOptions = await FormGetAllStudentStatusOptions();
            internship.HomeOfficeOptions = await FormGetAllHomeOfficeOptions();

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
                IsActive = Helpers.InputHelper.ValueOfEnabledCheckboxStatic, // IsActive is enabled by default
                AllLanguages = await FormGetLanguagesAsync(),
                StudentStatusOptions = await FormGetAllStudentStatusOptions(),
                HomeOfficeOptions = await FormGetAllHomeOfficeOptions()
            };

            return new AuthNewInternshipView()
            {
                AuthMaster = authMaster,
                InternshipForm = form,
                CanCreateInternship = CurrentCompany.IsAvailable, // user can create internship only if he created company before
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
            form.HomeOfficeOptions = await FormGetAllHomeOfficeOptions();
            form.StudentStatusOptions = await FormGetAllStudentStatusOptions();

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
            form.StudentStatusOptions = await FormGetAllStudentStatusOptions();
            form.HomeOfficeOptions = await FormGetAllHomeOfficeOptions();

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
            if (otherUserId.Equals(CurrentUser.Id, StringComparison.OrdinalIgnoreCase))
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

            // mark all received messaged for current user as read
            await Services.MessageService.MarkMessagesAsRead(CurrentUser.Id, otherUserId);

            return new AuthConversationView()
            {
                AuthMaster = authMaster,
                Messages = messages,
                ConversationUser = otherUser,
                Me = new AuthMessageUserModel()
                {
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName,
                    UserID = CurrentUser.Id,
                    UserName = CurrentUser.UserName,
                    Nickname = CurrentUser.Nickname
                },
                MessageForm = messageForm ?? defaultMessageForm
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
                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Pro přidání práce musíte prvně vytvořit firmu");
                }

                var thesis = new Entity.Thesis
                {
                    ApplicationUserId = CurrentUser.Id,
                    CompanyID = CurrentCompany.CompanyID,
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
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
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
                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Pro přidání práce musíte prvně vytvořit firmu");
                }

                var thesis = new Entity.Thesis
                {
                    ID = form.ID,
                    ApplicationUserId = CurrentUser.Id,
                    CompanyID = CurrentCompany.CompanyID,
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
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

        public async Task<int> CreateMessage(AuthMessageForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new ValidationException("Pro odeslání zprávy se přihlašte");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = CurrentUser.Id,
                    RecipientCompanyID = form.CompanyID,
                    RecipientApplicationUserId = form.RecipientApplicationUserId,
                    MessageText = form.Message,
                    Subject = null, // no subject needed
                    IsRead = false,
                };

                return await Services.MessageService.InsertAsync(message);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
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
                if (!CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException("Uživatel není přihlášen");
                }

                var applicationUser = await Services.IdentityService.GetAsync(CurrentUser.Id);

                if (applicationUser == null)
                {
                    throw new ValidationException($"Uživatel s ID { CurrentUser.Id} nebyl nalezen");
                }

                // set object properties
                applicationUser.FirstName = form.FirstName;
                applicationUser.LastName = form.LastName;
                applicationUser.Nickname = form.Nickname;

                await Services.IdentityService.UpdateAsync(applicationUser);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

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
                    if (! await IsValidCountry(form.CountryID))
                    {
                        throw new ValidationException("Vyplňte stát");
                    }

                    var company = new Entity.Company
                    {
                        ApplicationUserId = CurrentUser.Id,
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
                        ApplicationUserId = CurrentUser.Id,
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

                if (!CurrentUser.IsAuthenticated)
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
                    MaxDurationInDays = Services.InternshipDurationTypeService.GetDurationInDays(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInWeeks = Services.InternshipDurationTypeService.GetDurationInWeeks(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInMonths = Services.InternshipDurationTypeService.GetDurationInMonths(maxDurationTypeEnum, form.MaxDuration),
                    MinDurationInDays = Services.InternshipDurationTypeService.GetDurationInDays(minDurationTypeEnum, form.MinDuration),
                    MinDurationInWeeks = Services.InternshipDurationTypeService.GetDurationInWeeks(minDurationTypeEnum, form.MinDuration),
                    MinDurationInMonths = Services.InternshipDurationTypeService.GetDurationInMonths(minDurationTypeEnum, form.MinDuration),
                    MinDurationTypeID = form.MinDurationTypeID,
                    MaxDurationTypeID = form.MaxDurationTypeID,
                    IsActive = form.GetIsActive(),
                    ApplicationUserId = CurrentUser.Id,
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours,
                    Requirements = form.Requirements,
                    Languages = form.Languages,
                    HomeOfficeOptionID = form.HomeOfficeOptionID,
                    StudentStatusOptionID = form.StudentStatusOptionID,
                };

                await Services.InternshipService.InsertAsync(internship);

                return internship.ID;
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
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

                if (!CurrentUser.IsAuthenticated)
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
                    MaxDurationInDays = Services.InternshipDurationTypeService.GetDurationInDays(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInWeeks = Services.InternshipDurationTypeService.GetDurationInWeeks(maxDurationTypeEnum, form.MaxDuration),
                    MaxDurationInMonths = Services.InternshipDurationTypeService.GetDurationInMonths(maxDurationTypeEnum, form.MaxDuration),
                    MinDurationInDays = Services.InternshipDurationTypeService.GetDurationInDays(minDurationTypeEnum, form.MinDuration),
                    MinDurationInWeeks = Services.InternshipDurationTypeService.GetDurationInWeeks(minDurationTypeEnum, form.MinDuration),
                    MinDurationInMonths = Services.InternshipDurationTypeService.GetDurationInMonths(minDurationTypeEnum, form.MinDuration),
                    MinDurationTypeID = form.MinDurationTypeID,
                    MaxDurationTypeID = form.MaxDurationTypeID,
                    IsActive = form.GetIsActive(),
                    ApplicationUserId = CurrentUser.Id,
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours,
                    Requirements = form.Requirements,
                    Languages = form.Languages,
                    HomeOfficeOptionID = form.HomeOfficeOptionID,
                    StudentStatusOptionID = form.StudentStatusOptionID
                };

                await Services.InternshipService.UpdateAsync(internship);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

        public void UploadCompanyGalleryFiles(HttpRequestBase request)
        {
            try
            {
                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException("Firma není dostupná");
                }

                for (var i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];

                    var galleryPath = Entity.Company.GetCompanyGalleryFolderPath(CurrentCompany.CompanyGuid);
                    var fileNameToSave = Entity.Company.GetCompanyGalleryFileName(Guid.NewGuid()); // generate new guid for new images

                    // save file
                    Services.FileProvider.SaveImage(file, galleryPath, fileNameToSave);
                }
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
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
                    throw new ValidationException("Nelze nahrát prázdný soubor");
                }

                if (!CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException(UiExceptionEnum.NotAuthenticated.ToString());
                }

                Services.FileProvider.SaveImage(form.Avatar, ApplicationUser.GetAvatarFolderPath(CurrentUser.Id), ApplicationUser.GetAvatarFileName(), FileConfig.AvatarSideSize, FileConfig.AvatarSideSize);
            }
            catch (FileUploadException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Note: This method is inneficiently called each time an account page is accessed
        /// </summary>
        /// <param name="applicationUserId"></param>
        private void PrepareUserDirectories(string applicationUserId)
        {
            // create base folder
            var baseFolderPath = SystemConfig.ServerRootPath + "\\" + ApplicationUser.GetUserBaseFolderPath(applicationUserId);
            Directory.CreateDirectory(baseFolderPath);

            // create avatars folder
            var avatarsFolderPath = SystemConfig.ServerRootPath + "\\" + ApplicationUser.GetAvatarFolderPath(applicationUserId);
            Directory.CreateDirectory(avatarsFolderPath);
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

        private async Task<IEnumerable<AuthInternshipHomeOfficeOptionModel>> FormGetAllHomeOfficeOptions()
        {
            return (await Services.HomeOfficeOptionService.GetAllCachedAsync())
                .Select(m => new AuthInternshipHomeOfficeOptionModel()
                {
                    CodeName = m.CodeName,
                    HomeOfficeName = m.HomeOfficeName,
                    ID = m.ID
                });
        }

        private async Task<IEnumerable<AuthInternshipStudentStatusOptionModel>> FormGetAllStudentStatusOptions()
        {
            return (await Services.StudentStatusOptionService.GetAllCachedAsync())
                .Select(m => new AuthInternshipStudentStatusOptionModel()
                {
                    CodeName = m.CodeName,
                    StudentStatusName = m.StatusName,
                    ID = m.ID
                });
        }

        private async Task<AuthInternshipDurationType> GetDurationTypeAsync(int durationID)
        {
            var durationTypes = await FormGetInternshipDurationsAsync();
            return durationTypes
                .Where(m => m.ID == durationID)
                .SingleOrDefault();
        }

        private async Task<IEnumerable<AuthInternshipAmountType>> FormGetInternshipAmountTypesAsync()
        {
            var internshipAmountTypes = await Services.InternshipAmountTypeService.GetAllCachedAsync();

            return internshipAmountTypes.Select(m => new AuthInternshipAmountType()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                AmountTypeName = m.AmountTypeName
            });
        }

        private async Task<IEnumerable<AuthInternshipDurationType>> FormGetInternshipDurationsAsync()
        {
            var durationTypes = await Services.InternshipDurationTypeService.GetAllCachedAsync();

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
            var thesisTypes = await Services.ThesisTypeService.GetAllCachedAsync();

            return thesisTypes.Select(m => new AuthThesisTypeModel()
            {
                ID = m.ID,
                Name = m.Name
            });
        }

        private async Task<IEnumerable<AuthCurrencyModel>> FormGetCurrenciesAsync()
        {
            var currencies = await Services.CurrencyService.GetAllCachedAsync();

            return currencies.Select(m => new AuthCurrencyModel()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                CurrencyName = m.CurrencyName
            });
        }

        private async Task<IEnumerable<AuthInternshipLanguageModel>> FormGetLanguagesAsync()
        {
            var languages = await Services.LanguageService.GetAllCachedAsync();

            return languages.Select(m => new AuthInternshipLanguageModel()
            {
                CodeName = m.CodeName,
                LanguageName = m.LanguageName
            });
        }

        private async Task<bool> IsValidCountry(int countryID)
        {
            return (await FormGetCountriesAsync()).FirstOrDefault(m => m.ID == countryID) != null;
        }

        private async Task<IEnumerable<AuthCountryModel>> FormGetCountriesAsync()
        {
            var countries = await Services.CountryService.GetAllCachedAsync();

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
            var companySizes = await Services.CompanySizeService.GetAllCachedAsync();

            return companySizes.Select(m => new AuthCompanySize()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                CompanySizeName = m.CompanySizeName
            });
        }

        /// <summary>
        /// Gets conversations for current user
        /// </summary>
        /// <param name="otherUserId">ID of the other user (NEVER current user)</param>
        /// <param name="page">Page number</param>
        /// <returns>Collection of messages of current user with other user</returns>
        private async Task<IPagedList<AuthMessageModel>> GetConversationMessagesAsync(string otherUserId, int? page)
        {
            var pageSize = 10;
            var pageNumber = (page ?? 1);

            var messagesQuery = Services.MessageService.GetAll()
               .Where(m =>
                    (m.RecipientApplicationUserId == CurrentUser.Id || m.SenderApplicationUserId == CurrentUser.Id)
                    &&
                    (m.RecipientApplicationUserId == otherUserId || m.SenderApplicationUserId == otherUserId))
               .OrderByDescending(m => m.Created)
               .Select(m => new AuthMessageModel()
               {
                   ID = m.ID,
                   MessageCreated = m.Created,
                   SenderApllicationUserId = m.SenderApplicationUserId,
                   SenderApplicationUserName = m.SenderApplicationUser.UserName,
                   RecipientApplicationUserId = m.RecipientApplicationUserId,
                   RecipientApplicationUserName = m.RecipientApplicationUser.UserName,
                   CompanyID = m.RecipientCompanyID,
                   CompanyName = m.Company.CompanyName,
                   MessageText = m.MessageText,
                   CurrentUserId = CurrentUser.Id,
                   IsRead = m.IsRead,
                   SenderFirstName = m.SenderApplicationUser.FirstName,
                   SenderNickname = m.SenderApplicationUser.Nickname,
                   RecipientNickname = m.RecipientApplicationUser.Nickname,
                   SenderLastName = m.SenderApplicationUser.LastName,
                   RecipientFirstName = m.RecipientApplicationUser.FirstName,
                   RecipientLastName = m.RecipientApplicationUser.LastName,
                   Subject = m.Subject,
               });

            var cacheSetup = Services.CacheService.GetSetup<AuthMessageModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
                EntityKeys.KeyDeleteAny<Message>(),
                EntityKeys.KeyCreateAny<Message>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id + "_" + otherUserId;
            cacheSetup.PageNumber = pageNumber;
            cacheSetup.PageSize = pageSize;

            return await Services.CacheService.GetOrSet(async () => await messagesQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);
        }

        /// <summary>
        /// Gets name of given user
        /// </summary>
        /// <param name="applicationUserId">ID of user</param>
        /// <returns>Name of given user</returns>
        private async Task<AuthMessageUserModel> GetMessageUserAsync(string applicationUserId)
        {
            var cacheSetup = Services.CacheService.GetSetup<AuthMessageUserModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
            };
            cacheSetup.ObjectStringID = applicationUserId;

            var userQuery = Services.IdentityService.GetSingle(applicationUserId)
                .Select(m => new AuthMessageUserModel()
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    UserID = m.Id,
                    UserName = m.UserName,
                    Nickname = m.Nickname
                });

            return await Services.CacheService.GetOrSet(async () => await userQuery.FirstOrDefaultAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets messages of current user
        /// </summary>
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
        /// <returns>Collection of messages of current user</returns>
        private async Task<IPagedList<AuthConversationModel>> GetConversationsAsync(int? page)
        {
            var pageSize = 10;
            var pageNumber = (page ?? 1);

            // get both incoming and outgoming messages as well as messages targeted for given company
            var messagesQuery = Services.MessageService.GetAll()
                   .Where(m =>
                    (m.RecipientApplicationUserId == CurrentUser.Id || m.SenderApplicationUserId == CurrentUser.Id))
               .OrderByDescending(m => m.Created)
               .Select(m => new AuthMessageModel()
               {
                   ID = m.ID,
                   MessageCreated = m.Created,
                   SenderApllicationUserId = m.SenderApplicationUserId,
                   SenderApplicationUserName = m.SenderApplicationUser.UserName,
                   RecipientApplicationUserId = m.RecipientApplicationUserId,
                   RecipientApplicationUserName = m.RecipientApplicationUser.UserName,
                   CompanyID = m.RecipientCompanyID,
                   CompanyName = m.Company.CompanyName,
                   MessageText = m.MessageText,
                   CurrentUserId = CurrentUser.Id,
                   IsRead = m.IsRead,
                   SenderFirstName = m.SenderApplicationUser.FirstName,
                   SenderLastName = m.SenderApplicationUser.LastName,
                   SenderNickname = m.SenderApplicationUser.Nickname,
                   RecipientNickname = m.RecipientApplicationUser.Nickname,
                   RecipientFirstName = m.RecipientApplicationUser.FirstName,
                   RecipientLastName = m.RecipientApplicationUser.LastName,
                   Subject = m.Subject,
               });

            var cacheSetupMessages = Services.CacheService.GetSetup<AuthMessageModel>(GetSource());
            cacheSetupMessages.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
                EntityKeys.KeyDeleteAny<Message>(),
                EntityKeys.KeyCreateAny<Message>(),
            };
            cacheSetupMessages.ObjectStringID = CurrentUser.Id;

            // get all messages for this user
            var allMessages = await Services.CacheService.GetOrSet(async () => await messagesQuery.ToListAsync(), cacheSetupMessages);

            // get conversations from messages
            var cacheSetupConversations = Services.CacheService.GetSetup<AuthConversationModel>(GetSource());
            cacheSetupConversations.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
                EntityKeys.KeyDeleteAny<Message>(),
                EntityKeys.KeyCreateAny<Message>(),
            };
            cacheSetupConversations.ObjectStringID = CurrentUser.Id;

            var conversations = Services.CacheService.GetOrSet(() => FilterConversationMessages(allMessages), cacheSetupConversations);

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
                var existingConversationMessage = conversationList.Where(m => m.RecipientApplicationUserId == CurrentUser.Id || m.SenderApllicationUserId == CurrentUser.Id)
                    .FirstOrDefault();

                if (existingConversationMessage == null)
                {
                    // add message to conversations
                    conversationList.Add(new AuthConversationModel()
                    {
                        CurrentUserId = CurrentUser.Id,
                        ID = message.ID,
                        RecipientFirstName = message.RecipientFirstName,
                        RecipientLastName = message.RecipientLastName,
                        RecipientNickname = message.RecipientNickname,
                        SenderNickname = message.SenderNickname,
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
            var internshipsQuery = Services.InternshipService.GetAll()
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

            if (CurrentCompany.IsAvailable)
            {
                internshipsQuery = internshipsQuery.Where(m => m.ApplicationUserId == CurrentUser.Id || m.CompanyID == CurrentCompany.CompanyID);
            }
            else
            {
                internshipsQuery = internshipsQuery.Where(m => m.ApplicationUserId == CurrentUser.Id);
            }

            internshipsQuery = internshipsQuery.OrderByDescending(m => m.Created);

            var cacheSetup = Services.CacheService.GetSetup<AuthInternshipListingModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id + "_" + CurrentCompany.CompanyID;

            return await Services.CacheService.GetOrSet(async () => await internshipsQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets internships of current company/user
        /// </summary>
        /// <returns>Collection of internships</returns>
        private async Task<IEnumerable<AuthThesisListingModel>> GetThesesListingsAsync()
        {
            var thesisQuery = Services.ThesisService.GetAll()
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

            if (CurrentCompany.IsAvailable)
            {
                thesisQuery = thesisQuery.Where(m => m.ApplicationUserId == CurrentUser.Id || m.CompanyID == CurrentCompany.CompanyID);
            }
            else
            {
                thesisQuery = thesisQuery.Where(m => m.ApplicationUserId == CurrentUser.Id);
            }

            thesisQuery = thesisQuery.OrderByDescending(m => m.Created);

            var cacheSetup = Services.CacheService.GetSetup<AuthThesisListingModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id + "_" + CurrentCompany.CompanyID;

            return await Services.CacheService.GetOrSet(async () => await thesisQuery.ToListAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets company ID of current user
        /// </summary>
        /// <returns>CompanyID of current user or 0 if user is not logged or hasn't created any company</returns>
        private async Task<int> GetCompanyIDOfCurrentUserAsync()
        {
            if (!CurrentUser.IsAuthenticated)
            {
                return 0;
            }

            var companyQuery = Services.CompanyService.GetAll()
                .Where(m => m.ApplicationUserId == CurrentUser.Id)
                .Take(1)
                .Select(m => m.ID);

            var cacheSetup = Services.CacheService.GetSetup<int>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Company>(),
                EntityKeys.KeyDeleteAny<Entity.Company>(),
            };

            var company = await Services.CacheService.GetOrSet(async () => await companyQuery.FirstOrDefaultAsync(), cacheSetup);

            return company;
        }

        /// <summary>
        /// Gets company categories and caches the result
        /// </summary>
        /// <returns>Collection of company categories</returns>
        private async Task<IEnumerable<AuthCompanyCategoryModel>> FormGetCompanyCategories()
        {
            var cacheSetup = Services.CacheService.GetSetup<AuthCompanyCategoryModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<CompanyCategory>(),
                EntityKeys.KeyDeleteAny<CompanyCategory>(),
                EntityKeys.KeyUpdateAny<CompanyCategory>(),
            };

            var companyCategoriesQuery = Services.CompanyCategoryService.GetAll()
                .Select(m => new AuthCompanyCategoryModel()
                {
                    CompanyCategoryID = m.ID,
                    CompanyCategoryName = m.Name
                });

            var companyCategories = await Services.CacheService.GetOrSetAsync(async () => await companyCategoriesQuery.ToListAsync(), cacheSetup);

            return companyCategories;
        }

        /// <summary>
        /// Gets internship categories and caches the result
        /// </summary>
        /// <returns>Collection of internship categories</returns>
        private async Task<IEnumerable<AuthInternshipCategoryModel>> FormGetInternshipCategoriesAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<AuthInternshipCategoryModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<InternshipCategory>(),
                EntityKeys.KeyDeleteAny<InternshipCategory>(),
                EntityKeys.KeyUpdateAny<InternshipCategory>(),
            };

            var internshipCategoriesQuery = Services.InternshipCategoryService.GetAll()
                .Select(m => new AuthInternshipCategoryModel()
                {
                    InternshipCategoryID = m.ID,
                    InternshipCategoryName = m.Name
                });

            var internshipCategories = await Services.CacheService.GetOrSetAsync(async () => await internshipCategoriesQuery.ToListAsync(), cacheSetup);

            return internshipCategories;
        }



        #endregion
    }
}
