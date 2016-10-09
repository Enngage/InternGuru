using Cache;
using Core.Context;
using Core.Exceptions;
using Entity;
using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Services
{
    public class LogService : BaseService<Log>, ILogService
    {

        public LogService(IAppContext appContext, ICacheService cacheService) : base(appContext, cacheService) { }

        #region IService members

        public Task<int> InsertAsync(Log obj)
        {
            this.AppContext.Logs.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            // get log
            var log = this.AppContext.Logs.Find(id);

            if (log != null)
            {
                // delete log
                this.AppContext.Logs.Remove(log);

                // touch cache keys
                this.TouchDeleteKeys(log);

                // fire event
                this.OnDelete(log);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<int> UpdateAsync(Log obj)
        {
            // get log
            var log = this.AppContext.Logs.Find(obj.ID);

            if (log == null)
            {
                throw new NotFoundException(string.Format("Log with ID: {0} not found", obj.ID));
            }

            // keep the created date
            obj.Created = log.Created;

            // update log
            this.AppContext.Entry(log).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(log);

            // fire event
            this.OnUpdate(log);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public IQueryable<Log> GetAll()
        {
            return this.AppContext.Logs;
        }

        public IQueryable<Log> GetSingle(int id)
        {
            return this.AppContext.Logs.Where(m => m.ID == id).Take(1);
        }

        public Task<Log> GetAsync(int id)
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

        public async Task<IEnumerable<Log>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }

    #endregion
}
