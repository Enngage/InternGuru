using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class HomeOfficeOptionService :  BaseService<HomeOfficeOption>, IHomeOfficeOptionService
    {

        public HomeOfficeOptionService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var homeOfficeOption = AppContext.HomeOfficeOptions.Find(id);

            if (homeOfficeOption != null)
            {
                AppContext.HomeOfficeOptions.Remove(homeOfficeOption);

                // touch cache keys
                TouchDeleteKeys(homeOfficeOption);

                // fire event
                OnDelete(homeOfficeOption);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<HomeOfficeOption> GetAsync(int id)
        {
            return AppContext.HomeOfficeOptions.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<HomeOfficeOption> GetAll()
        {
            return AppContext.HomeOfficeOptions;
        }

        public IQueryable<HomeOfficeOption> GetSingle(int id)
        {
            return AppContext.HomeOfficeOptions.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(HomeOfficeOption obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.HomeOfficeOptions.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(HomeOfficeOption obj)
        {
            var homeOfficeOption = AppContext.HomeOfficeOptions.Find(obj.ID);

            if (homeOfficeOption == null)
            {
                throw new NotFoundException($"HomeOfficeOption with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, homeOfficeOption);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(homeOfficeOption).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(homeOfficeOption);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<HomeOfficeOption>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
