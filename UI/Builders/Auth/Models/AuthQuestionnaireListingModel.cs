using System;
using System.Collections.Generic;
using Service.Services.Questionnaires;

namespace UI.Builders.Auth.Models
{
    public class AuthQuestionnaireListingModel
    {
        public int ID { get; set; }
        public string QuestionnaireName { get; set; }
        public IList<IQuestion> Questions { get; set; }
        public DateTime Updated { get; set; }
        public string QuestionnaireXml { get; set; }
        public string CreatedByApplicationUserId { get; set; }
        public int CompanyID { get; set; }
    }
}
