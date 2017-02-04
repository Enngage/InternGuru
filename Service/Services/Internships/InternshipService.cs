using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipService : BaseService<Internship>, IInternshipService
    {
        public InternshipService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(Internship obj)
        {
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            // set code name
            obj.CodeName = obj.GetCodeName();

            // set active since date
            obj.ActiveSince = obj.IsActive ? DateTime.Now : DateTime.MinValue;

           return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(Internship obj)
        {
            var internship = EntityDbSet.Find(obj.ID);

            if (internship == null)
            {
                throw new NotFoundException($"Internship with ID: {obj.ID} not found");
            }

            obj.Updated = DateTime.Now;
            obj.Created = internship.Created;

            // set code name
            obj.CodeName = obj.GetCodeName();

            // set active since date if internship was not active before, but is active now
            obj.ActiveSince = !internship.IsActive && obj.IsActive ? DateTime.Now : internship.ActiveSince;

            // save changes
            return base.UpdateAsync(obj, internship);
        }

        public override IDbSet<Internship> GetEntitySet()
        {
            return this.AppContext.Internships;
        }
    }
}
