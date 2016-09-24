using System.Linq;
using System.Threading.Tasks;
using Entity;
using System;

namespace Core.Services
{
    public interface ILogService : IService<Log>
    {
        void LogException(Exception ex, string url = null, string userName = null);
        Task LogExceptionAsync(Exception ex);
        Task LogExceptionAsync(Exception ex, string url = null, string userName = null);
    }
}