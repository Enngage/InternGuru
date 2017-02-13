using System.Data.Entity;
using Entity;

namespace Service.Services.Internships
{
    public class InternshipService : BaseService<Internship>, IInternshipService
    {
        public InternshipService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<Internship> GetEntitySet()
        {
            return AppContext.Internships;
        }
    }
}
