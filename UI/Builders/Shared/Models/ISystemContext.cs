using Identity;
using Service.Context;

namespace UI.Builders.Shared.Models
{
    public interface ISystemContext
    {
        IAppContext AppContext { get; }
        ApplicationUserManager ApplicationUserManager { get; }
        ApplicationSignInManager ApplicationSignInManager { get; }
    }
}
