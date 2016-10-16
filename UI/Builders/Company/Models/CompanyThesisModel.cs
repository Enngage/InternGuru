
namespace UI.Builders.Company.Models
{
    public class CompanyThesisModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public bool IsPaid { get; set; }
        public int Amount { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCodeName { get; set; }
        public bool CurrencyDisplaySignOnLeft { get; set; }
    }
}
