using System.Collections.Generic;
using System.Xml;

namespace Service.Services.Questionaries
{
    public class QuestionareService : IQuestionareService
    {
        private const string QuestionXmlElement = "Question";
        private const string QuestionTextXmlElement = "Text";
        private const string QuestionTypeXmlElement = "Type";
        private const string QuestionDataXmlElement = "Data";

        public int CreateQuestionare(string questionText, IEnumerable<IQuestion> questions, string applicationUserId, int companyId)
        {


            return 0;
        }

        public string GetQuestionareXml(IEnumerable<IQuestion> questions)
        {
            var doc = new XmlDocument();

            foreach (var question in questions)
            {
                // add question to xml
                var questionElement = doc.AppendChild(doc.CreateElement(QuestionXmlElement));

                // set type
                questionElement.AppendChild(doc.CreateElement(QuestionTypeXmlElement)).InnerText = question.QuestionType.TypeName;

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
