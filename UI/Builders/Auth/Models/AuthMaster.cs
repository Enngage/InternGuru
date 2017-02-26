
using PagedList;

namespace UI.Builders.Auth.Models
{
    public class AuthMaster
    {
        public AuthCompanyMasterModel CompanyMaster { get; set; }
        public AuthCandidateMasterModel CandidateMaster { get; set; }
        public IPagedList<AuthConversationModel> ConversationsPaged { get; set; }

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

        /// <summary>
        /// Master action used for either company or user account
        /// </summary>
        public string AuthMasterAction { get; set; }
    }
}
