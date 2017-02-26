using System;
using System.Linq;
using Service.Services.Questionnaires;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Views
{
    public class AuthSubmissionView : AuthMasterView
    {
        public AuthQuestionnaireSubmissionModel Submission { get; set; }
        public string QuestionnaireName { get; set; }
        public int QuestionnaireID { get; set; }
        public DateTime QuestionnaireCreated { get; set; }
        public int QuestionsWithCorrectAnswerCount => Submission.Questions.Where(m => m.Result != QuestionAnswerResultEnum.NotATestQuestion).Count();
        public int WronglyAnsweredQuestionsCount => Submission.Questions.Where(m => m.Result == QuestionAnswerResultEnum.Wrong).Count();
        public int CorrectlyAnsweredQuestionsCount => Submission.Questions.Where(m => m.Result == QuestionAnswerResultEnum.Correct).Count();
    }
}
