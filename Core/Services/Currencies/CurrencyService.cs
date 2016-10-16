using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Entity;
using Cache;
using Core.Exceptions;
using System.Collections.Generic;

namespace Core.Services
{
    public class CurrencyService :  BaseService<Currency>, ICurrencyService
    {

        public CurrencyService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var currency = this.AppContext.Currencies.Find(id);

            if (currency != null)
            {
                // delete currency
                this.AppContext.Currencies.Remove(currency);

                // touch cache keys
                this.TouchDeleteKeys(currency);

                // fire event
                this.OnDelete(currency);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Currency> GetAsync(int id)
        {
            return this.AppContext.Currencies.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Currency> GetAll()
        {
            return this.AppContext.Currencies;
        }

        public IQueryable<Currency> GetSingle(int id)
        {
            return this.AppContext.Currencies.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Currency obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.Currencies.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Currency obj)
        {
            var currency = this.AppContext.Currencies.Find(obj.ID);

            if (currency == null)
            {
                throw new NotFoundException(string.Format("Currency with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, currency);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(currency).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(currency);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
