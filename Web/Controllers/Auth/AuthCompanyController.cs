using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Helpers;
using Service.Context;
using UI.Builders.Auth;
using UI.Builders.Auth.Forms;
using UI.Builders.Master;
using UI.Builders.Questionnaire;
using UI.Enums;
using UI.Events;
using UI.Exceptions;
using UI.Modules.Questionnaire.Forms;

namespace Web.Controllers.Auth
{
    [Authorize]
    public class AuthCompanyController : AuthController
    {

        #region Builder

        private readonly QuestionnaireBuilder _questionnaireBuilder;

        #endregion

        #region Constructor

        public AuthCompanyController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, AuthBuilder authBuilder, QuestionnaireBuilder questionnaireBuilder) : base(appContext, serviceEvents, masterBuilder, authBuilder)
        {
            _questionnaireBuilder = questionnaireBuilder;
        }

        #endregion

        #region Actions

        [Route(CompanyActionPrefix + "/Dotaznik/{id:int}/{page:int?}")]
        public async Task<ActionResult> QuestionnaireSubmissions(int id, int? page)
        {
            var model = await AuthBuilder.AuthQuestionnaireBuilder.BuildQuestionnaireSubmissionsViewAsync(id, page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(QuestionnaireSubmissions)}.cshtml", model);
        }


        [Route(CompanyActionPrefix + "/Dotaznik/{id:int}/Formular/{submissionid:int}")]
        public async Task<ActionResult> ViewSubmission(int id, int submissionid)
        {
            var model = await AuthBuilder.AuthQuestionnaireBuilder.BuildQuestionnaireSubmissionViewAsync(id, submissionid);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(ViewSubmission)}.cshtml", model);
        }


        [Route(CompanyActionPrefix + "/NovyDotaznik")]
        public async Task<ActionResult> NewQuestionnaire()
        {
            var model = await AuthBuilder.AuthQuestionnaireBuilder.BuildQuestionnaireNewViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(NewQuestionnaire)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/UpravitDotaznik/{id:int}")]
        public async Task<ActionResult> EditQuestionnaire(int id, string result)
        {
            var model = await AuthBuilder.AuthQuestionnaireBuilder.BuildEditQuestionnaireViewAsync(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            var actionResult = EnumHelper.ParseEnum(result, ActionResultEnum.Unknown);

            if (actionResult == ActionResultEnum.Success)
            {
                model.QuestionnaireForm.FormResult.IsSuccess = true;
            }

            return View($"{CompanyViewsFolder}{nameof(EditQuestionnaire)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/ZaverecnePrace")]
        public async Task<ActionResult> Theses()
        {
            var model = await AuthBuilder.AuthThesisBuilder.BuildThesesVieAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(Theses)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/Staze")]
        public async Task<ActionResult> Internships()
        {
            var model = await AuthBuilder.AuthInternshipBuilder.BuildInternshipsViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(Internships)}.cshtml", model);
        }


        [Route(CompanyActionPrefix + "/Dotazniky")]
        public async Task<ActionResult> Questionnaires()
        {
            var model = await AuthBuilder.AuthQuestionnaireBuilder.BuildQuestionnairesViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(Questionnaires)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/RegistrovatFirmu")]
        public async Task<ActionResult> RegisterCompany()
        {
            var model = await AuthBuilder.AuthCompanyBuilder.BuildRegisterCompanyViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(RegisterCompany)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/UpravitFirmu")]
        public async Task<ActionResult> EditCompany(string result)
        {
            var model = await AuthBuilder.AuthCompanyBuilder.BuildEditCompanyViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            // get result
            var actionResult = EnumHelper.ParseEnum(result, ActionResultEnum.Unknown);
            if (actionResult == ActionResultEnum.Success)
            {
                // set flag indicating that company has just been created
                model.CompanyForm.IsNewlyRegisteredCompany = true;
            }

            return View($"{CompanyViewsFolder}{nameof(EditCompany)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/NovaStaz")]
        public async Task<ActionResult> NewInternship()
        {
            var model = await AuthBuilder.AuthInternshipBuilder.BuildNewInternshipViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(NewInternship)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/UpravitStaz/{id:int}")]
        public async Task<ActionResult> EditInternship(int? id, string result)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await AuthBuilder.AuthInternshipBuilder.BuildEditInternshipViewAsync((int)id);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return View($"{CompanyViewsFolder}{nameof(EditInternship)}.cshtml", model);
            }

            // get result
            var actionResult = EnumHelper.ParseEnum(result, ActionResultEnum.Unknown);
            if (actionResult == ActionResultEnum.Success)
            {
                // set flag indicating that internship has just been created
                model.InternshipForm.IsNewlyCreatedInternship = true;
            }

            return View($"{CompanyViewsFolder}{nameof(EditInternship)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/NovaZaverecnaPrace")]
        public async Task<ActionResult> NewThesis()
        {
            var model = await AuthBuilder.AuthThesisBuilder.BuildNewThesisViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(NewThesis)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/UpravitZaverecnouPraci/{id:int}")]
        public async Task<ActionResult> EditThesis(int? id, string result)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await AuthBuilder.AuthThesisBuilder.BuildEditThesisViewAsync((int)id);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            // get result
            var actionResult = EnumHelper.ParseEnum(result, ActionResultEnum.Unknown);
            if (actionResult == ActionResultEnum.Success)
            {
                // set flag indicating that thesis has just been created
                model.ThesisForm.IsNewlyCreatedThesis = true;
            }

            return View($"{CompanyViewsFolder}{nameof(EditThesis)}.cshtml", model);
        }

        [Route(CompanyActionPrefix + "/FiremniGalerie")]
        public async Task<ActionResult> CompanyGallery()
        {
            var model = await AuthBuilder.AuthCompanyBuilder.BuildCompanyGalleryViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}{nameof(CompanyGallery)}.cshtml", model);
        }

        #endregion

        #region POSTs

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/NovaStaz")]
        public async Task<ActionResult> NewInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(NewInternship)}.cshtml", await AuthBuilder.AuthInternshipBuilder.BuildNewInternshipViewAsync(form));
            }

            try
            {
                // create internship
                var internshipID = await AuthBuilder.AuthInternshipBuilder.CreateInternship(form);

                var model = await AuthBuilder.AuthInternshipBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                // update InternshipID
                model.InternshipForm.ID = internshipID;

                // redirect
                return RedirectToAction("EditInternship", new { id = internshipID, result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(NewInternship)}.cshtml", await AuthBuilder.AuthInternshipBuilder.BuildNewInternshipViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/UpravitStaz/{id:int}")]
        public async Task<ActionResult> EditInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(EditInternship)}.cshtml", await AuthBuilder.AuthInternshipBuilder.BuildEditInternshipViewAsync(form));
            }

            try
            {
                // edit internship
                await AuthBuilder.AuthInternshipBuilder.EditInternship(form);

                var model = await AuthBuilder.AuthInternshipBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                return View($"{CompanyViewsFolder}{nameof(EditInternship)}.cshtml", model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(EditInternship)}.cshtml", await AuthBuilder.AuthInternshipBuilder.BuildEditInternshipViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/RegistrovatFirmu")]
        public async Task<ActionResult> RegisterCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(RegisterCompany)}.cshtml", await AuthBuilder.AuthCompanyBuilder.BuildRegisterCompanyViewAsync(form));
            }

            try
            {
                // create company
                var companyID = await AuthBuilder.AuthCompanyBuilder.CreateCompany(form);

                var model = await AuthBuilder.AuthCompanyBuilder.BuildRegisterCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                // update CompanyID
                model.CompanyForm.ID = companyID;

                // redirect to edit company 
                // do not return view because the CurrentCompany would not get updated in this request
                return RedirectToAction("EditCompany", new { result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(RegisterCompany)}.cshtml", await AuthBuilder.AuthCompanyBuilder.BuildRegisterCompanyViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/UpravitFirmu")]
        public async Task<ActionResult> EditCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(EditCompany)}.cshtml", await AuthBuilder.AuthCompanyBuilder.BuildEditCompanyViewAsync(form));
            }

            try
            {
                // edit company
                await AuthBuilder.AuthCompanyBuilder.EditCompany(form);

                var model = await AuthBuilder.AuthCompanyBuilder.BuildEditCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                return View($"{CompanyViewsFolder}{nameof(EditCompany)}.cshtml", model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(EditCompany)}.cshtml", await AuthBuilder.AuthCompanyBuilder.BuildEditCompanyViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/NovaZaverecnaPrace")]
        public async Task<ActionResult> NewThesis(AuthAddEditThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(NewThesis)}.cshtml", await AuthBuilder.AuthThesisBuilder.BuildNewThesisViewAsync(form));
            }

            try
            {
                // create thesis
                var thesisID = await AuthBuilder.AuthThesisBuilder.CreateThesis(form);

                var model = await AuthBuilder.AuthThesisBuilder.BuildNewThesisViewAsync(form);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                // update thesis ID
                model.ThesisForm.ID = thesisID;

                // redirect
                return RedirectToAction("EditThesis", new { id = thesisID, result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(NewThesis)}.cshtml", await AuthBuilder.AuthThesisBuilder.BuildNewThesisViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/UpravitZaverecnouPraci/{id:int}")]
        public async Task<ActionResult> EditThesis(AuthAddEditThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(EditThesis)}.cshtml", await AuthBuilder.AuthThesisBuilder.BuildEditThesisViewAsync(form));
            }

            try
            {
                // edit thesis
                await AuthBuilder.AuthThesisBuilder.EditThesis(form);

                var model = await AuthBuilder.AuthThesisBuilder.BuildEditThesisViewAsync(form);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                return View($"{CompanyViewsFolder}{nameof(EditThesis)}.cshtml", model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(EditThesis)}.cshtml", await AuthBuilder.AuthThesisBuilder.BuildEditThesisViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/UpravitDotaznik/{id:int}")]
        public async Task<ActionResult> EditQuestionnaire(QuestionnaireCreateEditForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(EditQuestionnaire)}.cshtml", await AuthBuilder.AuthQuestionnaireBuilder.BuildEditQuestionnaireViewAsync(form));
            }

            try
            {
                // edit questionare
                await _questionnaireBuilder.EditQuestionnaireAsync(form.QuestionnaireID, form.QuestionnaireName, form.FieldGuids, Request);

                // redirect to edit questionare
                return RedirectToAction("EditQuestionnaire", new { id = form.QuestionnaireID, result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(EditQuestionnaire)}.cshtml", await AuthBuilder.AuthQuestionnaireBuilder.BuildEditQuestionnaireViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/NovyDotaznik")]
        public async Task<ActionResult> NewQuestionnaire(QuestionnaireCreateEditForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View($"{CompanyViewsFolder}{nameof(NewQuestionnaire)}.cshtml", await AuthBuilder.AuthQuestionnaireBuilder.BuildQuestionnaireNewViewAsync(form));
            }

            try
            {
                // create questionare
                var result = await _questionnaireBuilder.CreateQuestionnaireAsync(form.QuestionnaireName, form.FieldGuids, Request);

                // redirect to edit questionare
                return RedirectToAction("EditQuestionnaire", new { id = result.ObjectID, result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(NewQuestionnaire)}.cshtml", await AuthBuilder.AuthQuestionnaireBuilder.BuildQuestionnaireNewViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CompanyActionPrefix + "/FiremniGalerie")]
        public async Task<ActionResult> CompanyGallery(AuthCompanyGalleryUploadForm form)
        {
            var model = await AuthBuilder.AuthCompanyBuilder.BuildCompanyGalleryViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                // upload files
                AuthBuilder.AuthCompanyBuilder.UploadCompanyGalleryFiles(Request);

                return View($"{CompanyViewsFolder}{nameof(CompanyGallery)}.cshtml", model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View($"{CompanyViewsFolder}{nameof(CompanyGallery)}.cshtml", model);
            }
        }

        #endregion

    }
}