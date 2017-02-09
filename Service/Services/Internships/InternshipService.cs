using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipService : BaseService<Internship>, IInternshipService
    {
        public InternshipService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override void ExtendObject(SaveEventType eventType, Internship newObj, Internship oldObj = null)
        {
            switch (eventType)
            {
                case SaveEventType.Update:
                    if (oldObj != null)
                    {
                        // set active since date if internship was not active before, but is active now
                        if (newObj.IsActive)
                        {
                            newObj.ActiveSince = !oldObj.IsActive && newObj.IsActive ? DateTime.Now : oldObj.ActiveSince;
                        }
                        else if (!newObj.IsActive)
                        {
                            // thesis is not active anymore
                            newObj.ActiveSince = DateTime.MinValue;
                        }
                    }
                    break;
                case SaveEventType.Insert:
                    // set active since date
                    newObj.ActiveSince = newObj.IsActive ? DateTime.Now : DateTime.MinValue;
                    break;
            }
        }

        public override IDbSet<Internship> GetEntitySet()
        {
            return AppContext.Internships;
        }
    }
}
