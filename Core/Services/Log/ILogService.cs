using System.Linq;
using System.Threading.Tasks;
using Entity;
using System;

namespace Core.Services
{
    public interface ILogService
    {
        Task DeleteAsync(int id);
        Task<Log> GetAsync(int id);
        IQueryable<Log> GetAll();
        Task InsertAsync(Log obj);
        Task UpdateAsync(Log obj);
        void LogException(Exception ex, string url = null, string userName = null);
        Task LogExceptionAsync(Exception ex);
        Task LogExceptionAsync(Exception ex, string url = null, string userName = null);
    }
}