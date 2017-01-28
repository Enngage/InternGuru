using UI.Builders.Account.Forms;
using UI.Builders.Master.Views;

namespace UI.Builders.Account.Views
{
    public class ResetPasswordViewModel : MasterView
    {
        public ResetPasswordForm ResetPasswordForm { get; set; }
        public bool InvalidCodeToken { get; set; }
        public string Code { get; set; }
    }
}
