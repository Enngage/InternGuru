using System.Collections;
using System.Collections.Generic;
using UI.Builders.Company.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Company.Views
{
    public class CompanyIndexView : MasterView
    {
        public IEnumerable<CompanyModel> Companies { get; set; }
    }
}
