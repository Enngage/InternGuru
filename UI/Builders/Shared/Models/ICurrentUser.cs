using System.Collections.Generic;
using Core.Helpers.Privilege;

namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Represent current user
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Authentication type
        /// </summary>
        string AuthenticationType { get; }

        /// <summary>
        /// Indicates if user is authenticated
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// User name
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// ApplicationUserId
        /// </summary>
        string Id { get;  }
        
        /// <summary>
        /// First name of user if available
        /// </summary>
        string FirstName { get;}

        /// <summary>
        /// Last name of user if available
        /// </summary>
        string LastName { get; }
        
        /// <summary>
        /// Privilege level
        /// </summary>
        PrivilegeLevel Privilege { get; }

        /// <summary>
        /// List of users in current role
        /// </summary>
        IEnumerable<string> Roles { get; }
    }
}
