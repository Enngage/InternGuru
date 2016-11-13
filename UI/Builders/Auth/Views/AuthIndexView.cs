using PagedList;
using System.Collections.Generic;
using UI.Builders.Auth.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthIndexView : MasterView
    {
        public IEnumerable<AuthInternshipListingModel> Internships { get; set; }
        public IEnumerable<AuthThesisListingModel> Theses { get; set; }
        public IPagedList<AuthConversationModel> Conversations { get; set; }
        public int NotReadMessagesCount { get; set; }
    }
}
