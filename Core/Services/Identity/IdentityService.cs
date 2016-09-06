using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Entity;
using Cache;

namespace Core.Services
{
    public class Identityservice : BaseService<EntityAbstract>, IIdentityService // Use EntityAbstract because ApplicationUser is not inheriting it
    {

        public Identityservice(IAppContext appContext, ICacheService cacheService) : base(appContext, cacheService) { }

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
    }
}
