using UI.Builders.Services;

namespace UI.Events
{
    public class ServiceEvents
    {
        private IServicesLoader services;
        public ServiceEvents(IServicesLoader servicesLoader)
        {
            this.services = servicesLoader;
        }

        /// <summary>
        /// Used to register events
        /// </summary>
        public void RegisterEvents()
        {
            this.services.MessageService.OnInsertObject += MessageService_OnInsertObject;
        }

        private void MessageService_OnInsertObject(object sender, Core.Events.InsertEventArgs<Entity.Message> e)
        {

        }
    }
}
