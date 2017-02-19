using System;

namespace UI.Builders.Thesis.Models
{
    public class ThesisBrowseModel
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string ThesisName { get; set; }
        public int InternshipCategoryID { get; set; }
        public string InternshipCategoryName { get; set; }
        public int Amount { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public bool CurrencyShowSignOnLeft { get; set; }
        public DateTime Created { get; set; }
        public bool IsPaid { get; set; }
        public bool HideAmount { get; set; }
        public int ThesisTypeID { get; set; }
        public string ThesisTypeName { get; set; }
        public string ThesisTypeCodeName { get; set; }
        public string ThesisTypeNameConverted { get; set; }
        public ThesisDetailCompanyModel Company { get; set; }
    }
}
