using UI.Builders.Auth.Forms;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthRegisterCompanyView : MasterView
    {
        public bool CompanyIsCreated { get; set; }
        public AuthAddEditCompanyForm CompanyForm { get; set; }
    }
}
