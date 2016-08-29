using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace Core.Services
{
    public interface IInternshipCategoryService
    {
        Task DeleteAsync(int id);
        IQueryable<InternshipCategory> GetAll();
        Task<InternshipCategory> GetAsync(int id);
        Task InsertAsync(InternshipCategory obj);
        Task UpdateAsync(InternshipCategory obj);
    }
}