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
    public class StudentStatusOptionService :  BaseService<StudentStatusOption>, IStudentStatusOptionService
    {

        public StudentStatusOptionService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var studentStatusOption = this.AppContext.StudentStatusOptions.Find(id);

            if (studentStatusOption != null)
            {
                this.AppContext.StudentStatusOptions.Remove(studentStatusOption);

                // touch cache keys
                this.TouchDeleteKeys(studentStatusOption);

                // fire event
                this.OnDelete(studentStatusOption);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<StudentStatusOption> GetAsync(int id)
        {
            return this.AppContext.StudentStatusOptions.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<StudentStatusOption> GetAll()
        {
            return this.AppContext.StudentStatusOptions;
        }

        public IQueryable<StudentStatusOption> GetSingle(int id)
        {
            return this.AppContext.StudentStatusOptions.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(StudentStatusOption obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.StudentStatusOptions.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(StudentStatusOption obj)
        {
            var studentStatusOption = this.AppContext.StudentStatusOptions.Find(obj.ID);

            if (studentStatusOption == null)
            {
                throw new NotFoundException($"StudentStatusOption with ID: {obj.ID} not found");
            }

            // fire event
            this.OnUpdate(obj, studentStatusOption);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(studentStatusOption).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(studentStatusOption);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentStatusOption>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }
    }
}
