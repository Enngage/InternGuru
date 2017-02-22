﻿using System.Collections.Generic;

namespace UI.Builders.Auth.Models
{
    public class AuthMasterModel
    {
        public IEnumerable<AuthInternshipListingModel> Internships { get; set; }
        public IEnumerable<AuthThesisListingModel> Theses { get; set; }
        public IEnumerable<AuthConversationModel> Conversations { get; set; }
        public IEnumerable<AuthQuestionnaireListingModel> Questionnaires { get; set; }

        /// <summary>
        /// Indicates if current user e-mail is visible to others
        /// This property is here as a fix for EditingUser as otherwise CurrentUser would be updated on next request
        /// Should be set when user is updated
        /// </summary>
        public bool? EmailVisibleForPeople { get; set; } = null; // set when user is updated

        /// <summary>
        /// Indicates whether to show user type selection or not
        /// </summary>
        public bool ShowUserTypeSelectionView { get; set; }


        /// <summary>
        /// Master layout used by the page 
        /// Layout is different for candidate and company users
        /// </summary>
        public string AuthMasterLayout { get; set; }

    }
}
