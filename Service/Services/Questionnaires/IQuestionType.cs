using System.Collections.Generic;

namespace Service.Services.Questionnaires
{
    public interface IQuestionType
    {
        /// <summary>
        /// Display name of the question
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Type name of the question
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// File name of partial view processing the question
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets data of question
        /// </summary>
        /// <returns>Gets data of question</returns>
        IList<IQuestionData> GetData();
    }
}
