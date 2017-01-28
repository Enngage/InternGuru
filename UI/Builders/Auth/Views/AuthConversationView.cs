using PagedList;
using UI.Builders.Auth.Models;
using System.Linq;
using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Views
{
    public class AuthConversationView : AuthMasterView
    {
        public AuthMessageModel LastMessage => Messages.FirstOrDefault();
        public AuthMessageUserModel ConversationUser { get; set; }
        public AuthMessageUserModel Me { get; set; }
        public IPagedList<AuthMessageModel> Messages { get; set; }
        public AuthMessageForm MessageForm { get; set; }
    }
}
