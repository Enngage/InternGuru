using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace Core.Services
{
    public interface ICompanyService
    {
        Task DeleteAsync(int id);
        IQueryable<Company> GetAll();
        Task<Company> GetAsync(int id);
        Task InsertAsync(Company obj);
        Task UpdateAsync(Company obj);
    }
}