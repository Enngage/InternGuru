using System.Collections.Generic;
using Service.Services.Questionaries;

namespace UI.Builders.Questionare.JsonObjects
{
    public class QuestionareQuestionJson
    {
        public string QuestionText { get; set; }

        public string QuestionType { get; set; }

        public IList<IQuestionData> QuestionData { get; set; }
    }
}
