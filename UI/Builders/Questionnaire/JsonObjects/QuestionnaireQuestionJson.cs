using System.Collections.Generic;
using Service.Services.Questionnaires;

namespace UI.Builders.Questionnaire.JsonObjects
{
    public class QuestionnaireQuestionJson
    {
        public string QuestionText { get; set; }

        public string QuestionType { get; set; }

        public IList<IQuestionData> QuestionData { get; set; }
    }
}
