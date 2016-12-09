using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Service.Context;
using Entity;
using Cache;
using Service.Exceptions;
using System.Collections.Generic;

namespace Service.Services
{
    public class ThesisTypeService :  BaseService<ThesisType>, IThesisTypeService
    {

        public ThesisTypeService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var thesisType = this.AppContext.ThesisTypes.Find(id);

            if (thesisType != null)
            {
                this.AppContext.ThesisTypes.Remove(thesisType);

                // touch cache keys
                this.TouchDeleteKeys(thesisType);

                // fire event
                this.OnDelete(thesisType);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<ThesisType> GetAsync(int id)
        {
            return this.AppContext.ThesisTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<ThesisType> GetAll()
        {
            return this.AppContext.ThesisTypes;
        }

        public IQueryable<ThesisType> GetSingle(int id)
        {
            return this.AppContext.ThesisTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(ThesisType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.ThesisTypes.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(ThesisType obj)
        {
            var thesisType = this.AppContext.ThesisTypes.Find(obj.ID);

            if (thesisType == null)
            {
                throw new NotFoundException(string.Format("ThesisType with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, thesisType);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(thesisType).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(thesisType);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ThesisType>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
