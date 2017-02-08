
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Questionaries
{
    public interface IQuestionareService : IService<Questionare>
    {
        Task<int> CreateQuestionare(string questionareName, IEnumerable<IQuestion> questions, string applicationUserId, int companyId);
        IEnumerable<IQuestion> GetQuestionsFromXml(string questionareXml);
        string GetQuestionareXml(IEnumerable<IQuestion> questions);
    }
}
