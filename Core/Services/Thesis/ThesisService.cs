using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Entity;

using Core.Context;
using Cache;
using Core.Exceptions;

namespace Core.Services
{
    public class ThesisService :  BaseService<Thesis>, IThesisService
    {

        public ThesisService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var thesis = this.AppContext.Theses.Find(id);

            if (thesis != null)
            {
                this.AppContext.Theses.Remove(thesis);

                // touch cache keys
                this.TouchDeleteKeys(thesis);

                // fire event
                this.OnDelete(thesis);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Thesis> GetAsync(int id)
        {
            return this.AppContext.Theses.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Thesis> GetAll()
        {
            return this.AppContext.Theses;
        }

        public IQueryable<Thesis> GetSingle(int id)
        {
            return this.AppContext.Theses.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Thesis obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            // set dates
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            this.AppContext.Theses.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Thesis obj)
        {
            var thesis = this.AppContext.Theses.Find(obj.ID);

            if (thesis == null)
            {
                throw new NotFoundException(string.Format("Thesis with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, thesis);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // set dates
            obj.Updated = DateTime.Now;
            obj.Created = thesis.Created;

            // update log
            this.AppContext.Entry(thesis).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(thesis);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Thesis>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
