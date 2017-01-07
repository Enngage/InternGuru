using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Logs;

namespace Service.Services.Internships
{
    public class StudentStatusOptionService :  BaseService<StudentStatusOption>, IStudentStatusOptionService
    {

        public StudentStatusOptionService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var studentStatusOption = AppContext.StudentStatusOptions.Find(id);

            if (studentStatusOption != null)
            {
                AppContext.StudentStatusOptions.Remove(studentStatusOption);

                // touch cache keys
                TouchDeleteKeys(studentStatusOption);

                // fire event
                OnDelete(studentStatusOption);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<StudentStatusOption> GetAsync(int id)
        {
            return AppContext.StudentStatusOptions.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<StudentStatusOption> GetAll()
        {
            return AppContext.StudentStatusOptions;
        }

        public IQueryable<StudentStatusOption> GetSingle(int id)
        {
            return AppContext.StudentStatusOptions.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(StudentStatusOption obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.StudentStatusOptions.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(StudentStatusOption obj)
        {
            var studentStatusOption = AppContext.StudentStatusOptions.Find(obj.ID);

            if (studentStatusOption == null)
            {
                throw new NotFoundException($"StudentStatusOption with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, studentStatusOption);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(studentStatusOption).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(studentStatusOption);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentStatusOption>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
