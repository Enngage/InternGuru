using UI.Builders.Auth.Forms;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthNewThesisView : MasterView
    {
        public AuthAddEditThesisForm ThesisForm { get; set; }
        public bool CanCreateThesis { get; set; }
    }
}
