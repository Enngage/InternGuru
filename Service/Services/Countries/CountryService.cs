using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Service.Context;
using Entity;
using Cache;
using Service.Exceptions;
using System.Collections.Generic;

namespace Service.Services
{
    public class CountryService :  BaseService<Country>, ICountryService
    {

        public CountryService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var country = this.AppContext.Countries.Find(id);

            if (country != null)
            {
                // delete country
                this.AppContext.Countries.Remove(country);

                // touch cache keys
                this.TouchDeleteKeys(country);

                // fire event
                this.OnDelete(country);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Country> GetAsync(int id)
        {
            return this.AppContext.Countries.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Country> GetAll()
        {
            return this.AppContext.Countries;
        }

        public IQueryable<Country> GetSingle(int id)
        {
            return this.AppContext.Countries.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Country obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.Countries.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Country obj)
        {
            var country = this.AppContext.Countries.Find(obj.ID);

            if (country == null)
            {
                throw new NotFoundException(string.Format("Country with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, country);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(country).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(country);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Country>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
