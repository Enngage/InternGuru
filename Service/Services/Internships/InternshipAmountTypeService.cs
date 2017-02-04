using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipAmountTypeService :  BaseService<InternshipAmountType>, IInternshipAmountTypeService
    {

        public InternshipAmountTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(InternshipAmountType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override  Task<int> UpdateAsync(InternshipAmountType obj)
        {
            var amountType = AppContext.InternshipAmountTypes.Find(obj.ID);

            if (amountType == null)
            {
                throw new NotFoundException($"Amount type with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, amountType);
        }

        public override IDbSet<InternshipAmountType> GetEntitySet()
        {
            return this.AppContext.InternshipAmountTypes;
        }
    }
}
