using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Helpers;
using Core.Helpers.Internship;
using Service.Exceptions;
using Service.Extensions;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Auth
{
    public class AuthInternshipBuilder : AuthMasterBuilder
    {
        #region Constructor

        public AuthInternshipBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Internship

        public async Task<AuthCompanyTypeIndexView> BuildInternshipsViewAsync()
        {
            return await BuildCompanyTypeIndexViewAsync();
        }

        public async Task<AuthEditInternshipView> BuildEditInternshipViewAsync(int internshipID)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var internshipQuery = Services.InternshipService.GetSingle(internshipID)
                .ForUser(CurrentUser.Id) 
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
                    HideAmount = m.HideAmount ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
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
                    MinEducationTypeID = m.MinEducationTypeID,
                    StudentStatusOptionID = m.StudentStatusOptionID,
                    QuestionnaireID = m.QuestionnaireID
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
            internship.Questionnaires = await FormGetQuestionnairesAsync();
            internship.Currencies = await FormGetCurrenciesAsync();
            internship.AllLanguages = await FormGetLanguagesAsync();
            internship.StudentStatusOptions = await FormGetAllStudentStatusOptionsAsync();
            internship.EducationTypes = await FormGetEducationTypesAsync();

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
                StudentStatusOptions = await FormGetAllStudentStatusOptionsAsync(),
                EducationTypes = await FormGetEducationTypesAsync(),
                Questionnaires = await FormGetQuestionnairesAsync(),
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
            form.EducationTypes = await FormGetEducationTypesAsync();
            form.StudentStatusOptions = await FormGetAllStudentStatusOptionsAsync();
            form.Questionnaires = await FormGetQuestionnairesAsync();

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
            form.StudentStatusOptions = await FormGetAllStudentStatusOptionsAsync();
            form.EducationTypes = await FormGetEducationTypesAsync();
            form.Questionnaires = await FormGetQuestionnairesAsync();

            return new AuthNewInternshipView()
            {
                AuthMaster = authMaster,
                InternshipForm = form,
                CanCreateInternship = CurrentCompany.IsAvailable
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Edits internship from given form
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditInternship(AuthAddEditInternshipForm form)
        {
            try
            {
                if (!CurrentCompany.IsAvailable)
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
                    IsPaid = form.GetIsPaid(),
                    CompanyID = CurrentCompany.CompanyID,
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
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours,
                    Requirements = form.Requirements,
                    Languages = form.Languages,
                    MinEducationTypeID = form.MinEducationTypeID,
                    StudentStatusOptionID = form.StudentStatusOptionID,
                    HideAmount = form.GetHideAmount(),
                    QuestionnaireID = form.QuestionnaireID,
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


        /// <summary>
        /// Creates new internship from given form
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new internship</returns>
        public async Task<int> CreateInternship(AuthAddEditInternshipForm form)
        {
            try
            {
                if (!CurrentCompany.IsAvailable)
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
                    CompanyID = CurrentCompany.CompanyID,
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
                    HasFlexibleHours = form.GetHasFlexibleHours(),
                    WorkingHours = form.WorkingHours,
                    Requirements = form.Requirements,
                    Languages = form.Languages,
                    MinEducationTypeID = form.MinEducationTypeID,
                    StudentStatusOptionID = form.StudentStatusOptionID,
                    HideAmount = form.GetHideAmount(),
                    QuestionnaireID = form.QuestionnaireID,
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


        #endregion
    }
}
