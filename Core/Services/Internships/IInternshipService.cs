using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace Core.Services
{
    public interface IInternshipService
    {
        Task DeleteAsync(int id);
        IQueryable<Internship> GetAll();
        Task<Internship> GetAsync(int id);
        Task InsertAsync(Internship obj);
        Task UpdateAsync(Internship obj);
    }
}