using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace Core.Services
{
    public interface ICompanyCategoryService
    {
        Task DeleteAsync(int id);
        IQueryable<CompanyCategory> GetAll();
        Task<CompanyCategory> GetAsync(int id);
        Task InsertAsync(CompanyCategory obj);
        Task UpdateAsync(CompanyCategory obj);
    }
}