using System.Collections.Generic;
using UI.Builders.Master.Views;
using UI.Builders.Thesis.Models;

namespace UI.Builders.Thesis.Views
{
    public class ThesisNotFoundView : MasterView
    {
        public IEnumerable<ThesisBrowseModel> LatestTheses { get; set; }
    }
}
