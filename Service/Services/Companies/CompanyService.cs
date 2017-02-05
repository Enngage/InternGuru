using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Companies
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }


        public override IDbSet<Company> GetEntitySet()
        {
            return EntitySet;
        }

        public IDbSet<Company> EntitySet => this.AppContext.Companies;

    }
}
