using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Logs;

namespace Service.Services.Identity
{
    public class Identityservice : BaseService<ApplicationUser>, IIdentityService 
    {

        public Identityservice(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<ApplicationUser> GetAsync(string applicationUserId)
        {
            return AppContext.Users.FirstOrDefaultAsync(m => m.Id == applicationUserId);
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return AppContext.Users;
        }

        public IQueryable<ApplicationUser> GetSingle(string applicationUserId)
        {
            return AppContext.Users.Where(m => m.Id == applicationUserId).Take(1);
        }

        public Task<int> UpdateAsync(ApplicationUser obj)
        {
            var user = AppContext.Users.Find(obj.Id);

            if (user == null)
            {
                throw new NotFoundException($"User with ID: {obj.Id} was not found");
            }

            // fire event
            OnUpdate(obj, user);

            // update user
            AppContext.Entry(user).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(obj);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
