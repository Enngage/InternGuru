using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Thesis
{
    public class ThesisTypeService :  BaseService<ThesisType>, IThesisTypeService
    {

        public ThesisTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }


        public override Task<int> InsertAsync(ThesisType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(ThesisType obj)
        {
            var thesisType = AppContext.ThesisTypes.Find(obj.ID);

            if (thesisType == null)
            {
                throw new NotFoundException($"ThesisType with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, thesisType);
        }


        public override IDbSet<ThesisType> GetEntitySet()
        {
            return this.AppContext.ThesisTypes;
        }
    }
}
