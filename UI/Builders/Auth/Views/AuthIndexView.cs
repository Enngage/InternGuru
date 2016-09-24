using PagedList;
using System.Collections.Generic;
using UI.Builders.Auth.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthIndexView : MasterView
    {
        public IEnumerable<AuthInternshipListingModel> Internships { get; set; }
        public IPagedList<AuthMessageModel> Messages { get; set; }
    }
}
