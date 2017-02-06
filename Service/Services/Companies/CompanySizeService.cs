using System.Data.Entity;
using Entity;

namespace Service.Services.Companies
{
    public class CompanySizeService :  BaseService<CompanySize>, ICompanySizeService
    {

        public CompanySizeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<CompanySize> GetEntitySet()
        {
            return AppContext.CompanySizes;
        }
    }
}
