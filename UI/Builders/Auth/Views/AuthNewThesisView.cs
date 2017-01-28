using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Views
{
    public class AuthNewThesisView : AuthMasterView
    {
        public AuthAddEditThesisForm ThesisForm { get; set; }
        public bool CanCreateThesis { get; set; }
    }
}
