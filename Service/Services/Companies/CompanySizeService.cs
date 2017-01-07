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
    public class CompanySizeService :  BaseService<CompanySize>, ICompanySizeService
    {

        public CompanySizeService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var companySize = AppContext.CompanySizes.Find(id);

            if (companySize != null)
            {
                // delete companySize
                AppContext.CompanySizes.Remove(companySize);

                // touch cache keys
                TouchDeleteKeys(companySize);

                // fire event
                OnDelete(companySize);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<CompanySize> GetAsync(int id)
        {
            return AppContext.CompanySizes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<CompanySize> GetAll()
        {
            return AppContext.CompanySizes;
        }

        public IQueryable<CompanySize> GetSingle(int id)
        {
            return AppContext.CompanySizes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(CompanySize obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.CompanySizes.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(CompanySize obj)
        {
            var companySize = AppContext.CompanySizes.Find(obj.ID);

            if (companySize == null)
            {
                throw new NotFoundException($"CompanySize with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, companySize);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(companySize).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(companySize);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CompanySize>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
