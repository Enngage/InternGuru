using System.Data.Entity;
using Entity;

namespace Service.Services.Thesis
{
    public class ThesisTypeService :  BaseService<ThesisType>, IThesisTypeService
    {

        public ThesisTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<ThesisType> GetEntitySet()
        {
            return AppContext.ThesisTypes;
        }
    }
}
