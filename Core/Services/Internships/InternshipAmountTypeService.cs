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
    public class InternshipAmountTypeService :  BaseService<InternshipAmountType>, IInternshipAmountTypeService
    {

        public InternshipAmountTypeService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var amountType = this.AppContext.InternshipAmountTypes.Find(id);

            if (amountType != null)
            {
                // delete amountType
                this.AppContext.InternshipAmountTypes.Remove(amountType);

                // touch cache keys
                this.TouchDeleteKeys(amountType);

                // fire event
                this.OnDelete(amountType);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<InternshipAmountType> GetAsync(int id)
        {
            return this.AppContext.InternshipAmountTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipAmountType> GetAll()
        {
            return this.AppContext.InternshipAmountTypes;
        }

        public IQueryable<InternshipAmountType> GetSingle(int id)
        {
            return this.AppContext.InternshipAmountTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(InternshipAmountType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.InternshipAmountTypes.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(InternshipAmountType obj)
        {
            var amountType = this.AppContext.InternshipAmountTypes.Find(obj.ID);

            if (amountType == null)
            {
                throw new NotFoundException(string.Format("Amount type with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, amountType);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(amountType).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(amountType);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InternshipAmountType>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
