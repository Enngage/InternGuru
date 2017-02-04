using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class StudentStatusOptionService :  BaseService<StudentStatusOption>, IStudentStatusOptionService
    {

        public StudentStatusOptionService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(StudentStatusOption obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(StudentStatusOption obj)
        {
            var studentStatusOption = AppContext.StudentStatusOptions.Find(obj.ID);

            if (studentStatusOption == null)
            {
                throw new NotFoundException($"StudentStatusOption with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, studentStatusOption);
        }


        public override IDbSet<StudentStatusOption> GetEntitySet()
        {
            return this.AppContext.StudentStatusOptions;
        }
    }
}
