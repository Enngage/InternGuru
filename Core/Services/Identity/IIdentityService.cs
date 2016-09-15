using Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IIdentityService 
    {
        IQueryable<ApplicationUser> GetSingle(string applicationUserId);
        IQueryable<ApplicationUser> GetAll();
        Task<ApplicationUser> GetAsync(string applicationUserId);
        Task<int> UpdateAsync(ApplicationUser user);
    }
}