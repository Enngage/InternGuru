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
    public class CompanySizeService :  BaseService<CompanySize>, ICompanySizeService
    {

        public CompanySizeService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var companySize = this.AppContext.CompanySizes.Find(id);

            if (companySize != null)
            {
                // delete companySize
                this.AppContext.CompanySizes.Remove(companySize);

                // touch cache keys
                this.TouchDeleteKeys(companySize);

                // fire event
                this.OnDelete(companySize);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<CompanySize> GetAsync(int id)
        {
            return this.AppContext.CompanySizes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<CompanySize> GetAll()
        {
            return this.AppContext.CompanySizes;
        }

        public IQueryable<CompanySize> GetSingle(int id)
        {
            return this.AppContext.CompanySizes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(CompanySize obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.CompanySizes.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(CompanySize obj)
        {
            var companySize = this.AppContext.CompanySizes.Find(obj.ID);

            if (companySize == null)
            {
                throw new NotFoundException(string.Format("CompanySize with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, companySize);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(companySize).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(companySize);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CompanySize>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
