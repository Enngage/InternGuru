﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using System.Xml;
using Entity;

namespace Service.Services.Questionnaires
{
    public class QuestionnaireService : BaseService<Questionnaire>, IQuestionnaireService
    {
        private const string QuestionXmlRootElement = "Questions";
        private const string QuestionXmlElement = "Question";
        private const string QuestionTextXmlElement = "Text";
        private const string QuestionTypeXmlElement = "Type";
        private const string QuestionDataXmlElement = "Data";

        public QuestionnaireService(IServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public override IDbSet<Questionnaire> GetEntitySet()
        {
            return AppContext.Questionnaires;
        }

        public async Task<int> EditQuestionnaireAsync(int questionnaireID, string questionnaireName, IEnumerable<IQuestion> questions, string applicationUserId, int companyId)
        {
            if (string.IsNullOrEmpty(questionnaireName))
            {
                throw new NotSupportedException("Questionnaire name cannot be empty");
            }

            if (questions == null)
            {
                throw new NotSupportedException("Questionnaire needs to have at least 1 question");
            }

            // get existing obj
            var questionnaire = await GetSingleAsync(questionnaireID);

            if (questionnaire == null)
            {
                throw new ObjectNotFoundException($"Questionnaire with ID = '{questionnaireID}' was not found");
            }

            questionnaire.QuestionnaireName = questionnaireName;
            questionnaire.QuestionnaireXml = GetQuestionnaireXml(questions);

            return await UpdateAsync(questionnaire);
        }


        public Task<IInsertActionResult> CreateQuestionnaireAsync(string questionnaireName, IEnumerable<IQuestion> questions, string applicationUserId, int companyId)
        {
            if (string.IsNullOrEmpty(questionnaireName))
            {
                throw new NotSupportedException("Questionnaire name cannot be empty");
            }

            if (questions == null)
            {
                throw new NotSupportedException("Questionnaire needs to have at least 1 question");
            }

            if (string.IsNullOrEmpty(applicationUserId))
            {
                throw new ArgumentNullException(nameof(applicationUserId));
            }

            if (companyId == 0)
            {
                throw new NotSupportedException("Company is not assigned to Questionnaire");
            }

            var questionnaire = new Questionnaire()
            {
                CompanyID = companyId,
                CreatedByApplicationUserId = applicationUserId,
                QuestionnaireName = questionnaireName,
                QuestionnaireXml = GetQuestionnaireXml(questions)
            };

            return InsertAsync(questionnaire);
        }

        public IEnumerable<IQuestion> GetQuestionsFromXml(string questionnaireXml)
        {
            if (string.IsNullOrEmpty(questionnaireXml))
            {
                return null;
            }

            var doc = new XmlDocument();
            doc.LoadXml(questionnaireXml);

            var rootElement = doc.SelectSingleNode("//" + QuestionXmlRootElement);

            if (rootElement == null)
            {
                throw new InvalidOperationException("Questionnaire has invalid root element");
            }

            var questions = new List<IQuestion>();

            foreach (XmlNode questionElement in rootElement.ChildNodes)
            {
                var newQuestion = new Question()
                {
                    QuestionText = questionElement[QuestionTextXmlElement]?.InnerText,
                    QuestionType = questionElement[QuestionTypeXmlElement]?.InnerText,
                };

                var dataNodes = questionElement[QuestionDataXmlElement]?.ChildNodes;

                if (dataNodes == null)
                {
                    continue;
                }

                var questionData = new List<IQuestionData>();

                foreach (XmlNode data in dataNodes)
                {
                    questionData.Add(new QuestionData()
                    {
                        Value = data.InnerText,
                        Name = data.Name
                    });
                }

                newQuestion.Data = questionData;

                // add question
                questions.Add(newQuestion);
            }

            return questions;
        }

        public string GetQuestionnaireXml(IEnumerable<IQuestion> questions)
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
