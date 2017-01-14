using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipService : BaseService<Internship>, IInternshipService
    {

        public InternshipService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var internship = AppContext.Internships.Find(id);

            if (internship != null)
            {
                // delete internship
                AppContext.Internships.Remove(internship);

                // touch cache keys
                TouchDeleteKeys(internship);

                // fire event
                OnDelete(internship);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Internship> GetAsync(int id)
        {
            return AppContext.Internships.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Internship> GetAll()
        {
            return AppContext.Internships;
        }

        public IQueryable<Internship> GetSingle(int id)
        {
            return AppContext.Internships.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Internship obj)
        {
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.Internships.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Internship obj)
        {
            var internship = AppContext.Internships.Find(obj.ID);

            if (internship == null)
            {
                throw new NotFoundException($"Internship with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, internship);

            obj.Updated = DateTime.Now;
            obj.Created = internship.Created;

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(internship).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(internship);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Internship>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
