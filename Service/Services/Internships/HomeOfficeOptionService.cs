using System.Data.Entity;
using Entity;

namespace Service.Services.Internships
{
    public class HomeOfficeOptionService :  BaseService<HomeOfficeOption>, IHomeOfficeOptionService
    {

        public HomeOfficeOptionService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }


        public override IDbSet<HomeOfficeOption> GetEntitySet()
        {
            return AppContext.HomeOfficeOptions;
        }
    }
}
