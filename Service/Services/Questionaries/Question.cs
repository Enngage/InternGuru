using System.Collections.Generic;

namespace Service.Services.Questionaries
{
    public class Question : IQuestion
    {
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public IList<IQuestionData> Data { get; set; }
    }
}
