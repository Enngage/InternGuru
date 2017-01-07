
using System;

namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Represent company of current user
    /// </summary>
    public interface ICurrentCompany
    {
        /// <summary>
        /// Company name
        /// </summary>
        string CompanyName { get; }

        /// <summary>
        /// Indicates if current user is assigned to a company
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// ID of company
        /// </summary>
        int CompanyID { get; }

        /// <summary>
        /// ID of user who created company
        /// </summary>
        string CompanyCreatedByApplicationUserId { get; }

        /// <summary>
        /// Name of user who created company
        /// </summary>
        string CompanyCreatedByApplicationUserName { get; }

        /// <summary>
        /// GUID of company
        /// </summary>
        Guid CompanyGuid { get; }
    }
}
