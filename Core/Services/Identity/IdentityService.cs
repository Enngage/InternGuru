using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Entity;
using Cache;
using Core.Exceptions;
using System.Collections.Generic;

namespace Core.Services
{
    public class Identityservice : BaseService<ApplicationUser>, IIdentityService 
    {

        public Identityservice(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<ApplicationUser> GetAsync(string applicationUserId)
        {
            return this.AppContext.Users.FirstOrDefaultAsync(m => m.Id == applicationUserId);
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return this.AppContext.Users;
        }

        public IQueryable<ApplicationUser> GetSingle(string applicationUserId)
        {
            return this.AppContext.Users.Where(m => m.Id == applicationUserId).Take(1);
        }

        public Task<int> UpdateAsync(ApplicationUser obj)
        {
            var user = this.AppContext.Users.Find(obj.Id);

            if (user == null)
            {
                throw new NotFoundException(string.Format("User with ID: {0} was not found", obj.Id));
            }

            // update user
            this.AppContext.Entry(user).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(obj);

            // fire event
            this.OnUpdate(obj);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
