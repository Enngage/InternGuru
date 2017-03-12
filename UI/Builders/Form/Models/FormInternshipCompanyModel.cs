
using System;

namespace UI.Builders.Form.Models
{
    public class FormInternshipModel
    {
        public int CompanyID { get; set; }
        public Guid CompanyGuid { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCodeName { get; set; }
        public int InternshipID { get; set; }
        public string InternshipTitle { get; set; }
        public int? QuestionnaireID { get; set; }
        public string InternshipCodeName { get; set; }

    }
}
