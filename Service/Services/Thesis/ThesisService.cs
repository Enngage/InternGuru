using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Thesis
{
    public class ThesisService :  BaseService<Entity.Thesis>, IThesisService
    {

        public ThesisService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override void ExtendObject(SaveEventType eventType, Entity.Thesis newObj, Entity.Thesis oldObj = null)
        {
            switch (eventType)
            {
                case SaveEventType.Update:
                    if (oldObj != null)
                    {
                        // set active since date if thesis was not active before, but is active now
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

        public override IDbSet<Entity.Thesis> GetEntitySet()
        {
            return AppContext.Theses;
        }
    }
}
