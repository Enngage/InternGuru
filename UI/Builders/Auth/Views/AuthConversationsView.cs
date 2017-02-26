

using PagedList;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Views
{
    public class AuthConversationsView : AuthMasterView
    {
        public IPagedList<AuthConversationModel> ConversationsPaged { get; set; }
    }
}
