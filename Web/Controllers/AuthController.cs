using System;
using System.Collections.Generic;
using System.Linq;
using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Helpers;
using Service.Services.Questionnaires;
using UI.Base;
using UI.Builders.Auth;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Master;
using UI.Builders.Questionnaire;
using UI.Enums;
using UI.Events;
using UI.Exceptions;
using UI.Modules.Questionnaire.Forms;

namespace Web.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {

        #region Builder

        private readonly AuthBuilder _authBuilder;
        private readonly QuestionnaireBuilder _questionnaireBuilder;

        #endregion

        #region Constructor

        public AuthController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, AuthBuilder authBuilder, QuestionnaireBuilder questionnaireBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _authBuilder = authBuilder;
            _questionnaireBuilder = questionnaireBuilder;
        }

        #endregion

        #region Actions


        [Route("Uzivatel/Dotaznik/{id:int}/{page:int?}")]
        public async Task<ActionResult> QuestionnaireSubmissions(int id, int? page)
        {
            var model = await _authBuilder.BuildQuestionnaireSubmissionsViewAsync(id, page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }


        [Route("Uzivatel/Dotaznik/{id:int}/Formular/{submissionid:int}")]
        public async Task<ActionResult> ViewSubmission(int id, int submissionid)
        {
            var model = await _authBuilder.BuildQuestionnaireSubmissionViewAsync(id, submissionid);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }


        [Route("Uzivatel/NovyDotaznik")]
        public async Task<ActionResult> NewQuestionnaire()
        {
            var model = await _authBuilder.BuildQuestionnaireNewViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/UpravitDotaznik/{id:int}")]
        public async Task<ActionResult> EditQuestionnaire(int id, string result)
        {
            var model = await _authBuilder.BuildEditQuestionnaireViewAsync(id);

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

            return View(model);
        }

        [Route("Uzivatel")]
        public async Task<ActionResult> Index(int? page)
        {
            var model = await _authBuilder.BuildIndexViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/ZaverecnePrace/{page:int?}")]
        public async Task<ActionResult> Theses(int? page)
        {
            var model = await _authBuilder.BuildThesesVieAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/Staze/{page:int?}")]
        public async Task<ActionResult> Internships(int? page)
        {
            var model = await _authBuilder.BuildInternshipsViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }


        [Route("Uzivatel/Dotazniky/{page:int?}")]
        public async Task<ActionResult> Questionnaires(int? page)
        {
            var model = await _authBuilder.BuildQuestionnairesViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/RegistrovatFirmu")]
        public async Task<ActionResult> RegisterCompany()
        {
            var model = await _authBuilder.BuildRegisterCompanyViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/UpravitFirmu")]
        public async Task<ActionResult> EditCompany(string result)
        {
            var model = await _authBuilder.BuildEditCompanyViewAsync();

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

            return View(model);
        }

        [Route("Uzivatel/NovaStaz")]
        public async Task<ActionResult> NewInternship()
        {
            var model = await _authBuilder.BuildNewInternshipViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/UpravitStaz/{id:int}")]
        public async Task<ActionResult> EditInternship(int? id, string result)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _authBuilder.BuildEditInternshipViewAsync((int) id);

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
                // set flag indicating that internship has just been created
                model.InternshipForm.IsNewlyCreatedInternship = true;
            }

            return View(model);
        }

        [Route("Uzivatel/NovaZaverecnaPrace")]
        public async Task<ActionResult> NewThesis()
        {
            var model = await _authBuilder.BuildNewThesisViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/UpravitZaverecnouPraci/{id:int}")]
        public async Task<ActionResult> EditThesis(int? id, string result)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _authBuilder.BuildEditThesisViewAsync((int) id);

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

            return View(model);
        }

        [Route("Uzivatel/UpravitProfil")]
        public async Task<ActionResult> EditProfile()
        {
            var model = await _authBuilder.BuildEditProfileViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/Avatar")]
        public async Task<ActionResult> Avatar()
        {
            var model = await _authBuilder.BuildAvatarViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/FiremniGalerie")]
        public async Task<ActionResult> CompanyGallery()
        {
            var model = await _authBuilder.BuildCompanyGalleryViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route("Uzivatel/Konverzace/{id}/{page:int?}")]
        public async Task<ActionResult> Conversation(string id, int? page)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var model = await _authBuilder.BuildConversationViewAsync(id, page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        #endregion

        #region POST methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/Konverzace/{id}/{page:int?}")]
        public async Task<ActionResult> Conversation(AuthMessageForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form));
            }

            try
            {
                await _authBuilder.CreateMessage(form);

                var model = await _authBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form);

                // set form status
                model.MessageForm.FormResult.IsSuccess = true;

                // clear the form
                model.MessageForm.Message = string.Empty;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/Avatar")]
        public async Task<ActionResult> Avatar(AuthAvatarUploadForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildAvatarViewAsync());
            }

            try
            {
                _authBuilder.UploadAvatar(form);

                var model = await _authBuilder.BuildAvatarViewAsync();

                model.AvatarForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildAvatarViewAsync());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/FiremniGalerie")]
        public async Task<ActionResult> CompanyGallery(AuthCompanyGalleryUploadForm form)
        {
            var model = await _authBuilder.BuildCompanyGalleryViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                // upload files
                _authBuilder.UploadCompanyGalleryFiles(Request);

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/UpravitProfil")]
        public async Task<ActionResult> EditProfile(AuthEditProfileForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditProfileViewAsync(form));
            }

            try
            {
                // edit profile
                await _authBuilder.EditProfile(form);

                var model = await _authBuilder.BuildEditProfileViewAsync(form);

                // set form status
                model.ProfileForm.FormResult.IsSuccess = true;

                // set flag indicating whether user's e-mail is still visible to others or not
                model.AuthMaster.EmailVisibleForPeople = string.IsNullOrEmpty(model.ProfileForm.FirstName) && string.IsNullOrEmpty(model.ProfileForm.Nickname);

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditProfileViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/NovaStaz")]
        public async Task<ActionResult> NewInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildNewInternshipViewAsync(form));
            }

            try
            {
                // create internship
                var internshipID = await _authBuilder.CreateInternship(form);

                var model = await _authBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                // update InternshipID
                model.InternshipForm.ID = internshipID;

                // redirect
                return RedirectToAction("EditInternship", new {id = internshipID, result = ActionResultEnum.Success});
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildNewInternshipViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/UpravitStaz/{id:int}")]
        public async Task<ActionResult> EditInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditInternshipViewAsync(form));
            }

            try
            {
                // edit internship
                await _authBuilder.EditInternship(form);

                var model = await _authBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditInternshipViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/RegistrovatFirmu")]
        public async Task<ActionResult> RegisterCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildRegisterCompanyViewAsync(form));
            }

            try
            {
                // create company
                var companyID = await _authBuilder.CreateCompany(form);

                var model = await _authBuilder.BuildRegisterCompanyViewAsync(form);

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

                return View(await _authBuilder.BuildRegisterCompanyViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/UpravitFirmu")]
        public async Task<ActionResult> EditCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditCompanyViewAsync(form));
            }

            try
            {
                // edit company
                await _authBuilder.EditCompany(form);

                var model = await _authBuilder.BuildEditCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditCompanyViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/NovaZaverecnaPrace")]
        public async Task<ActionResult> NewThesis(AuthAddEditThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildNewThesisViewAsync(form));
            }

            try
            {
                // create thesis
                var thesisID = await _authBuilder.CreateThesis(form);

                var model = await _authBuilder.BuildNewThesisViewAsync(form);

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

                return View(await _authBuilder.BuildNewThesisViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/UpravitZaverecnouPraci/{id:int}")]
        public async Task<ActionResult> EditThesis(AuthAddEditThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditThesisViewAsync(form));
            }

            try
            {
                // edit thesis
                await _authBuilder.EditThesis(form);

                var model = await _authBuilder.BuildEditThesisViewAsync(form);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditThesisViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/UpravitDotaznik/{id:int}")]
        public async Task<ActionResult> EditQuestionnaire(QuestionnaireCreateEditForm form)
        {
            try
            {
                // get questions from request
                var questions = GetQuestionnaireQuestionsFromRequest(form.FieldGuids);

                // edit questionare
                await _questionnaireBuilder.EditQuestionnaireAsync(form.QuestionnaireID, form.QuestionnaireName, questions);

                // redirect to edit questionare
                return RedirectToAction("EditQuestionnaire", new { id = form.QuestionnaireID, result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditQuestionnaireViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Uzivatel/NovyDotaznik")]
        public async Task<ActionResult> NewQuestionnaire(QuestionnaireCreateEditForm form)
        {
            try
            {
                // get questions from request
                var questions = GetQuestionnaireQuestionsFromRequest(form.FieldGuids);

                // create questionare
                var result = await _questionnaireBuilder.CreateQuestionnaireAsync(form.QuestionnaireName, questions);

                // redirect to edit questionare
                return RedirectToAction("EditQuestionnaire", new { id = result.ObjectID, result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildQuestionnaireNewViewAsync(form));
            }
        }

        #endregion

        #region User type selection GET methods

        [HttpGet]
        [Route("Uzivatel/Typ/Uchazec")]
        public async Task<ActionResult> SetCandidateType()
        {
            await _authBuilder.SetUserTypeAsync(false, true);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Uzivatel/Typ/Firma")]
        public async Task<ActionResult> SetCompanyType()
        {
            await _authBuilder.SetUserTypeAsync(true, false);

            return RedirectToAction("Index");
        }

        #endregion

        #region Helper methods

        private ActionResult ShowUserTypeSelection(AuthMasterView view)
        {
            return View("~/views/auth/UserTypeSelection.cshtml", view);
        }

        private IList<IQuestion> GetQuestionnaireQuestionsFromRequest(IList<string> fieldGuids)
        {
            // use specific form field names generated by JS
            var questionTypePrefix = "QuestionType_";
            var questionTextPrefix = "QuestionText_";
            var questionCorrectAnswerPrefix = "QuestionCorrectAnswer_";
            var fieldDataPrefix = "Data_";
            var fieldDataSeparator = '_';

            var questions = new List<IQuestion>();

            if (fieldGuids == null)
            {
                return new List<IQuestion>();
            }

            foreach (var fieldGuid in fieldGuids)
            {
                // get question specific data from submitted form dadta
                var questionType = Request.Form[questionTypePrefix + fieldGuid];
                var questionText = Request.Form[questionTextPrefix + fieldGuid];
                var questionCorrectAnswer = Request.Form[questionCorrectAnswerPrefix + fieldGuid];

                var dataPrefix = fieldDataPrefix + fieldGuid;

                var questionData = new List<IQuestionData>();

                // get questions data
                foreach (var key in Request.Form.AllKeys.Where(m => m.StartsWith(dataPrefix, StringComparison.OrdinalIgnoreCase)))
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    var keyValues = key.Split(fieldDataSeparator);

                    if (keyValues.Length != 3)
                    {
                        // invalid number of params
                        continue;
                    }

                    var dataName = keyValues[2];

                    questionData.Add(new QuestionData()
                    {
                        Name = dataName,
                        Value = Request.Form[key]
                    });
                }

                // add question
                questions.Add(new Question()
                {
                    Data = questionData,
                    QuestionText = questionText,
                    QuestionType = questionType,
                    Guid = fieldGuid,
                    CorrectAnswer = questionCorrectAnswer
                });
            }

            return questions;
        }

        #endregion

    }
}