using Cache;
using EmailProvider;
using Service.Context;

namespace Service.Services
{
    /// <summary>
    /// Dependencies of all service classes
    /// WARNING: Avoid circular dependencies between service classes
    /// </summary>
    public interface IServiceDependencies
    {
        IAppContext AppContext { get; set; }
        ICacheService CacheService { get; }
        IEmailProvider EmailProvider { get; }
    }
}
