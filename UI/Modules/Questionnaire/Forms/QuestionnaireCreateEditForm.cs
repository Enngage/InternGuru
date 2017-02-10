using System.Collections.Generic;
using Service.Services.Questionnaires;
using UI.Base;

namespace UI.Modules.Questionnaire.Forms
{
    public class QuestionnaireCreateEditForm : BaseForm
    {
        public int QuestionnaireID { get; set; }
        public string ApplicationUserID { get; set; }
        public int CompanyID { get; set; }
        public string QuestionnaireName { get; set; }
        public string QuesitonnaireXml { get; set; }
        public IList<string> FieldGuids { get; set; }
        public IList<IQuestion> Questions { get; set; }

        public string InitialStateJson { get; set; }

        public bool IsNewQuestionare => QuestionnaireID == 0;

    }
}
