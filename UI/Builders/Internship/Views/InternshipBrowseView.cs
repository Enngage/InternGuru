using System;
using System.Collections.Generic;
using UI.Builders.Internship.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Internship.Views
{
    public class InternshipBrowseView : MasterView
    {
        public IEnumerable<InternshipBrowseModel> Internships { get; set; }
        public IEnumerable<InternshipCategoryModel> InternshipCategories { get; set; }
    }
}
