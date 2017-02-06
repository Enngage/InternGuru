
using System.Collections.Generic;

namespace Service.Services.Questionaries
{
    public interface IQuestionareService
    {
        int CreateQuestionare(string questionText, IEnumerable<IQuestion> questions, string applicationUserId, int companyId);

        string GetQuestionareXml(IEnumerable<IQuestion> questions);
    }
}
