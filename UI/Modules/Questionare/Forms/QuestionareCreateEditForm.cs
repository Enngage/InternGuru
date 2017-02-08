using System.Collections.Generic;
using Service.Services.Questionaries;
using UI.Base;

namespace UI.Modules.Questionare.Forms
{
    public class QuestionareCreateEditForm : BaseForm
    {
        public int QuestionareID { get; set; }
        public string ApplicationUserID { get; set; }
        public int CompanyID { get; set; }
        public string QuestionareName { get; set; }
        public string QuestionareXml { get; set; }
        public IList<string> FieldGuids { get; set; }
        public IList<IQuestion> Questions { get; set; }

        public bool IsNewQuestionare => QuestionareID == 0;

    }
}
