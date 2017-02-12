using Core.Models;
using Identity;
using Service.Context;

namespace UI.Builders.Shared.Models
{
    public class SystemContext : ISystemContext
    {
        public IAppContext AppContext { get; }
        public ApplicationUserManager ApplicationUserManager { get; }
        public ApplicationSignInManager ApplicationSignInManager { get; }
        public IUser CurrentUser { get; }

        public SystemContext(
            IAppContext appContext,
            ApplicationUserManager applicationUserManager,
            ApplicationSignInManager applicationSignInManager,
            IUser currentUser
            )
        {
            AppContext = appContext;
            ApplicationUserManager = applicationUserManager;
            ApplicationSignInManager = applicationSignInManager;
            CurrentUser = currentUser;
        }
    }
}
