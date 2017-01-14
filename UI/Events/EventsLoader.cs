using UI.Events.EventClasses;

namespace UI.Events
{
    public class EventsLoader : IEventsLoader
    {
        public NotificationEvents NotificationEvents { get; }

        public EventsLoader(
            NotificationEvents notificationEvents
            )
        {
            NotificationEvents = notificationEvents;
        }
     
    }
}
