﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Entity.Base;
using Newtonsoft.Json;
using Service.Exceptions;
using Service.Extensions;
using Service.Services;
using Service.Services.Questionnaires;
using UI.Base;
using UI.Builders.Questionnaire.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;
using UI.Modules.Questionnaire.Forms;

namespace UI.Builders.Questionnaire
{
    public class QuestionnaireBuilder : BaseBuilder
    {

        #region Constructor

        public QuestionnaireBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions


        #endregion

        #region Methods

        public async Task DeleteQuestionnaireAsync(int questionnaireID)
        {
            try
            {
                await Services.QuestionnaireService.DeleteAsync(questionnaireID);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.DeleteFailure, ex);
            }
        }

        public string GetInitialStateJson(IList<IQuestion> questions)
        {
            return questions == null ? null : JsonConvert.SerializeObject(questions);
        }

        public QuestionnaireCreateEditForm GetForm(QuestionnaireCreateEditForm form = null)
        {
            if (form?.Questions != null)
            {
                // calculate initial state by converting questions to JSON object
                form.InitialStateJson = GetInitialStateJson(form.Questions);
            }
            return form ?? new QuestionnaireCreateEditForm();
        }

        public async Task<QuestionnaireCreateEditForm> GetQuestionnaireEditFormAsync(int questionnareID)
        {
            var quesionareQuery = Services.QuestionnaireService.GetSingle(questionnareID)
                .ForUser(CurrentUser.Id)
                .Select(m => new QuestionnaireCreateEditForm()
                {
                    QuestionnaireName = m.QuestionnaireName,
                    CreatedByApplicationUserID = m.CreatedByApplicationUserId,
                    CompanyID = m.CompanyID,
                    QuestionnaireID = m.ID,
                    QuesitonnaireXml = m.QuestionnaireXml,
                });

            var questionnaire = await quesionareQuery.FirstOrDefaultAsync();

            if (questionnaire == null)
            {
                return null;
            }

            // check if current user has access to questionnaire
            if (!questionnaire.CreatedByApplicationUserID.Equals(CurrentUser.Id) || questionnaire.CompanyID != CurrentCompany.CompanyID)
            {
                return null;
            }

            // get questions from XML
            questionnaire.Questions = Services.QuestionnaireService.GetQuestionsFromXml(questionnaire.QuesitonnaireXml).ToList();

            // get initial json
            questionnaire.InitialStateJson = GetInitialStateJson(questionnaire.Questions);

            return questionnaire;
        }

        public async Task<QuestionnaireModel> GetQuestionnaireAsync(int questionnaireID)
        {
            // get companies from db/cache
            var cacheSetup = Services.CacheService.GetSetup<QuestionnaireModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<QuestionnaireModel>(questionnaireID)
            };
            cacheSetup.ObjectID = questionnaireID;

            var questionnaireQuery = Services.QuestionnaireService.GetSingle(questionnaireID)
                .Select(m => new QuestionnaireModel()
                {
                    ID = m.ID,
                    CodeName = m.CodeName,
                    CompanyID = m.CompanyID,
                    QuestionnaireName = m.QuestionnaireName,
                    CreatedByApplicationUserId = m.CreatedByApplicationUserId,
                    QuestionnaireXml = m.QuestionnaireXml,
                });

            var questionnaire = await Services.CacheService.GetOrSetAsync(async () => await questionnaireQuery.FirstOrDefaultAsync(), cacheSetup);

            // parse questions
            if (questionnaire == null)
            {
                return null;
            }

            questionnaire.Questions = Services.QuestionnaireService.GetQuestionsFromXml(questionnaire.QuestionnaireXml).ToList();

            return questionnaire;
        } 

        public async Task<IInsertActionResult> CreateQuestionnaireAsync(string questionnaireName, IList<IQuestion> questions)
        {
            try
            {
                if (string.IsNullOrEmpty(questionnaireName))
                {
                    questionnaireName = "Bezejmenný dotazník";
                }

                if (questions == null)
                {
                    throw new ValidationException("Dotazník nemohl být uložen z důvodu neplatných otázek");
                }

                if (!questions.Any())
                {
                    throw new ValidationException("Dotazník musí mít alespoň 1 otázku");
                }

                if (!CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException("Pro vytvoření dotazníku se prosím přihlaste");
                }

                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException("Pro vytvoření dotazníku musíte mít zaregistrovanou firmu");
                }

                return await Services.QuestionnaireService.CreateQuestionnaireAsync(questionnaireName, questions, CurrentUser.Id, CurrentCompany.CompanyID);
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

        public async Task<int> EditQuestionnaireAsync(int questionnaireID, string questionnaireName, IList<IQuestion> questions)
        {
            try
            {
                if (string.IsNullOrEmpty(questionnaireName))
                {
                    questionnaireName = "Bezejmenný dotazník";
                }

                if (questions == null)
                {
                    throw new ValidationException("Dotazník nemohl být uložen z důvodu neplatných otázek");
                }

                if (!questions.Any())
                {
                    throw new ValidationException("Dotazník musí mít alespoň 1 otázku");
                }

                if (!CurrentUser.IsAuthenticated)
                {
                    throw new ValidationException("Pro vytvoření dotazníku se prosím přihlaste");
                }

                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException("Pro vytvoření dotazníku musíte mít zaregistrovanou firmu");
                }

                return await Services.QuestionnaireService.EditQuestionnaireAsync(questionnaireID, questionnaireName, questions, CurrentUser.Id, CurrentCompany.CompanyID);
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

        public async Task<IInsertActionResult> SubmitQuestionnaireFormAsync(int questionnaireID, IList<string> fieldGuids, HttpRequestBase request)
        {
            try
            {
                // get submitted questions
                var submittedQuestions = await GetSubmittedQuestionsFromRequestAsync(questionnaireID, fieldGuids, request);

                return await Services.QuestionnaireSubmissionService.CreateSubmissionAsync(questionnaireID, submittedQuestions);
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

        private async Task<IList<IQuestionSubmit>> GetSubmittedQuestionsFromRequestAsync(int questionnaireID, IList<string> fieldGuids, HttpRequestBase request)
        {
            // use specific form field names generated by JS
            var fieldDataSeparator = '_';
            var questionDataPrefix = "SubmitData" + fieldDataSeparator;
            var questionAnswerSuffix = fieldDataSeparator + "answer";

            var questions = new List<IQuestionSubmit>();

            if (fieldGuids == null)
            {
                return new List<IQuestionSubmit>();
            }

            // load questions from the questionnaire
            var questionnaire = await GetQuestionnaireAsync(questionnaireID);

            if (questionnaire == null)
            {
                throw new NullReferenceException($"Questionnaire with ID = '{questionnaireID}' was not found");
            }

            foreach (var questionGuid in fieldGuids)
            {
                // get question specific data from submitted form data
                var answerKey = questionDataPrefix + questionGuid + questionAnswerSuffix;

                var answer = request.Form[answerKey];

                var originalQuestion = questionnaire.Questions.Where(m => m.Guid.Equals(questionGuid, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (originalQuestion == null)
                {
                    throw new NullReferenceException($"Cannot set question with GUID = '{questionGuid}' because it does not exist");
                }

                // add question
                questions.Add(new QuestionSubmit()
                {
                    Guid = questionGuid,
                    QuestionText = originalQuestion.QuestionText,
                    CorrectAnswer = originalQuestion.CorrectAnswer,
                    Answer = answer,
                    QuestionType = originalQuestion.QuestionType
                });
            }

            return questions;
        }

        #endregion

    }
}
