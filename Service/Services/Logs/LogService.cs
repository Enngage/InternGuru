using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;

namespace Service.Services.Logs
{
    public class LogService : BaseService<Log>, ILogService
    {

        public LogService(IAppContext appContext, ICacheService cacheService) : base(appContext, cacheService) { }

        #region IService members

        public Task<int> InsertAsync(Log obj)
        {
            AppContext.Logs.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            // get log
            var log = AppContext.Logs.Find(id);

            if (log != null)
            {
                // delete log
                AppContext.Logs.Remove(log);

                // touch cache keys
                TouchDeleteKeys(log);

                // fire event
                OnDelete(log);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<int> UpdateAsync(Log obj)
        {
            // get log
            var log = AppContext.Logs.Find(obj.ID);

            if (log == null)
            {
                throw new NotFoundException($"Log with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, log);

            // keep the created date
            obj.Created = log.Created;

            // update log
            AppContext.Entry(log).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(log);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public IQueryable<Log> GetAll()
        {
            return AppContext.Logs;
        }

        public IQueryable<Log> GetSingle(int id)
        {
            return AppContext.Logs.Where(m => m.ID == id).Take(1);
        }

        public Task<Log> GetAsync(int id)
        {
            return AppContext.Logs.FirstOrDefaultAsync(m => m.ID == id);
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

        public void LogException(Exception ex, string url, string userName = null)
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

            AppContext.Logs.Add(log);
            SaveChanges();
        }

        public async Task<IEnumerable<Log>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }

    #endregion
}
