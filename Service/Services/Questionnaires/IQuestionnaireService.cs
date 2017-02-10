using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Questionnaires
{
    public interface IQuestionnaireService : IService<Questionnaire>
    {
        Task<IInsertActionResult> CreateQuestionnaireAsync(string questionnaireName, IEnumerable<IQuestion> questions, string applicationUserId, int companyId);
        Task<int> EditQuestionnaireAsync(int questionnaireID, string questionnaireName, IEnumerable<IQuestion> questions, string applicationUserId, int companyId);
        IEnumerable<IQuestion> GetQuestionsFromXml(string questionnaireXml);
        string GetQuestionnaireXml(IEnumerable<IQuestion> questions);
    }
}
