
using UI.Builders.Auth.Enum;

namespace UI.Builders.Auth.Models
{
    public class AuthTab
    {

        public AuthTab() { }
        public AuthTab(AuthTabEnum tab, bool display, int priority)
        {
            Tab = tab;
            Display = display;
            Priority = priority;
        }

        /// <summary>
        /// Tab to be displayed
        /// </summary>
        public AuthTabEnum Tab { get; set; }

        /// <summary>
        /// Indicates if tab is displayed
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Priority - the lower the number, the higher priority
        /// </summary>
        public int Priority { get; set; }

        public void Set(bool display, int priority)
        {
            Display = display;
            Priority = priority;
        }
    }
}
