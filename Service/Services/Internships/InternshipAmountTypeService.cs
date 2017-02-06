using System.Data.Entity;
using Entity;

namespace Service.Services.Internships
{
    public class InternshipAmountTypeService :  BaseService<InternshipAmountType>, IInternshipAmountTypeService
    {

        public InternshipAmountTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<InternshipAmountType> GetEntitySet()
        {
            return AppContext.InternshipAmountTypes;
        }
    }
}
