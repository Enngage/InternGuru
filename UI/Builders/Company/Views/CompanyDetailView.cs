using UI.Builders.Company.Enums;
using UI.Builders.Company.Forms;
using UI.Builders.Company.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Company.Views
{
    public class CompanyDetailView : MasterView
    {
        /// <summary>
        /// Value representing which tab in the right menu is active
        /// </summary>
        public CompanyDetailMenuEnum ActiveTab { get; set; } = CompanyDetailMenuEnum.About;

        public CompanyDetailModel Company { get; set; }
        public CompanyContactUsForm ContactUsForm { get; set; }
    }
}
