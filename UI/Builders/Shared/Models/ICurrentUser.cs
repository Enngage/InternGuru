﻿using System.Collections.Generic;
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
        /// E-mail of user
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Nickname of user
        /// </summary>
        string Nickname { get; }

        /// <summary>
        /// Indicates if e-mail of user is visible to other people
        /// </summary>
        bool EmailVisibleToOthers { get; }

        /// <summary>
        /// Gets display name of user
        /// </summary>
        string UserDisplayName { get; }

        /// <summary>
        /// Privilege level
        /// </summary>
        PrivilegeLevel Privilege { get; }

        /// <summary>
        /// List of users in current role
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Indicates if current user is of Company type
        /// </summary>
        bool IsCompany { get; }

        /// <summary>
        /// Indicates if current user is of Candidate type
        /// </summary>
        bool IsCandidate { get; }
    }
}
