using System.Data.Entity;
using Entity;

namespace Service.Services.Internships
{
    public class StudentStatusOptionService :  BaseService<StudentStatusOption>, IStudentStatusOptionService
    {

        public StudentStatusOptionService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }


        public override IDbSet<StudentStatusOption> GetEntitySet()
        {
            return AppContext.StudentStatusOptions;
        }
    }
}
