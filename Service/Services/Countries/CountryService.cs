using System.Data.Entity;
using Entity;

namespace Service.Services.Countries
{
    public class CountryService :  BaseService<Country>, ICountryService
    {

        public CountryService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<Country> GetEntitySet()
        {
            return AppContext.Countries;
        }
    }
}
