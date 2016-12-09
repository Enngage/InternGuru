using System.Collections.Generic;

namespace UI.Builders.Auth.Models
{
    public class AuthMasterModel
    {        
        public IEnumerable<AuthInternshipListingModel> Internships { get; set; }
        public IEnumerable<AuthThesisListingModel> Theses { get; set; }
        public IEnumerable<AuthConversationModel> Conversations { get; set; }
    }
}
