using Cache;
using EmailProvider;
using Service.Context;

namespace Service.Services
{
    public class ServiceDependencies : IServiceDependencies
    {
        public IAppContext AppContext { get; set; }
        public ICacheService CacheService { get; }
        public IEmailProvider EmailProvider { get; }

        public ServiceDependencies(
            IAppContext appContext,
            ICacheService cacheService,
            IEmailProvider emailProvider
        )
        {
            AppContext = appContext;
            CacheService = cacheService;
            EmailProvider = emailProvider;
        }
    }
}
