using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Entity;
using Entity.Base;
using Service.Exceptions;
using Service.Extensions;
using Service.Services.Activities.Enums;
using Service.Services.Questionnaires;
using Service.Services.Thesis.Enums;
using UI.Base;
using UI.Builders.Form.Forms;
using UI.Builders.Form.Models;
using UI.Builders.Form.Views;
using UI.Builders.Questionnaire;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Form
{
    public class FormBuilder : BaseBuilder
    {

        #region Builders

        private readonly QuestionnaireBuilder _questionnaireBuilder;

        #endregion

        #region Constructor

        public FormBuilder(ISystemContext systemContext, IServicesLoader servicesLoader, QuestionnaireBuilder questionnaireBuilder) : base(systemContext, servicesLoader)
        {
            _questionnaireBuilder = questionnaireBuilder;
        }

        #endregion

        #region Actions

        public async Task<FormInternshipView> BuildInternshipViewAsync(int internshipID, FormInternshipForm form = null, HttpRequestBase request = null)
        {
            var defaultForm = new FormInternshipForm()
            {
                Message = null,
                InternshipID = internshipID
            };

            var internship = await GetInternshipModelAsync(internshipID);

            if (internship == null)
            {
                return null;
            }

            // load questionnaire if available
            if (internship.QuestionnaireID != null)
            {
                if (form == null)
                {
                    var questions = await GetQuestionnaireQuestionsAsync(internship.QuestionnaireID ?? 0);
                    defaultForm.QuestionsJson = _questionnaireBuilder.GetStateJson(questions);
                }
                else
                {
                    // get questions
                    var questions = await GetQuestionnaireQuestionsAsync(internship.QuestionnaireID ?? 0);
                    var submittedQuestions = await _questionnaireBuilder.GetSubmittedQuestionsFromRequestAsync(internship.QuestionnaireID ?? 0, form.FieldGuids, request);

                    // assign submitted values to questions get JSON
                    var assignedQuestions = _questionnaireBuilder.AssignSubmittedQuestions(questions, submittedQuestions);

                    form.QuestionsJson = _questionnaireBuilder.GetStateJson(assignedQuestions);
                }
            }


            return new FormInternshipView()
            {
                Internship = internship,
                InternshipForm = form ?? defaultForm
            };

        }

        public async Task<FormThesisView> BuildThesisViewAsync(int thesisID, FormThesisForm form = null, HttpRequestBase request = null)
        {
            var defaultForm = new FormThesisForm()
            {
                ThesisID = thesisID,
                Message = null
            };

            var thesis = await GetThesisModelAsync(thesisID);

            if (thesis == null)
            {
                return null;
            }

            // load questionnaire if available
            if (thesis.QuestionnaireID != null)
            {
                if (form == null)
                {
                    var questions = await GetQuestionnaireQuestionsAsync(thesis.QuestionnaireID ?? 0);
                    defaultForm.QuestionsJson = _questionnaireBuilder.GetStateJson(questions);
                }
                else
                {
                    // get questions
                    var questions = await GetQuestionnaireQuestionsAsync(thesis.QuestionnaireID ?? 0);
                    var submittedQuestions = await _questionnaireBuilder.GetSubmittedQuestionsFromRequestAsync(thesis.QuestionnaireID ?? 0, form.FieldGuids, request);

                    // assign submitted values to questions get JSON
                    var assignedQuestions = _questionnaireBuilder.AssignSubmittedQuestions(questions, submittedQuestions);

                    form.QuestionsJson = _questionnaireBuilder.GetStateJson(assignedQuestions);
                }
            }

            return new FormThesisView()
            {
                Thesis = thesis,
                ThesisForm = form ?? defaultForm
            };

        }

        #endregion

        #region Public methods

        public async Task<int> SaveThesisForm(FormThesisForm form, HttpRequestBase request)
        {

            using (var transaction = AppContext.BeginTransaction())
            {
                try
                {
                    if (!CurrentUser.IsAuthenticated)
                    {
                        // only authenticated users can send message
                        throw new ValidationException("Pro odeslání zprávy se prosím přihlašte");
                    }

                    // get thesis
                    var thesis = await GetThesisModelAsync(form.ThesisID);

                    if (thesis == null)
                    {
                        throw new ValidationException($"Závěrečná práce s ID {form.ThesisID} nebyla nalezena");
                    }

                    // get recipient (company's representative)
                    var companyUserID = await GetIDOfCompanyUserAsync(thesis.CompanyID);

                    if (string.IsNullOrEmpty(companyUserID))
                    {
                        throw new ValidationException($"Záveřečná práce u firmy {thesis.CompanyName} nemá přiřazeného správce");
                    }

                    int? questionnaireSubmissionID = null;
                    if (thesis.QuestionnaireID != null)
                    {
                        // validate required questions
                        var questions = (await _questionnaireBuilder.GetQuestionnaireAsync((int) thesis.QuestionnaireID)).Questions?.ToList();
                        var submittedQuestions = await _questionnaireBuilder.GetSubmittedQuestionsFromRequestAsync((int)thesis.QuestionnaireID, form.FieldGuids, request);

                        CheckRequiredQuestions(questions, submittedQuestions);

                        var submissionResult = await _questionnaireBuilder.SubmitQuestionnaireFormAsync((int)thesis.QuestionnaireID, form.FieldGuids, request);
                        questionnaireSubmissionID = submissionResult.ObjectID;
                    }

                    var message = new Message()
                    {
                        SenderApplicationUserId = CurrentUser.Id,
                        RecipientApplicationUserId = companyUserID,
                        MessageText = form.Message,
                        Subject = thesis.ThesisName,
                        IsRead = false,
                        QuestionnaireSubmissionID = questionnaireSubmissionID
                    };

                    var messageResult = await Services.MessageService.InsertAsync(message);

                    // log activity if we got this far
                    var activityCurrentUserId = CurrentUser.IsAuthenticated ? CurrentUser.Id : null;

                    await Services.ActivityService.LogActivity(ActivityTypeEnum.FormSubmitThesis, thesis.CompanyID, activityCurrentUserId, thesis.ID);

                    transaction.Commit();

                    return messageResult.ObjectID;
                }
                catch (InvalidRecipientException ex)
                {
                    transaction.Rollback();

                    Services.LogService.LogException(ex);

                    throw new UiException("Nelze odeslat zprávu sám sobě", ex);
                }
                catch (ValidationException ex)
                {
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(UiExceptionEnum.SaveFailure, ex);
                }
            }
        }

        public async Task<int> SaveInternshipForm(FormInternshipForm form, HttpRequestBase request)
        {
            using (var transaction = AppContext.BeginTransaction())
            {
                try
                {
                    if (!CurrentUser.IsAuthenticated)
                    {
                        // only authenticated users can send message
                        throw new ValidationException("Pro odeslání zprávy se prosím přihlašte");
                    }

                    // get internship
                    var internship = await GetInternshipModelAsync(form.InternshipID);

                    if (internship == null)
                    {
                        throw new ValidationException($"Stáž s ID {form.InternshipID} nebyla nalezena");
                    }

                    // get recipient (company's representative)
                    var companyUserID = await GetIDOfCompanyUserAsync(internship.CompanyID);

                    if (string.IsNullOrEmpty(companyUserID))
                    {
                        throw new ValidationException($"Stáž u firmy {internship.CompanyName} nemá přiřazeného správce");
                    }

                    int? questionnaireSubmissionID = null;
                    if (internship.QuestionnaireID != null)
                    {
                        // validate required questions
                        var questions = (await _questionnaireBuilder.GetQuestionnaireAsync((int)internship.QuestionnaireID)).Questions?.ToList();
                        var submittedQuestions = await _questionnaireBuilder.GetSubmittedQuestionsFromRequestAsync((int)internship.QuestionnaireID, form.FieldGuids, request);

                        CheckRequiredQuestions(questions, submittedQuestions);

                        var submissionResult = await _questionnaireBuilder.SubmitQuestionnaireFormAsync((int) internship.QuestionnaireID, form.FieldGuids, request);
                        questionnaireSubmissionID = submissionResult.ObjectID;
                    }

                    var message = new Message()
                    {
                        SenderApplicationUserId = CurrentUser.Id,
                        RecipientApplicationUserId = companyUserID,
                        MessageText = form.Message,
                        Subject = internship.InternshipTitle,
                        IsRead = false,
                        QuestionnaireSubmissionID = questionnaireSubmissionID
                    };

                    var messageResult = await Services.MessageService.InsertAsync(message);

                    // log activity if we got this far
                    var activityCurrentUserId = CurrentUser.IsAuthenticated ? CurrentUser.Id : null;
                    await Services.ActivityService.LogActivity(ActivityTypeEnum.FormSubmitInternship, internship.CompanyID, activityCurrentUserId, internship.InternshipID);

                    transaction.Commit();

                    // return message id
                    return messageResult.ObjectID;
                }
                catch (InvalidRecipientException ex)
                {
                    transaction.Rollback();

                    Services.LogService.LogException(ex);

                    throw new UiException("Nelze odeslat zprávu sám sobě", ex);
                }
                catch (ValidationException ex)
                {
                    transaction.Rollback();

                    // log error
                    Services.LogService.LogException(ex);

                    // re-throw
                    throw new UiException(ex.Message, ex);
                }
                catch (Exception ex)
                {
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

        /// <summary>
        /// Throws exception if any of the questions are required, but were not filled
        /// </summary>
        /// <param name="questions"></param>
        /// <param name="submittedQuestions"></param>
        private void CheckRequiredQuestions(List<IQuestion> questions, IList<IQuestionSubmit> submittedQuestions)
        {
            foreach (var question in questions)
            {
                var submittedQuestion = submittedQuestions.Where(m => m.Guid.Equals(question.Guid, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (submittedQuestion == null)
                {
                    throw new ValidationException("Neznámá otázka");
                }

                if (question.QuestionRequired && string.IsNullOrEmpty(submittedQuestion.Answer))
                {
                    throw new ValidationException("Vyplň prosím všechny povinné otázky");
                }
            }
        }

        /// <summary>
        /// Gets thesis model from DB or Cache
        /// </summary>
        /// <param name="thesisID">ThesisID</param>
        /// <returns>Internship model</returns>
        private async Task<FormThesisModel> GetThesisModelAsync(int thesisID)
        {
            var cacheSetup = Services.CacheService.GetSetup<FormThesisModel>(GetSource());
            cacheSetup.ObjectID = thesisID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Thesis>(thesisID),
                EntityKeys.KeyDelete<Entity.Thesis>(thesisID),
            };

            var thesisQuery = Services.ThesisService.GetSingle(thesisID)
                .OnlyActive()
                .Select(m => new FormThesisModel()
                {
                    QuestionnaireID = m.QuestionnaireID,
                    CompanyID = m.Company.ID,
                    ThesisCodeName = m.ThesisType.CodeName,
                    CompanyName = m.Company.CompanyName,
                    CompanyCodeName = m.Company.CodeName,
                    ID = m.ID,
                    CompanyGuid = m.Company.Guid,
                    ThesisName = m.ThesisName,
                    ThesisTypeName = m.ThesisType.Name
                });

            var thesis = await Services.CacheService.GetOrSetAsync(async () => await thesisQuery.FirstOrDefaultAsync(), cacheSetup);

            if (thesis == null)
            {
                return null;
            }

            // thesis name
            thesis.ThesisTypeNameConverted = thesis.ThesisCodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", (await GetAllThesisTypesAsync())) : thesis.ThesisTypeName;

            return thesis;
        }

        /// <summary>
        /// Gets all thesis types except the "all" type
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllThesisTypesAsync()
        {
            return (await Services.ThesisTypeService.GetAllCachedAsync())
                .Where(m => !m.CodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase))
                .Select(m => m.Name)
                .ToList();
        }

        /// <summary>
        /// Gets internship model from DB or Cache
        /// </summary>
        /// <param name="internshipID">Internship ID</param>
        /// <returns>Internship model</returns>
        private async Task<FormInternshipModel> GetInternshipModelAsync(int internshipID)
        {
            var cacheSetup = Services.CacheService.GetSetup<FormInternshipModel>(GetSource());
            cacheSetup.ObjectID = internshipID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Internship>(internshipID),
                EntityKeys.KeyDelete<Entity.Internship>(internshipID),
            };

            var internshipQuery = Services.InternshipService.GetSingle(internshipID)
                .OnlyActive()
                .Select(m => new FormInternshipModel()
                {
                    CompanyID = m.CompanyID,
                    CompanyGuid = m.Company.Guid,
                    CompanyName = m.Company.CompanyName,
                    InternshipID = m.ID,
                    InternshipTitle = m.Title,
                    CompanyCodeName = m.Company.CodeName,
                    QuestionnaireID = m.QuestionnaireID,
                    InternshipCodeName = m.CodeName
                });

            return await Services.CacheService.GetOrSetAsync(async () => await internshipQuery.FirstOrDefaultAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets questions for given questionnaire
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        private async Task<IList<IQuestion>> GetQuestionnaireQuestionsAsync(int questionnaireID)
        {
            var cacheSetup = Services.CacheService.GetSetup<string>(GetSource());
            cacheSetup.ObjectID = questionnaireID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Questionnaire>(questionnaireID),
                EntityKeys.KeyDelete<Entity.Questionnaire>(questionnaireID),
            };

            var questionnaireQuery = Services.QuestionnaireService.GetSingle(questionnaireID)
                .Select(m => m.QuestionnaireXml);

            var questionnaireXml = await Services.CacheService.GetOrSetAsync(async () => await questionnaireQuery.FirstOrDefaultAsync(), cacheSetup);

            if (questionnaireXml == null)
            {
                // questionnaire was not found
                return null;
            }

            var questions = Services.QuestionnaireService.GetQuestionsFromXml(questionnaireXml);

            // get json
            return questions.ToList();
        }

        /// <summary>
        /// Gets ID of user who created company
        /// </summary>
        /// <param name="companyID">CompanyID</param>
        /// <returns>ID of user who created company</returns>
        private async Task<string> GetIDOfCompanyUserAsync(int companyID)
        {
            return await Services.CompanyService.GetSingle(companyID)
                .Select(m => m.CreatedByApplicationUserId)
                .FirstOrDefaultAsync();
        }

        #endregion

    }
}
