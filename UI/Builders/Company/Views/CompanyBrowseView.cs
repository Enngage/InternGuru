using System.Collections.Generic;
using UI.Builders.Company.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Company.Views
{
    public class CompanyBrowseView : MasterView
    {
        public IEnumerable<CompanyBrowseModel> Companies { get; set; }
    }
}
