using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Questionnaires
{
    public interface IQuestionnaireSubmissionService : IService<QuestionnaireSubmission>
    {
        Task<IInsertActionResult> CreateSubmissionAsync(int questionnaireID, IList<IQuestionSubmit> submittedQuestions);
        IDbSet<QuestionnaireSubmission> GetEntitySet();
        string GetSubmissionXml(IList<IQuestionSubmit> questions);
        IList<IQuestionSubmit> GetSubmitsFromXml(string submissionXml);
    }
}
