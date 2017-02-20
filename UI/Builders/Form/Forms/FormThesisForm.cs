using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UI.Base;

namespace UI.Builders.Form.Forms
{
    public class FormThesisForm : BaseForm
    {
        [Required(ErrorMessage = "Nelze odeslat prázdnou zprávu")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Nevalidní práce")]
        public int ThesisID { get; set; }
        public string QuestionsJson { get; set; }
        public bool HasAttachedQuestionnaire => !string.IsNullOrEmpty(QuestionsJson);
        public IList<string> FieldGuids { get; set; }
    }
}
