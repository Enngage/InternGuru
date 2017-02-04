using System;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Logs
{
    public interface ILogService : IService<Log>
    {
        void LogException(Exception ex, string url = null, string userName = null);
        Task LogExceptionAsync(Exception ex);
        Task LogExceptionAsync(Exception ex, string url, string userName);
    }
}