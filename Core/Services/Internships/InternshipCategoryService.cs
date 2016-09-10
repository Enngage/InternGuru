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
    public class InternshipCategoryService : BaseService<InternshipCategory>, IInternshipCategoryService
    {

        public InternshipCategoryService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task DeleteAsync(int id)
        {
            var category = this.AppContext.InternshipCategories.Find(id);

            if (category != null)
            {
                // delete category
                this.AppContext.InternshipCategories.Remove(category);

                // touch cache keys
                this.TouchDeleteKeys(category, category.ID.ToString());

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<InternshipCategory> GetAsync(int id)
        {
            return this.AppContext.InternshipCategories.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipCategory> GetAll()
        {
            return this.AppContext.InternshipCategories;
        }

        public IQueryable<InternshipCategory> GetSingle(int id)
        {
            return this.AppContext.InternshipCategories.Where(m => m.ID == id).Take(1);
        }

        public Task InsertAsync(InternshipCategory obj)
        {
            this.AppContext.InternshipCategories.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            return this.SaveChangesAsync();
        }

        public Task UpdateAsync(InternshipCategory obj)
        {
            var category = this.AppContext.InternshipCategories.Find(obj.ID);

            if (category == null)
            {
                throw new NotFoundException(string.Format("InternshipCategory with ID: {0} not found", category.ID));
            }

            // update log
            this.AppContext.Entry(category).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(category, category.ID.ToString());

            // save changes
            return this.AppContext.SaveChangesAsync();
        }
    }
}
