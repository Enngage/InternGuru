
using System.Collections.Generic;

namespace Service.Services.Questionaries
{
    public interface IQuestion
    {
        /// <summary>
        /// Type of the question
        /// </summary>
        IQuestionType QuestionType { get; set; }

        /// <summary>
        /// Text of the question
        /// </summary>
        string QuestionText { get; set; }

        /// <summary>
        /// Answer of the question
        /// </summary>
        string Answer { get; set; }

        /// <summary>
        /// Correct answer of the question
        /// </summary>
        string CorrectAnswer { get; set; }

        /// <summary>
        /// Data of question
        /// </summary>
        IList<IQuestionData> Data { get; set; }
    }
}
