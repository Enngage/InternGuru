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
    public class InternshipDurationTypeService :  BaseService<InternshipDurationType>, IInternshipDurationTypeService
    {

        public InternshipDurationTypeService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var durationType = this.AppContext.InternshipDurationTypes.Find(id);

            if (durationType != null)
            {
                // delete durationType
                this.AppContext.InternshipDurationTypes.Remove(durationType);

                // touch cache keys
                this.TouchDeleteKeys(durationType);

                // fire event
                this.OnDelete(durationType);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<InternshipDurationType> GetAsync(int id)
        {
            return this.AppContext.InternshipDurationTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipDurationType> GetAll()
        {
            return this.AppContext.InternshipDurationTypes;
        }

        public IQueryable<InternshipDurationType> GetSingle(int id)
        {
            return this.AppContext.InternshipDurationTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(InternshipDurationType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.InternshipDurationTypes.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(InternshipDurationType obj)
        {
            var durationType = this.AppContext.InternshipDurationTypes.Find(obj.ID);

            if (durationType == null)
            {
                throw new NotFoundException(string.Format("InternshipDurationType with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, durationType);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(durationType).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(durationType);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InternshipDurationType>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
