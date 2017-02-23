using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Service.Exceptions;
using Service.Extensions;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Auth
{
    public class AuthThesisBuilder : AuthMasterBuilder
    {
        #region Constructor

        public AuthThesisBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Actions

        public async Task<AuthCompanyTypeIndexView> BuildThesesVieAsync(int? page)
        {
            return await BuildCompanyTypeIndexViewAsync(page);
        }

        public async Task<AuthEditThesisView> BuildEditThesisViewAsync(int thesisID)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var thesisQuery = Services.ThesisService.GetAll()
                .ForUser(CurrentUser.Id)
                .Where(m => m.ID == thesisID)
                .Select(m => new AuthAddEditThesisForm()
                {
                    ID = m.ID,
                    Amount = m.Amount,
                    CurrencyID = m.CurrencyID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    Description = m.Description,
                    IsPaid = m.IsPaid ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
                    HideAmount = m.HideAmount ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
                    IsActive = m.IsActive ? Helpers.InputHelper.ValueOfEnabledCheckboxStatic : "",
                    ThesisName = m.ThesisName,
                    ThesisTypeID = m.ThesisTypeID,
                    QuestionnaireID = m.QuestionnaireID
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
            thesis.Questionnaires = await FormGetQuestionnairesAsync();

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
                Questionnaires = await FormGetQuestionnairesAsync(),
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
            form.Questionnaires = await FormGetQuestionnairesAsync();

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
            form.Questionnaires = await FormGetQuestionnairesAsync();

            return new AuthNewThesisView()
            {
                AuthMaster = authMaster,
                ThesisForm = form,
                CanCreateThesis = CurrentCompany.IsAvailable
            };
        }

        #endregion

        #region Methods


        /// <summary>
        /// Edits thesis
        /// </summary>
        /// <param name="form">form</param>
        public async Task EditThesis(AuthAddEditThesisForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new ValidationException("Pro vytvoření stáže se musíš přihlásit");
                }

                // verify company
                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Pro přidání práce musíš mít vytvořenou firmu");
                }

                var thesis = new Entity.Thesis
                {
                    ID = form.ID,
                    CompanyID = CurrentCompany.CompanyID,
                    Amount = form.Amount ?? 0,
                    CurrencyID = form.CurrencyID,
                    Description = form.Description,
                    InternshipCategoryID = form.InternshipCategoryID,
                    IsActive = form.GetIsActive(),
                    IsPaid = form.GetIsPaid(),
                    ThesisTypeID = form.ThesisTypeID,
                    ThesisName = form.ThesisName,
                    HideAmount = form.GetHideAmount(),
                    QuestionnaireID = form.QuestionnaireID
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

        /// <summary>
        /// Creates thesis
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new thesis</returns>
        public async Task<int> CreateThesis(AuthAddEditThesisForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send messages
                    throw new ValidationException("Pro vytvoření stáže se musíš přihlásit");
                }

                // verify company
                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Pro přidání práce musíš mít vytvořenou firmu");
                }

                var thesis = new Entity.Thesis
                {
                    CompanyID = CurrentCompany.CompanyID,
                    Amount = form.Amount ?? 0,
                    CurrencyID = form.CurrencyID,
                    Description = form.Description,
                    InternshipCategoryID = form.InternshipCategoryID,
                    IsActive = form.GetIsActive(),
                    IsPaid = form.GetIsPaid(),
                    ThesisTypeID = form.ThesisTypeID,
                    ThesisName = form.ThesisName,
                    HideAmount = form.GetHideAmount(),
                    QuestionnaireID = form.QuestionnaireID
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


        #endregion


    }
}
