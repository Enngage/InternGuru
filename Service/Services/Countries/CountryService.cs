using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Countries
{
    public class CountryService :  BaseService<Country>, ICountryService
    {

        public CountryService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var country = AppContext.Countries.Find(id);

            if (country != null)
            {
                // delete country
                AppContext.Countries.Remove(country);

                // touch cache keys
                TouchDeleteKeys(country);

                // save changes
                return SaveChangesAsync(SaveEventType.Delete, country);
            }

            return Task.FromResult(0);
        }

        public Task<Country> GetAsync(int id)
        {
            return AppContext.Countries.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Country> GetAll()
        {
            return AppContext.Countries;
        }

        public IQueryable<Country> GetSingle(int id)
        {
            return AppContext.Countries.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Country obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.Countries.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            return SaveChangesAsync(SaveEventType.Insert, obj);
        }

        public Task<int> UpdateAsync(Country obj)
        {
            var country = AppContext.Countries.Find(obj.ID);

            if (country == null)
            {
                throw new NotFoundException($"Country with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(country).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(country);

            // save changes
            return SaveChangesAsync(SaveEventType.Update, obj, country);
        }

        public async Task<IEnumerable<Country>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
