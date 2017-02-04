using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class StudentStatusOptionService :  BaseService<StudentStatusOption>, IStudentStatusOptionService
    {

        public StudentStatusOptionService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var studentStatusOption = AppContext.StudentStatusOptions.Find(id);

            if (studentStatusOption != null)
            {
                AppContext.StudentStatusOptions.Remove(studentStatusOption);

                // touch cache keys
                TouchDeleteKeys(studentStatusOption);

                // save changes
                return SaveChangesAsync(SaveEventType.Delete, studentStatusOption);
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

            return SaveChangesAsync(SaveEventType.Insert, obj);
        }

        public Task<int> UpdateAsync(StudentStatusOption obj)
        {
            var studentStatusOption = AppContext.StudentStatusOptions.Find(obj.ID);

            if (studentStatusOption == null)
            {
                throw new NotFoundException($"StudentStatusOption with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(studentStatusOption).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(studentStatusOption);

            // save changes
            return SaveChangesAsync(SaveEventType.Update, obj, studentStatusOption);
        }

        public async Task<IEnumerable<StudentStatusOption>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }
    }
}
