using System.Collections.Generic;
using UI.Builders.Home.Models;
using UI.Builders.Internship.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Home.Views
{
    public class HomeIndexView : MasterView
    {
        public IList<HomeInternshipListingModel> Internships { get; set; }
        public IEnumerable<InternshipCategoryModel> Categories { get; set; }
    }
}
