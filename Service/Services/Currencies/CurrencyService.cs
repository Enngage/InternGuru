using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Currencies
{
    public class CurrencyService :  BaseService<Currency>, ICurrencyService
    {

        public CurrencyService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(Currency obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(Currency obj)
        {
            var currency = AppContext.Currencies.Find(obj.ID);

            if (currency == null)
            {
                throw new NotFoundException($"Currency with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, currency);
        }

        public override IDbSet<Currency> GetEntitySet()
        {
            return this.AppContext.Currencies;
        }
    }
}
