using System;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Helpers;
using Service.Context;
using UI.Base;
using UI.Builders.Company.Models;
using UI.Builders.Master;
using UI.Builders.Questionnaire;
using UI.Builders.Questionnaire.Data;
using UI.Events;
using UI.Helpers;

namespace Web.Api
{
    public class QuestionnaireController : BaseApiController
    {
        readonly QuestionnaireBuilder _questionnaireBuilder;

        public QuestionnaireController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, QuestionnaireBuilder questionnaireBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _questionnaireBuilder = questionnaireBuilder;
        }

        #region Actions

        [HttpPost]
        public async Task<IHttpActionResult> DeleteQuestionnaire(QuestionnaireDeleteQuery query)
        {
            try
            {
                await _questionnaireBuilder.DeleteQuestionnaireAsync(query.QuestionnaireID);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion
    }
}