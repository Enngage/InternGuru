using System;
using System.Collections.Generic;
using Service.Services.Questionnaires;

namespace UI.Builders.Auth.Models
{
    public class AuthQuestionnaireSubmissionModel
    {
        public int QuestionnaireID { get; set; }
        public string QuestionnaireName { get; set; }
        public int ID { get; set; }
        public IList<IQuestionSubmit> Questions { get; set; }
        public string CreatedByApplicationUserName { get; set; }
        public string CreatedByApplicationUserId { get; set; }
        public string CreatedByNickname { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public string SubmissionXml { get; set; }
        public DateTime Created { get; set; }
    }
}
