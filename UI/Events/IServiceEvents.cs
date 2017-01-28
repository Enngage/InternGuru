
using UI.Builders.Shared.Models;

namespace UI.Events
{
    public interface IServiceEvents
    {
        /// <summary>
        /// Method where events should be registered
        /// </summary>
        /// <param name="currentUser">Instance of current user in order to give service classes context</param>
        /// <param name="url">URL of current request</param>
        void RegisterEvents(ICurrentUser currentUser, string url);

    }
}
