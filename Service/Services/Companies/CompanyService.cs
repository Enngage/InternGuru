using System.Data.Entity;
using Entity;

namespace Service.Services.Companies
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }


        public override IDbSet<Company> GetEntitySet()
        {
            return AppContext.Companies;
        }

    }
}
