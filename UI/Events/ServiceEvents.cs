using System;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Events
{
    /// <summary>
    /// Use this class to register events
    /// </summary>
    public class ServiceEvents : IServiceEvents
    {

        #region Properties

        private IEventsLoader EventsLoader { get; }

        private IServicesLoader Services { get; }

        private ICurrentUser CurrentUser { get; set; }
        private string CurrentUrl { get; set; }

        #endregion

        #region Constructor

        public ServiceEvents(IServicesLoader servicesLoader, IEventsLoader eventsLoader)
        {
            Services = servicesLoader;
            EventsLoader = eventsLoader;
        }

        #endregion

        #region Event registration

        /// <summary>
        /// Use to register events
        /// This methods needs to initialize CurrentUser & CurrentUrl properties
        /// </summary>
        /// <param name="currentUser">>Instance of current user in order to give service classes context</param>
        /// <param name="url">Url of current request</param>
        /// 
        public void RegisterEvents(ICurrentUser currentUser, string url)
        {
            CurrentUser = currentUser;
            CurrentUrl = url;

            Services.MessageService.OnInsertAfterObject += MessageService_OnInsertObject;

            Services.InternshipService.OnUpdateAfterObject += InternshipService_OnUpdateObject;
            Services.InternshipService.OnInsertAfterObject += InternshipService_OnInsertObject;
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
            try
            {
                EventsLoader.NotificationEvents.SendMessageNotificationToRecipient(e.Obj);
            }
            catch (Exception ex)
            {
                Services.LogService.LogException(ex, CurrentUrl, CurrentUser?.UserName);
            }
        }

        private void InternshipService_OnInsertObject(object sender, Service.Events.InsertEventArgs<Entity.Internship> e)
        {
            try
            {
                EventsLoader.NotificationEvents.SendInternshipActiveNotification(e.Obj);
            }
            catch (Exception ex)
            {
                Services.LogService.LogException(ex, CurrentUrl, CurrentUser?.UserName);
            }
        }

        private void InternshipService_OnUpdateObject(object sender, Service.Events.UpdateEventArgs<Entity.Internship> e)
        {
            try
            {
                EventsLoader.NotificationEvents.SendInternshipActiveNotification(e.Obj, e.OriginalObj);
            }
            catch (Exception ex)
            {
                Services.LogService.LogException(ex, CurrentUrl, CurrentUser?.UserName);
            }
        }

        #endregion
    }
}
