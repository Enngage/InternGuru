using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Xml;
using Entity;

namespace Service.Services.Questionaries
{
    public class QuestionareService : BaseService<Questionare>, IQuestionareService
    {
        private const string QuestionXmlRootElement = "Questions";
        private const string QuestionXmlElement = "Question";
        private const string QuestionTextXmlElement = "Text";
        private const string QuestionTypeXmlElement = "Type";
        private const string QuestionDataXmlElement = "Data";

        public QuestionareService(IServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public override IDbSet<Questionare> GetEntitySet()
        {
            return AppContext.Questionares;
        }

        public Task<int> CreateQuestionare(string questionareName, IEnumerable<IQuestion> questions, string applicationUserId, int companyId)
        {
            if (string.IsNullOrEmpty(questionareName))
            {
                throw new NotSupportedException("Questionare name cannot be empty");
            }

            if (questions == null)
            {
                throw new NotSupportedException("Questionare needs to have at least 1 question");
            }

            if (string.IsNullOrEmpty(applicationUserId))
            {
                throw new ArgumentNullException(nameof(applicationUserId));
            }

            if (companyId == 0)
            {
                throw new NotSupportedException("Company is not assigned to questionare");
            }

            var questionare = new Questionare()
            {
                CompanyID = companyId,
                ApplicationUserId = applicationUserId,
                QuestionareName = questionareName,
                QuestionareDefinitionXml = GetQuestionareXml(questions)
            };

            return InsertAsync(questionare);
        }

        public IEnumerable<IQuestion> GetQuestionsFromXml(string questionareXml)
        {
            if (string.IsNullOrEmpty(questionareXml))
            {
                return null;
            }

            var doc = new XmlDocument();
            doc.LoadXml(questionareXml);

            var rootElement = doc.SelectSingleNode("//" + QuestionXmlRootElement);

            if (rootElement == null)
            {
                throw new InvalidOperationException("Questionare has invalid root element");
            }

            var questions = new List<IQuestion>();

            foreach (XmlNode questionElement in rootElement.ChildNodes)
            {
                var newQuestion = new Question()
                {
                    QuestionText = questionElement.SelectSingleNode("//" + QuestionTextXmlElement)?.InnerText,
                    QuestionType = questionElement.SelectSingleNode("//" + QuestionTypeXmlElement)?.InnerText,
                };

                var dataNodes = questionElement.SelectSingleNode("//" + QuestionDataXmlElement)?.ChildNodes;

                if (dataNodes == null)
                {
                    continue;
                }

                var questionData = new List<IQuestionData>();

                foreach (XmlNode data in dataNodes)
                {
                    questionData.Add(new QuestionData()
                    {
                        Value  = data.InnerText,
                        Name = data.Name
                    });
                }

                newQuestion.Data = questionData;

                // add question
                questions.Add(newQuestion);
            }

            return questions;
        }

        public string GetQuestionareXml(IEnumerable<IQuestion> questions)
        {
            var doc = new XmlDocument();

            var rootXmlElement = doc.AppendChild(doc.CreateElement(QuestionXmlRootElement));

            foreach (var question in questions)
            {
                // add question to xml
                var questionElement = rootXmlElement.AppendChild(doc.CreateElement(QuestionXmlElement));

                // set type
                questionElement.AppendChild(doc.CreateElement(QuestionTypeXmlElement)).InnerText = question.QuestionType;

                // set text of question
                questionElement.AppendChild(doc.CreateElement(QuestionTextXmlElement)).InnerText = question.QuestionText;

                // set data main element
                var dataElem = questionElement.AppendChild(doc.CreateElement(QuestionDataXmlElement));

                // set all data elements
                foreach (var dataElement in question.Data)
                {
                    dataElem.AppendChild(doc.CreateElement(dataElement.Name)).InnerText = dataElement.Value;
                }
            }

            return doc.OuterXml;
        }

    }
}
