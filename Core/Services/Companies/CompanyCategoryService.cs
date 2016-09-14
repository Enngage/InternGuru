using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Entity;
using Cache;
using Core.Exceptions;

namespace Core.Services
{
    public class CompanyCategoryService : BaseService<CompanyCategory>, ICompanyCategoryService
    {

        public CompanyCategoryService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task DeleteAsync(int id)
        {
            var category = this.AppContext.CompanyCategories.Find(id);

            if (category != null)
            {
                // delete category
                this.AppContext.CompanyCategories.Remove(category);

                // touch cache keys
                this.TouchDeleteKeys(category);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<CompanyCategory> GetAsync(int id)
        {
            return this.AppContext.CompanyCategories.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<CompanyCategory> GetAll()
        {
            return this.AppContext.CompanyCategories;
        }

        public IQueryable<CompanyCategory> GetSingle(int id)
        {
            return this.AppContext.CompanyCategories.Where(m => m.ID == id).Take(1);
        }

        public Task InsertAsync(CompanyCategory obj)
        {
            this.AppContext.CompanyCategories.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            return this.SaveChangesAsync();
        }

        public Task UpdateAsync(CompanyCategory obj)
        {
            var category = this.AppContext.CompanyCategories.Find(obj.ID);

            if (category == null)
            {
                throw new NotFoundException(string.Format("CompanyCategory with ID: {0} not found", category.ID));
            }

            // update log
            this.AppContext.Entry(category).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(category);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }
    }
}
