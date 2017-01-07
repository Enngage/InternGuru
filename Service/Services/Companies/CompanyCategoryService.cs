using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Logs;

namespace Service.Services.Companies
{
    public class CompanyCategoryService : BaseService<CompanyCategory>, ICompanyCategoryService
    {

        public CompanyCategoryService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var category = AppContext.CompanyCategories.Find(id);

            if (category != null)
            {
                // delete category
                AppContext.CompanyCategories.Remove(category);

                // touch cache keys
                TouchDeleteKeys(category);

                // fire event
                OnDelete(category);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<CompanyCategory> GetAsync(int id)
        {
            return AppContext.CompanyCategories.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<CompanyCategory> GetAll()
        {
            return AppContext.CompanyCategories;
        }

        public IQueryable<CompanyCategory> GetSingle(int id)
        {
            return AppContext.CompanyCategories.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(CompanyCategory obj)
        {
            AppContext.CompanyCategories.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(CompanyCategory obj)
        {
            var category = AppContext.CompanyCategories.Find(obj.ID);

            if (category == null)
            {
                throw new NotFoundException($"CompanyCategory with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, category);

            // update log
            AppContext.Entry(category).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(category);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CompanyCategory>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
