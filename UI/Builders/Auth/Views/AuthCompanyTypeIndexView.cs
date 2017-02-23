using PagedList;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Views
{
    public class AuthCompanyTypeIndexView : AuthMasterView
    {
        public IPagedList<AuthConversationModel> ConversationsPaged { get; set; }
    }
}
