using UI.Builders.Company.Forms;
using UI.Builders.Company.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Company.Views
{
    public class CompanyDetailView : MasterView
    {
        public CompanyDetailModel Company { get; set; }
        public CompanyContactUsForm ContactUsForm { get; set; }
        public string Anchor { get; set; }
    }
}
