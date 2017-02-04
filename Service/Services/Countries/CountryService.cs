using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Countries
{
    public class CountryService :  BaseService<Country>, ICountryService
    {

        public CountryService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(Country obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(Country obj)
        {
            var country = AppContext.Countries.Find(obj.ID);

            if (country == null)
            {
                throw new NotFoundException($"Country with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, country);
        }

        public override IDbSet<Country> GetEntitySet()
        {
            return this.AppContext.Countries;
        }
    }
}
