using System.Data.Entity;

namespace Service.Services.Education
{
    public class EducationTypeTypeService :  BaseService<Entity.EducationType>, IEducationTypeService
    {

        public EducationTypeTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override IDbSet<Entity.EducationType> GetEntitySet()
        {
            return AppContext.EducationTypes;
        }
    }
}
