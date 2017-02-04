using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Service.Exceptions;

namespace Service.Services.Thesis
{
    public class ThesisService :  BaseService<Entity.Thesis>, IThesisService
    {

        public ThesisService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(Entity.Thesis obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            // set dates
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            // set active since date
            obj.ActiveSince = obj.IsActive ? DateTime.Now : DateTime.MinValue;

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(Entity.Thesis obj)
        {
            var thesis = AppContext.Theses.Find(obj.ID);

            if (thesis == null)
            {
                throw new NotFoundException($"Thesis with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // set dates
            obj.Updated = DateTime.Now;
            obj.Created = thesis.Created;

            // set active since date if internship was not active before, but is active now
            obj.ActiveSince = !thesis.IsActive && obj.IsActive ? DateTime.Now : thesis.ActiveSince;

            // save changes
            return base.UpdateAsync(obj, thesis);
        }


        public override IDbSet<Entity.Thesis> GetEntitySet()
        {
            return this.AppContext.Theses;
        }
    }
}
