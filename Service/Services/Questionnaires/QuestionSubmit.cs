

using System;

namespace Service.Services.Questionnaires
{
    public class QuestionSubmit : IQuestionSubmit
    {
        public string Guid { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public string CorrectAnswer { get; set; }
        public QuestionAnswerResultEnum Result
        {
            get
            {
                if (string.IsNullOrEmpty(CorrectAnswer))
                {
                    return QuestionAnswerResultEnum.NotATestQuestion;
                }
                return Answer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase) ? QuestionAnswerResultEnum.Correct : QuestionAnswerResultEnum.Wrong;
            }
        }

        public bool IsTestQuestion => Result != QuestionAnswerResultEnum.NotATestQuestion;
    }
}
