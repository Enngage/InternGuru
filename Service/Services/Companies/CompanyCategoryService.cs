using System.Data.Entity;
using Entity;

namespace Service.Services.Companies
{
    public class CompanyCategoryService : BaseService<CompanyCategory>, ICompanyCategoryService
    {

        public CompanyCategoryService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<CompanyCategory> GetEntitySet()
        {
            return this.AppContext.CompanyCategories;
        }
    }
}
