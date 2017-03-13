using PagedList;
using UI.Builders.Company.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Company.Views
{
    public class CompanyBrowseView : MasterView
    {
        public IPagedList<CompanyBrowseModel> Companies { get; set; }
    }
}
