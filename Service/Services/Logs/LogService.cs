using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Logs
{
    public class LogService : BaseService<Log>, ILogService
    {

        public LogService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        #region IService members

        public override Task<int> UpdateAsync(Log obj)
        {
            // get log
            var log = AppContext.Logs.Find(obj.ID);

            if (log == null)
            {
                throw new NotFoundException($"Log with ID: {obj.ID} not found");
            }

            // keep the created date
            obj.Created = log.Created;

            // save changes
            return base.UpdateAsync(obj, log);
        }

        public Task LogExceptionAsync(Exception ex)
        {
            return LogExceptionAsync(ex, null);
        }

        public Task LogExceptionAsync(Exception ex, string url, string userName = null)
        {
            var innerException = ex.InnerException?.ToString() ?? string.Empty;
            var log = new Log()
            {
                ApplicationUserName = userName,
                Url = url,
                ExceptionMessage = ex.Message,
                InnerException = innerException,
                Created = DateTime.Now,
                Stacktrace = ex.StackTrace,
            };

            return InsertAsync(log);
        }

        public void LogException(Exception ex, string url, string userName)
        {
            var innerException = ex.InnerException?.ToString() ?? string.Empty;
            var log = new Log()
            {
                ApplicationUserName = userName,
                Url = url,
                ExceptionMessage = ex.Message,
                InnerException = innerException,
                Created = DateTime.Now,
                Stacktrace = ex.StackTrace,
            };

            base.InsertObject(this.AppContext.Logs, log); 
        }


        public override IDbSet<Log> GetEntitySet()
        {
            return this.AppContext.Logs;
        }
    }

    #endregion
}
