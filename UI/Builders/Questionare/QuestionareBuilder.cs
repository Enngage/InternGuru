using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity.Base;
using Newtonsoft.Json;
using Service.Exceptions;
using Service.Services;
using Service.Services.Questionaries;
using UI.Base;
using UI.Builders.Questionare.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;
using UI.Modules.Questionare.Forms;

namespace UI.Builders.Questionare
{
    public class QuestionareBuilder : BaseBuilder
    {

        #region Constructor

        public QuestionareBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions


        #endregion


        #region Methods

        public string GetInitialStateJson(IList<IQuestion> questions)
        {
            return questions == null ? null : JsonConvert.SerializeObject(questions);
        }

        public QuestionareCreateEditForm GetForm(QuestionareCreateEditForm form = null)
        {
            if (form?.Questions != null)
            {
                // calculate initial state by converting questions to JSON object
                form.InitialStateJson = GetInitialStateJson(form.Questions);
            }
            return form ?? new QuestionareCreateEditForm();
        }

        public async Task<QuestionareCreateEditForm> GetQuestionareEditFormAsync(int questionareID)
        {
            var quesionareQuery = Services.QuestionareService.GetSingle(questionareID)
                .Select(m => new QuestionareCreateEditForm()
                {
                    QuestionareName = m.QuestionareName,
                    ApplicationUserID = m.ApplicationUserId,
                    CompanyID = m.CompanyID,
                    QuestionareID = m.ID,
                    QuestionareXml = m.QuestionareDefinitionXml,
                });

            var questionare = await quesionareQuery.FirstOrDefaultAsync();

            if (questionare == null)
            {
                return null;
            }

            // get questions from XML
            questionare.Questions = Services.QuestionareService.GetQuestionsFromXml(questionare.QuestionareXml).ToList();

            // get initial json
            questionare.InitialStateJson = GetInitialStateJson(questionare.Questions);

            return questionare;
        }

        public async Task<QuestionareModel> GetQuestionareAsync(int questionareID)
        {
            // get companies from db/cache
            var cacheSetup = Services.CacheService.GetSetup<QuestionareModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<QuestionareModel>(questionareID)
            };

            var questionareQuery = Services.QuestionareService.GetSingle(questionareID)
                .Select(m => new QuestionareModel()
                {
                    ID = m.ID,
                    CodeName = m.CodeName,
                    CompanyID = m.CompanyID,
                    QuestionareName = m.QuestionareName,
                    ApplicationUserId = m.ApplicationUserId,
                    QuestionareDefinitionXml = m.QuestionareDefinitionXml
                });

            var questionare = await Services.CacheService.GetOrSetAsync(async () => await questionareQuery.FirstOrDefaultAsync(), cacheSetup);

            return questionare;
        } 

        public async Task<IInsertActionResult> CreateQuestionareAsync(string questionareName, IList<IQuestion> questions)
        {
            try
            {
                if (string.IsNullOrEmpty(questionareName))
                {
                    questionareName = "Bezejmenný dotazník";
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

                return await Services.QuestionareService.CreateQuestionare(questionareName, questions, CurrentUser.Id, CurrentCompany.CompanyID);
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
