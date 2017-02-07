using System.Collections.Generic;
using UI.Base;

namespace UI.Modules.Questionare.Forms
{
    public class QuestionareCreateForm : BaseForm
    {
        public string QuestionareName { get; set; }
        public IList<string> FieldGuids { get; set; }

    }
}
