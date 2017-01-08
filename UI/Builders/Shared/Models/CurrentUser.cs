using System.Collections.Generic;
using Core.Helpers.Privilege;

namespace UI.Builders.Shared.Models
{
    public class CurrentUser : ICurrentUser
    {
        /// <summary>
        /// Authentication type
        /// </summary>
        public string AuthenticationType { get; set; }
        /// <summary>
        /// Indicates if user is authenticated
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// Name of user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// ApplicationUserId
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// First name of user if available
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of user if available
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// E-mail of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indicates if user is visible to other people
        /// </summary>
        public bool EmailVisibleToOthers => string.IsNullOrEmpty(FirstName);
           

        /// <summary>
        /// Privilege level of current user
        /// </summary>
        public PrivilegeLevel Privilege { get; set; }

        /// <summary>
        /// Roles of current user
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
