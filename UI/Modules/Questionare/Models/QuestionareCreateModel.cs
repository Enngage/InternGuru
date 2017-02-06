using System.Collections.Generic;
using Service.Services.Questionaries;

namespace UI.Modules.Questionare.Models
{
    public class QuestionareCreateModel
    {
        public QuestionareCreateModel(string questionareName, IList<IQuestionType> questionTypes)
        {
            QuestionareName = questionareName;
            QuestionTypes = questionTypes;
        }

        public string QuestionareName { get; }
        public IList<IQuestionType> QuestionTypes { get; }

    }
}
