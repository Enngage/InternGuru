using UI.Events.EventClasses;

namespace UI.Events
{
    public class EventsLoader : IEventsLoader
    {
        public NotificationEvents NotificationEvents { get; }
        public InternshipSubscriptionEvents InternshipSubscriptionEvents { get; }

        public EventsLoader(
            NotificationEvents notificationEvents,
            InternshipSubscriptionEvents internshipSubscriptionEvents
            )
        {
            NotificationEvents = notificationEvents;
            InternshipSubscriptionEvents = internshipSubscriptionEvents;
        }

    }
}
