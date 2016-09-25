using UI.Builders.Company.Enums;
using UI.Builders.Company.Forms;
using UI.Builders.Company.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Company.Views
{
    public class CompanyDetailView : MasterView
    {
        private CompanyDetailMenuEnum activeTab = CompanyDetailMenuEnum.About;

        /// <summary>
        /// Value representing which tab in the right menu is active
        /// </summary>
        public CompanyDetailMenuEnum ActiveTab
        {
            get
            {
                return activeTab;
            }
            set
            {
                activeTab = value;
            }
        }
        public CompanyDetailModel Company { get; set; }
        public CompanyContactUsForm ContactUsForm { get; set; }
    }
}
