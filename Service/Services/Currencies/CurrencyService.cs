using System.Data.Entity;
using Entity;

namespace Service.Services.Currencies
{
    public class CurrencyService :  BaseService<Currency>, ICurrencyService
    {

        public CurrencyService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<Currency> GetEntitySet()
        {
            return AppContext.Currencies;
        }
    }
}
