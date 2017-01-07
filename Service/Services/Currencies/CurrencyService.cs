using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Logs;

namespace Service.Services.Currencies
{
    public class CurrencyService :  BaseService<Currency>, ICurrencyService
    {

        public CurrencyService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var currency = AppContext.Currencies.Find(id);

            if (currency != null)
            {
                // delete currency
                AppContext.Currencies.Remove(currency);

                // touch cache keys
                TouchDeleteKeys(currency);

                // fire event
                OnDelete(currency);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Currency> GetAsync(int id)
        {
            return AppContext.Currencies.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Currency> GetAll()
        {
            return AppContext.Currencies;
        }

        public IQueryable<Currency> GetSingle(int id)
        {
            return AppContext.Currencies.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Currency obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.Currencies.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Currency obj)
        {
            var currency = AppContext.Currencies.Find(obj.ID);

            if (currency == null)
            {
                throw new NotFoundException($"Currency with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, currency);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(currency).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(currency);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
