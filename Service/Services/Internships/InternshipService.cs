using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipService : BaseService<Internship>, IInternshipService
    {

        public InternshipService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        //public IDbSet<Internship> DbSet => this.AppContext.Internships;

        public Task<int> DeleteAsync(int id)
        {
            var internship = AppContext.Internships.Find(id);

            if (internship != null)
            {
                // delete internship
                AppContext.Internships.Remove(internship);

                // touch cache keys
                TouchDeleteKeys(internship);

                // save changes
                return SaveChangesAsync(SaveEventType.Delete, internship);
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

            // set active since date
            obj.ActiveSince = obj.IsActive ? DateTime.Now : DateTime.MinValue;

            AppContext.Internships.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            return SaveChangesAsync(SaveEventType.Insert, obj);
        }

        public Task<int> UpdateAsync(Internship obj)
        {
            var internship = AppContext.Internships.Find(obj.ID);

            if (internship == null)
            {
                throw new NotFoundException($"Internship with ID: {obj.ID} not found");
            }

            obj.Updated = DateTime.Now;
            obj.Created = internship.Created;

            // set code name
            obj.CodeName = obj.GetCodeName();

            // set active since date if internship was not active before, but is active now
            obj.ActiveSince = !internship.IsActive && obj.IsActive ? DateTime.Now : internship.ActiveSince;

            // update
            AppContext.Entry(internship).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(internship);

            // save changes
            return SaveChangesAsync(SaveEventType.Update, obj, internship);
        }

        public async Task<IEnumerable<Internship>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
