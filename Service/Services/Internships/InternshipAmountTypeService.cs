using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipAmountTypeService :  BaseService<InternshipAmountType>, IInternshipAmountTypeService
    {

        public InternshipAmountTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var amountType = AppContext.InternshipAmountTypes.Find(id);

            if (amountType != null)
            {
                // delete amountType
                AppContext.InternshipAmountTypes.Remove(amountType);

                // touch cache keys
                TouchDeleteKeys(amountType);

                // fire event
                OnDelete(amountType);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<InternshipAmountType> GetAsync(int id)
        {
            return AppContext.InternshipAmountTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipAmountType> GetAll()
        {
            return AppContext.InternshipAmountTypes;
        }

        public IQueryable<InternshipAmountType> GetSingle(int id)
        {
            return AppContext.InternshipAmountTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(InternshipAmountType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.InternshipAmountTypes.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(InternshipAmountType obj)
        {
            var amountType = AppContext.InternshipAmountTypes.Find(obj.ID);

            if (amountType == null)
            {
                throw new NotFoundException($"Amount type with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, amountType);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(amountType).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(amountType);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InternshipAmountType>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
