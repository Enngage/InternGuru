using UI.Builders.Services;

namespace UI.Events
{
    /// <summary>
    /// Use this class to register events
    /// </summary>
    public class ServiceEvents : IServiceEvents
    {

        #region Properties

        private NotificationEvents NotificationEvents { get; set; }

        private IServicesLoader Services { get; set; }

        #endregion

        #region Constructor

        public ServiceEvents(IServicesLoader servicesLoader, NotificationEvents notificationEvents)
        {
            this.Services = servicesLoader;
            this.NotificationEvents = notificationEvents;
        }

        #endregion

        #region Event registration

        /// <summary>
        /// Used to register events
        /// </summary>
        public void RegisterEvents()
        {
            // ----- Registered events ------ //
            this.Services.MessageService.OnInsertObject += MessageService_OnInsertObject;
        }

        #endregion

        #region Events

        /// <summary>
        /// Executed when new message is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageService_OnInsertObject(object sender, Service.Events.InsertEventArgs<Entity.Message> e)
        {
            NotificationEvents.SendMessageNotifications(e.Obj);
        }

        #endregion
    }
}
