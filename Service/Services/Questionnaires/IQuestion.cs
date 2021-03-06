﻿
using System.Collections.Generic;

namespace Service.Services.Questionnaires
{
    public interface IQuestion
    {
        /// <summary>
        /// Indicates if question is required
        /// </summary>
        bool QuestionRequired { get; set; }

        /// <summary>
        /// Answer to question
        /// </summary>
        string Answer { get; set; }

        /// <summary>
        /// Type of the question
        /// </summary>
        string QuestionType { get; set; }

        /// <summary>
        /// Text of the question
        /// </summary>
        string QuestionText { get; set; }

        /// <summary>
        /// Correct answer of the question
        /// </summary>
        string CorrectAnswer { get; set; }

        /// <summary>
        /// Guid of the question
        /// </summary>
        string Guid { get; set; }

        /// <summary>
        /// Data of question
        /// </summary>
        IList<IQuestionData> Data { get; set; }
    }
}
