
namespace UI.Builders.Company.Models
{
    public class CompanyDetailInternshipModel
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string Title { get; set; }
        public bool IsPaid { get; set; }
        public bool HideAmount { get; set; }
        public string AmountTypeName { get; set; }
        public double Amount { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public bool CurrencyShowSignOnLeft { get; set; }
    }
}
