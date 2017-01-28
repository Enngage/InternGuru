using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Service.Exceptions;

namespace Service.Services.Thesis
{
    public class ThesisService :  BaseService<Entity.Thesis>, IThesisService
    {

        public ThesisService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var thesis = AppContext.Theses.Find(id);

            if (thesis != null)
            {
                AppContext.Theses.Remove(thesis);

                // touch cache keys
                TouchDeleteKeys(thesis);

                // fire event
                OnDelete(thesis);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Entity.Thesis> GetAsync(int id)
        {
            return AppContext.Theses.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Entity.Thesis> GetAll()
        {
            return AppContext.Theses;
        }

        public IQueryable<Entity.Thesis> GetSingle(int id)
        {
            return AppContext.Theses.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Entity.Thesis obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            // set dates
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            AppContext.Theses.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Entity.Thesis obj)
        {
            var thesis = AppContext.Theses.Find(obj.ID);

            if (thesis == null)
            {
                throw new NotFoundException($"Thesis with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, thesis);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // set dates
            obj.Updated = DateTime.Now;
            obj.Created = thesis.Created;

            // update log
            AppContext.Entry(thesis).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(thesis);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entity.Thesis>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
