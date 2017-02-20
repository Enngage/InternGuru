using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Xml;
using Entity;

namespace Service.Services.Questionnaires
{
    public class QuestionnaireSubmissionService : BaseService<QuestionnaireSubmission>, IQuestionnaireSubmissionService
    {
        private const string QuestionXmlRootElement = "Questions";
        private const string QuestionXmlElement = "Question";
        private const string QuestionTextXmlElement = "Text";
        private const string QuestionGuidElement = "Guid";
        private const string QuestionTypeXmlElement = "Type";
        private const string QuestionAnswerXmlElement = "Answer";
        private const string QuestionCorrectAnswerXmlElement = "CorrectAnswer";

        public QuestionnaireSubmissionService(IServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public override IDbSet<QuestionnaireSubmission> GetEntitySet()
        {
            return AppContext.QuestionnaireSubmissions;
        }


        /// <summary>
        /// Creates new submission
        /// </summary>
        /// <param name="questionnaireID">QuestionnaireID</param>
        /// <param name="submittedQuestions">Submitted questions</param>
        /// <returns></returns>
        public async Task<IInsertActionResult> CreateSubmissionAsync(int questionnaireID, IList<IQuestionSubmit> submittedQuestions)
        {
            var submission = new QuestionnaireSubmission()
            {
                QuestionnaireID = questionnaireID,
                SubmissionXml = GetSubmissionXml(submittedQuestions)
            };

            return await InsertAsync(submission);
        }

        /// <summary>
        /// Gets XML from given question submits
        /// </summary>
        /// <param name="questions">Submitted questions and answers</param>
        /// <returns>XML</returns>
        public string GetSubmissionXml(IList<IQuestionSubmit> questions)
        {
            var doc = new XmlDocument();

            var rootXmlElement = doc.AppendChild(doc.CreateElement(QuestionXmlRootElement));

            foreach (var question in questions)
            {
                // add question to xml
                var questionElement = rootXmlElement.AppendChild(doc.CreateElement(QuestionXmlElement));

                // set type
                questionElement.AppendChild(doc.CreateElement(QuestionTypeXmlElement)).InnerText = question.QuestionType;

                // set guid
                questionElement.AppendChild(doc.CreateElement(QuestionGuidElement)).InnerText = question.Guid;

                // set text of question
                questionElement.AppendChild(doc.CreateElement(QuestionTextXmlElement)).InnerText = question.QuestionText;

                // set answer
                questionElement.AppendChild(doc.CreateElement(QuestionAnswerXmlElement)).InnerText = question.Answer;

                // set correct answer
                questionElement.AppendChild(doc.CreateElement(QuestionCorrectAnswerXmlElement)).InnerText = question.CorrectAnswer;
            }

            return doc.OuterXml;
        }

        /// <summary>
        /// Gets submitted questions from given XML
        /// </summary>
        /// <param name="submissionXml">Submission XML</param>
        /// <returns>Submitted questions</returns>
        public IList<IQuestionSubmit> GetSubmitsFromXml(string submissionXml)
        {
            if (string.IsNullOrEmpty(submissionXml))
            {
                return null;
            }

            var doc = new XmlDocument();
            doc.LoadXml(submissionXml);

            var rootElement = doc.SelectSingleNode("//" + QuestionXmlRootElement);

            if (rootElement == null)
            {
                throw new InvalidOperationException("Submission has invalid root element");
            }

            var questions = new List<IQuestionSubmit>();

            foreach (XmlNode questionElement in rootElement.ChildNodes)
            {
                var newQuestion = new QuestionSubmit()
                {
                    QuestionText = questionElement[QuestionTextXmlElement]?.InnerText,
                    QuestionType = questionElement[QuestionTypeXmlElement]?.InnerText,
                    Guid = questionElement[QuestionGuidElement]?.InnerText,
                    Answer = questionElement[QuestionAnswerXmlElement]?.InnerText,
                    CorrectAnswer = questionElement[QuestionCorrectAnswerXmlElement]?.InnerText,
                };

                // add question
                questions.Add(newQuestion);
            }

            return questions;
        }
    }
}
