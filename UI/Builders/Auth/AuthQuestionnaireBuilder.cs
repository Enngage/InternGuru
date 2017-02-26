using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Entity.Base;
using PagedList;
using PagedList.EntityFramework;
using UI.Builders.Auth.Models;
using UI.Builders.Auth.Views;
using UI.Builders.Questionnaire;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Modules.Questionnaire.Forms;

namespace UI.Builders.Auth
{
    public class AuthQuestionnaireBuilder : AuthMasterBuilder
    {

        #region Builders

        private readonly QuestionnaireBuilder _questionnaireBuilder;

        #endregion

        #region Constructor

        public AuthQuestionnaireBuilder(ISystemContext systemContext, IServicesLoader servicesLoader, QuestionnaireBuilder questionnaireBuilder) : base(systemContext, servicesLoader)
        {
            _questionnaireBuilder = questionnaireBuilder;
        }

        #endregion

        #region Questionnaire

        public async Task<AuthQuestionnaireSubmissionsView> BuildQuestionnaireSubmissionsViewAsync(int questionnaireID, int? page)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            if (!await UserHasAccessToQuestionnaireAsync(questionnaireID))
            {
                return null;
            }

            var questionnaire = authMaster.CompanyMaster.Questionnaires.FirstOrDefault(m => m.ID == questionnaireID);

            if (questionnaire == null)
            {
                return null;
            }

            return new AuthQuestionnaireSubmissionsView()
            {
                AuthMaster = authMaster,
                SubmissionsPaged = await GetQuestionnaireSubmissionsAsync(questionnaireID, page),
                QuestionnaireID = questionnaireID,
                QuestionnaireName = questionnaire.QuestionnaireName
            };
        }

        public async Task<AuthSubmissionView> BuildQuestionnaireSubmissionViewAsync(int questionnaireID, int submissionID)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            if (!await UserHasAccessToQuestionnaireAsync(questionnaireID))
            {
                return null;
            }

            var questionnaire = authMaster.CompanyMaster.Questionnaires.FirstOrDefault(m => m.ID == questionnaireID);

            if (questionnaire == null)
            {
                return null;
            }

            return new AuthSubmissionView()
            {
                AuthMaster = authMaster,
                Submission = await GetQuestionnaireSubmissionAsync(questionnaireID, submissionID),
                QuestionnaireID = questionnaireID,
                QuestionnaireName = questionnaire.QuestionnaireName,
                QuestionnaireCreated = questionnaire.Created
            };
        }

        public async Task<AuthCompanyTypeIndexView> BuildQuestionnairesViewAsync()
        {
            return await BuildCompanyTypeIndexViewAsync();
        }

        public async Task<AuthNewQuestionnaireView> BuildQuestionnaireNewViewAsync(QuestionnaireCreateEditForm form = null)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthNewQuestionnaireView()
            {
                AuthMaster = authMaster,
                QuestionnaireForm = _questionnaireBuilder.GetForm(form)
            };
        }

        public async Task<AuthEditQuestionnaireView> BuildEditQuestionnaireViewAsync(int questionnaireID)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            var questionnaireForm = await _questionnaireBuilder.GetQuestionnaireEditFormAsync(questionnaireID);

            if (questionnaireForm == null)
            {
                return null;
            }

            return new AuthEditQuestionnaireView()
            {
                AuthMaster = authMaster,
                QuestionnaireForm = questionnaireForm
            };
        }

        public async Task<AuthEditQuestionnaireView> BuildEditQuestionnaireViewAsync(QuestionnaireCreateEditForm questionnaireForm)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (questionnaireForm == null)
            {
                return null;
            }

            if (authMaster == null)
            {
                return null;
            }

            return new AuthEditQuestionnaireView()
            {
                AuthMaster = authMaster,
                QuestionnaireForm = _questionnaireBuilder.GetForm(questionnaireForm)
            };
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets form submissions of given questionnaire
        /// </summary>
        /// <returns>Collection of form submissions</returns>
        private async Task<IPagedList<AuthQuestionnaireSubmissionModel>> GetQuestionnaireSubmissionsAsync(int questionnaireID, int? page)
        {
            var pageSize = 10;
            var pageNumber = (page ?? 1);

            var submissionsQuery = Services.QuestionnaireSubmissionService.GetAll()
                .Where(m => m.QuestionnaireID == questionnaireID)
                .Select(m => new AuthQuestionnaireSubmissionModel()
                {
                    ID = m.ID,
                    Created = m.Created,
                    CreatedByApplicationUserName = m.CreatedByApplicationUser.UserName,
                    CreatedByApplicationUserId = m.CreatedByApplicationUserId,
                    CreatedByFirstName = m.CreatedByApplicationUser.FirstName,
                    CreatedByLastName = m.CreatedByApplicationUser.LastName,
                    CreatedByNickname = m.CreatedByApplicationUser.Nickname,
                    QuestionnaireID = m.QuestionnaireID,
                    QuestionnaireName = m.Questionnaire.QuestionnaireName
                })
                .OrderByDescending(m => m.ID);

            var cacheSetup = Services.CacheService.GetSetup<AuthQuestionnaireSubmissionModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<QuestionnaireSubmission>(),
                EntityKeys.KeyDeleteAny<QuestionnaireSubmission>()
            };

            cacheSetup.ObjectID = questionnaireID;
            cacheSetup.PageNumber = pageNumber;
            cacheSetup.PageSize = pageSize;

            return await Services.CacheService.GetOrSet(async () => await submissionsQuery.ToPagedListAsync(pageNumber, pageSize), cacheSetup);
        }

        private async Task<bool> UserHasAccessToQuestionnaireAsync(int questionnaireID)
        {
            return (await FormGetQuestionnairesAsync()).FirstOrDefault(m => m.ID == questionnaireID) != null;
        }

        private async Task<AuthQuestionnaireSubmissionModel> GetQuestionnaireSubmissionAsync(int questionnaireID, int submissionID)
        {


            var submissionQuery = Services.QuestionnaireSubmissionService.GetSingle(submissionID)
                .Where(m => m.QuestionnaireID == questionnaireID)
                .Select(m => new AuthQuestionnaireSubmissionModel()
                {
                    ID = m.ID,
                    Created = m.Created,
                    CreatedByApplicationUserName = m.CreatedByApplicationUser.UserName,
                    CreatedByApplicationUserId = m.CreatedByApplicationUserId,
                    CreatedByFirstName = m.CreatedByApplicationUser.FirstName,
                    CreatedByLastName = m.CreatedByApplicationUser.LastName,
                    CreatedByNickname = m.CreatedByApplicationUser.Nickname,
                    QuestionnaireName = m.Questionnaire.QuestionnaireName,
                    QuestionnaireID = m.QuestionnaireID,
                    SubmissionXml = m.SubmissionXml
                });

            var cacheSetup = Services.CacheService.GetSetup<AuthQuestionnaireSubmissionModel>(GetSource());
            cacheSetup.Dependencies = new List<string>();

            cacheSetup.ObjectID = submissionID;

            var submission = await Services.CacheService.GetOrSet(async () => await submissionQuery.FirstOrDefaultAsync(), cacheSetup);

            if (submission == null)
            {
                return null;
            }

            submission.Questions = Services.QuestionnaireSubmissionService.GetSubmitsFromXml(submission.SubmissionXml);

            return submission;
        }

        #endregion

    }
}
