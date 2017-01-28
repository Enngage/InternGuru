using Identity;
using Service.Context;

namespace UI.Builders.Shared.Models
{
    public class SystemContext : ISystemContext
    {
        public IAppContext AppContext { get; private set; }
        public ApplicationUserManager ApplicationUserManager { get; private set; }
        public ApplicationSignInManager ApplicationSignInManager { get; private set; }

        public SystemContext(
            IAppContext appContext,
            ApplicationUserManager applicationUserManager,
            ApplicationSignInManager applicationSignInManager
            )
        {
            AppContext = appContext;
            ApplicationUserManager = applicationUserManager;
            ApplicationSignInManager = applicationSignInManager;
        }
    }
}
