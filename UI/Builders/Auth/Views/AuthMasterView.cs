using UI.Builders.Auth.Models;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthMasterView : MasterView
    {
        public AuthMasterModel AuthMaster { get; set; }
        public AuthTabs Tabs { get; set; } = new AuthTabs();
    }
}
