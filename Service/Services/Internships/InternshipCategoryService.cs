using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipCategoryService : BaseService<InternshipCategory>, IInternshipCategoryService
    {

        public InternshipCategoryService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var category = AppContext.InternshipCategories.Find(id);

            if (category != null)
            {
                // delete category
                AppContext.InternshipCategories.Remove(category);

                // touch cache keys
                TouchDeleteKeys(category);

                // fire event
                OnDelete(category);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<InternshipCategory> GetAsync(int id)
        {
            return AppContext.InternshipCategories.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipCategory> GetAll()
        {
            return AppContext.InternshipCategories;
        }

        public IQueryable<InternshipCategory> GetSingle(int id)
        {
            return AppContext.InternshipCategories.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(InternshipCategory obj)
        {
            AppContext.InternshipCategories.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(InternshipCategory obj)
        {
            var category = AppContext.InternshipCategories.Find(obj.ID);

            if (category == null)
            {
                throw new NotFoundException($"InternshipCategory with ID: {obj.ID} not found");
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

        public async Task<IEnumerable<InternshipCategory>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
