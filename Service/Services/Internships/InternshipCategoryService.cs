using System.Data.Entity;
using Entity;

namespace Service.Services.Internships
{
    public class InternshipCategoryService : BaseService<InternshipCategory>, IInternshipCategoryService
    {

        public InternshipCategoryService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<InternshipCategory> GetEntitySet()
        {
            return this.AppContext.InternshipCategories;
        }
    }
}
