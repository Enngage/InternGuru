using Cache;
using Core.Context;
using Core.Exceptions;
using Entity;
using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Core.Services
{
    public class LogService : ServiceAbstract, IService<Log>, ILogService
    {

        public LogService(IAppContext appContext, ICacheService cacheService) : base(appContext, cacheService) { }

        #region IService members

        public Task InsertAsync(Log obj)
        {
            this.AppContext.Logs.Add(obj);

            return this.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            // get log
            var log = this.AppContext.Logs.Find(id);

            if (log != null)
            {
                // delete log
                this.AppContext.Logs.Remove(log);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task UpdateAsync(Log obj)
        {
            // get log
            var log = this.AppContext.Logs.Find(obj.ID);

            if (obj == null)
            {
                throw new NotFoundException(string.Format("Log with ID: {0} not found", obj.ID));
            }

            // update log
            this.AppContext.Entry(obj).CurrentValues.SetValues(obj);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public IQueryable<Log> GetAll()
        {
            return this.AppContext.Logs;
        }

        public Task<Log> Get(int id)
        {
            return this.AppContext.Logs.FirstOrDefaultAsync(m => m.ID == id);
        }

        public Task LogExceptionAsync(Exception ex)
        {
            return LogExceptionAsync(ex, null);
        }

        public Task LogExceptionAsync(Exception ex, string url = null, string userName = null)
        {
            string innerException = ex.InnerException == null ? String.Empty : ex.InnerException.ToString();
            var log = new Log()
            {
                ApplicationUserName = userName,
                Url = url,
                ExceptionMessage = ex.Message,
                InnerException = innerException,
                Created = DateTime.Now,
                Stacktrace = ex.StackTrace,
            };

            return this.InsertAsync(log);
        }

        public void LogException(Exception ex, string url = null, string userName = null)
        {
            string innerException = ex.InnerException == null ? String.Empty : ex.InnerException.ToString();
            var log = new Log()
            {
                ApplicationUserName = userName,
                Url = url,
                ExceptionMessage = ex.Message,
                InnerException = innerException,
                Created = DateTime.Now,
                Stacktrace = ex.StackTrace,
            };

            this.AppContext.Logs.Add(log);
            this.SaveChanges();
        }
    }

    #endregion
}
