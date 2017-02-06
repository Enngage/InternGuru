using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Views
{
    public class AuthEditThesisView : AuthMasterView
    {
        public AuthAddEditThesisForm ThesisForm { get; set; }
        public bool ThesisExists => ThesisForm.ID != 0;
    }
}
