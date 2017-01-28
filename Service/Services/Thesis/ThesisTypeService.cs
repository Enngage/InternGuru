using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Thesis
{
    public class ThesisTypeService :  BaseService<ThesisType>, IThesisTypeService
    {

        public ThesisTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var thesisType = AppContext.ThesisTypes.Find(id);

            if (thesisType != null)
            {
                AppContext.ThesisTypes.Remove(thesisType);

                // touch cache keys
                TouchDeleteKeys(thesisType);

                // fire event
                OnDelete(thesisType);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<ThesisType> GetAsync(int id)
        {
            return AppContext.ThesisTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<ThesisType> GetAll()
        {
            return AppContext.ThesisTypes;
        }

        public IQueryable<ThesisType> GetSingle(int id)
        {
            return AppContext.ThesisTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(ThesisType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.ThesisTypes.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(ThesisType obj)
        {
            var thesisType = AppContext.ThesisTypes.Find(obj.ID);

            if (thesisType == null)
            {
                throw new NotFoundException($"ThesisType with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, thesisType);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(thesisType).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(thesisType);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ThesisType>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
