using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class HomeOfficeOptionService :  BaseService<HomeOfficeOption>, IHomeOfficeOptionService
    {

        public HomeOfficeOptionService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(HomeOfficeOption obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(HomeOfficeOption obj)
        {
            var homeOfficeOption = AppContext.HomeOfficeOptions.Find(obj.ID);

            if (homeOfficeOption == null)
            {
                throw new NotFoundException($"HomeOfficeOption with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, homeOfficeOption);
        }


        public override IDbSet<HomeOfficeOption> GetEntitySet()
        {
            return this.AppContext.HomeOfficeOptions;
        }
    }
}
