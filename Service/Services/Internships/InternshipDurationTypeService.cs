using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Logs;

namespace Service.Services.Internships
{
    public class InternshipDurationTypeService :  BaseService<InternshipDurationType>, IInternshipDurationTypeService
    {

        public InternshipDurationTypeService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var durationType = AppContext.InternshipDurationTypes.Find(id);

            if (durationType != null)
            {
                // delete durationType
                AppContext.InternshipDurationTypes.Remove(durationType);

                // touch cache keys
                TouchDeleteKeys(durationType);

                // fire event
                OnDelete(durationType);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<InternshipDurationType> GetAsync(int id)
        {
            return AppContext.InternshipDurationTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipDurationType> GetAll()
        {
            return AppContext.InternshipDurationTypes;
        }

        public IQueryable<InternshipDurationType> GetSingle(int id)
        {
            return AppContext.InternshipDurationTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(InternshipDurationType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.InternshipDurationTypes.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(InternshipDurationType obj)
        {
            var durationType = AppContext.InternshipDurationTypes.Find(obj.ID);

            if (durationType == null)
            {
                throw new NotFoundException($"InternshipDurationType with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, durationType);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(durationType).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(durationType);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InternshipDurationType>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
