using UI.Builders.Auth.Forms;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthEditThesisView : MasterView
    {
        public AuthAddEditThesisForm ThesisForm { get; set; }
        public bool ThesisExists
        {
            get
            {
                return ThesisForm.ID != 0;
            }
        }
    }
}
