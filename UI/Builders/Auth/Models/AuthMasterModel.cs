using System.Collections.Generic;

namespace UI.Builders.Auth.Models
{
    public class AuthMasterModel
    {        
        public IEnumerable<AuthInternshipListingModel> Internships { get; set; }
        public IEnumerable<AuthThesisListingModel> Theses { get; set; }
        public IEnumerable<AuthConversationModel> Conversations { get; set; }

        /// <summary>
        /// Indicates if current user e-mail is visible to others
        /// This property is here as a fix for EditingUser as otherwise CurrentUser would be updated on next request
        /// Should be set when user is updated
        /// </summary>
        public bool? EmailVisibleForPeople { get; set; } = null; // set when user is updated

    }
}
