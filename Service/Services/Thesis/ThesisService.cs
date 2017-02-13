using System.Data.Entity;

namespace Service.Services.Thesis
{
    public class ThesisService :  BaseService<Entity.Thesis>, IThesisService
    {

        public ThesisService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<Entity.Thesis> GetEntitySet()
        {
            return AppContext.Theses;
        }
    }
}
