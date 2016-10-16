﻿using System;
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
    public class InternshipService : BaseService<Internship>, IInternshipService
    {

        public InternshipService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var internship = this.AppContext.Internships.Find(id);

            if (internship != null)
            {
                // delete internship
                this.AppContext.Internships.Remove(internship);

                // touch cache keys
                this.TouchDeleteKeys(internship);

                // fire event
                this.OnDelete(internship);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Internship> GetAsync(int id)
        {
            return this.AppContext.Internships.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Internship> GetAll()
        {
            return this.AppContext.Internships;
        }

        public IQueryable<Internship> GetSingle(int id)
        {
            return this.AppContext.Internships.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Internship obj)
        {
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.Internships.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Internship obj)
        {
            var internship = this.AppContext.Internships.Find(obj.ID);

            if (internship == null)
            {
                throw new NotFoundException(string.Format("Internship with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, internship);

            obj.Updated = DateTime.Now;
            obj.Created = internship.Created;

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(internship).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(internship);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Internship>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
