using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Views
{
    public class AuthRegisterCompanyView : AuthMasterView
    {
        public bool CompanyIsCreated { get; set; }
        public AuthAddEditCompanyForm CompanyForm { get; set; }
    }
}
