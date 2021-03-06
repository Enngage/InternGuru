﻿using System.Collections.Generic;
using Core.Helpers.Privilege;
using UI.Builders.Shared.Enums;
using UI.Helpers;

namespace UI.Builders.Shared.Models
{
    public class CurrentUser : ICurrentUser
    {
        #region Setup

        private const string CandidateMasterView = "~/views/auth/CandidateTypeMaster.cshtml";
        private const string CompanyMasterview = "~/views/auth/CompanyTypeMaster.cshtml";

        private const string CandidateIndexAction = "CandidateTypeIndex";
        private const string CompanyIndexAction = "CompanyTypeIndex";

        #endregion

        /// <summary>
        /// Master layout used by the page 
        /// Layout is different for candidate and company users
        /// </summary>
        public string AuthMasterLayout => IsCompany ? CompanyMasterview : CandidateMasterView;

        /// <summary>
        /// Master action used for either company or user account
        /// </summary>
        public string AuthMasterAction => IsCompany? CompanyIndexAction : CandidateIndexAction;

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
        /// Nickname of user
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Indicates if user is visible to other people
        /// </summary>
        public bool EmailVisibleToOthers => string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(Nickname);

        /// <summary>
        /// Gets display name of user
        /// </summary>
        public string UserDisplayName => UserHelper.GetDisplayNameStatic(FirstName, LastName, Nickname, UserName);

        /// <summary>
        /// Privilege level of current user
        /// </summary>
        public PrivilegeLevel Privilege { get; set; }

        /// <summary>
        /// Roles of current user
        /// </summary>
        public IEnumerable<string> Roles { get; set; }


        /// <summary>
        /// Indicates if current user is of Company type
        /// </summary>
        public bool IsCompany { get; set; }

        /// <summary>
        /// Indicates if current user is of Candidate type
        /// </summary>
        public bool IsCandidate { get; set; }

        /// <summary>
        /// User type
        /// </summary>
        public UserTypeEnum UserType
        {
            get
            {
                if (IsCandidate && !IsCompany)
                {
                    return UserTypeEnum.Candidate;
                }
                if (IsCompany && !IsCandidate)
                {
                    return UserTypeEnum.Company;
                }
                if (IsCompany && IsCandidate)
                {
                    return UserTypeEnum.CompanyAndCandidate;
                }
                if (!IsCandidate && !IsCompany)
                {
                    return UserTypeEnum.None;
                }
                return UserTypeEnum.Unknown;
            }
        }

        /// <summary>
        /// Subscribed cities
        /// </summary>
        public string SubscribedCities { get; set; }
    }
}
