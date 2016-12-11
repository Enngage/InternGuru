using PagedList;
using System;
using System.Collections.Generic;
using UI.Builders.Master.Views;
using UI.Builders.Thesis.Models;

namespace UI.Builders.Thesis.Views
{
    public class ThesisBrowseView : MasterView
    {
        public IPagedList<ThesisBrowseModel> Theses { get; set; }
        public IEnumerable<ThesisCategoryModel> ThesisCategories { get; set; }
    }
}
