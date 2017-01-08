using System.Collections.Generic;
using UI.Builders.Auth.Enum;
using System.Linq;

namespace UI.Builders.Auth.Models
{
    public class AuthTabs
    {
        public AuthTab MainMenu { get; set; } = new AuthTab()
        {
            Display = false,
            Priority = 1,
            Tab = AuthTabEnum.MainMenu
        };

        public AuthTab MyInternships { get; set; } = new AuthTab()
        {
            Display = false,
            Priority = 2,
            Tab = AuthTabEnum.MyInternships
        };

        public AuthTab MyTheses { get; set; } = new AuthTab()
        {
            Display = false,
            Priority = 2,
            Tab = AuthTabEnum.MyTheses
        };

        public AuthTab RecentConversations { get; set; } = new AuthTab()
        {
            Display = false,
            Priority = 2,
            Tab = AuthTabEnum.RecentConversations
        };

        public AuthTab Admin { get; set; } = new AuthTab()
        {
            Display = false,
            Priority = 3,
            Tab = AuthTabEnum.Admin
        };



        /// <summary>
        /// Gets all tabs, tabs need to be added here manually
        /// </summary>
        /// <returns>Collection of tabs</returns>
        public IEnumerable<AuthTab> GetTabs()
        {
            return new List<AuthTab>()
            {
                MainMenu,
                MyInternships,
                MyTheses,
                RecentConversations,
                Admin,
            };
        }

        /// <summary>
        /// Gets active tabs ordered by priority
        /// </summary>
        /// <returns>Active tabs ordered by priority</returns>
        public IEnumerable<AuthTab> GetOrderedActiveTabs()
        {
            return GetTabs().Where(m => m.Display).OrderBy(m => m.Priority);
        }

    }
}
