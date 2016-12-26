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
    public class HomeOfficeOptionService :  BaseService<HomeOfficeOption>, IHomeOfficeOptionService
    {

        public HomeOfficeOptionService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var homeOfficeOption = this.AppContext.HomeOfficeOptions.Find(id);

            if (homeOfficeOption != null)
            {
                this.AppContext.HomeOfficeOptions.Remove(homeOfficeOption);

                // touch cache keys
                this.TouchDeleteKeys(homeOfficeOption);

                // fire event
                this.OnDelete(homeOfficeOption);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<HomeOfficeOption> GetAsync(int id)
        {
            return this.AppContext.HomeOfficeOptions.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<HomeOfficeOption> GetAll()
        {
            return this.AppContext.HomeOfficeOptions;
        }

        public IQueryable<HomeOfficeOption> GetSingle(int id)
        {
            return this.AppContext.HomeOfficeOptions.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(HomeOfficeOption obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.HomeOfficeOptions.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(HomeOfficeOption obj)
        {
            var homeOfficeOption = this.AppContext.HomeOfficeOptions.Find(obj.ID);

            if (homeOfficeOption == null)
            {
                throw new NotFoundException($"HomeOfficeOption with ID: {obj.ID} not found");
            }

            // fire event
            this.OnUpdate(obj, homeOfficeOption);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(homeOfficeOption).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(homeOfficeOption);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<HomeOfficeOption>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
