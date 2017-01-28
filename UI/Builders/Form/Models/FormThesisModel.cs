
using System;

namespace UI.Builders.Form.Models
{
    public class FormThesisModel
    {
        public int ID { get; set; }
        public string ThesisName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCodeName { get; set; }
        public int CompanyID { get; set; }
        public Guid CompanyGuid { get; set; }
        public string ThesisCodeName { get; set; }
        public string ThesisTypeName { get; set; }
        public string ThesisTypeNameConverted { get; set; }
    }
}
