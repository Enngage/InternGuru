using System.Collections.Generic;
using UI.Modules.Breadcrumbs.Models;

namespace UI.Modules.Breadcrumbs.Views
{
    public class BreadcrumbsView
    {
        public IList<BreadcrumbItem> Breadcrumbs { get; set; }
        public string Divider = "/";
    }
}
