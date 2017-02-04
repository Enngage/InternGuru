using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Identity
{
    public class Identityservice : BaseService<ApplicationUser>, IIdentityService 
    {

        public Identityservice(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<ApplicationUser> GetAsync(string applicationUserId)
        {
            return AppContext.Users.FirstOrDefaultAsync(m => m.Id == applicationUserId);
        }

        public IQueryable<ApplicationUser> GetSingle(string applicationUserId)
        {
            return AppContext.Users.Where(m => m.Id == applicationUserId).Take(1);
        }

        public override IDbSet<ApplicationUser> GetEntitySet()
        {
            return this.AppContext.Users;
        }
    }
}
