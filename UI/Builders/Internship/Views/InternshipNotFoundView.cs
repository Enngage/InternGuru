using System.Collections.Generic;
using UI.Builders.Internship.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Internship.Views
{
    public class InternshipNotFoundView : MasterView
    {
        public IEnumerable<InternshipBrowseModel> LatestInternships { get; set; }
    }
}
