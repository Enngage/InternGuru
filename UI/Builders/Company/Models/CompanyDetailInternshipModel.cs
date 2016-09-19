
namespace UI.Builders.Company.Models
{
    public class CompanyDetailInternshipModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsPaid { get; set; }
        public string AmountType { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
