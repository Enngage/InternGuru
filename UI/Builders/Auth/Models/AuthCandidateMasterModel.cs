
using System.Collections.Generic;
using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Models
{
    public class AuthCandidateMasterModel : AuthMaster
    {
        public List<string> AvailableCities { get; set; }
        public AuthCitiesSubscriptionForm CitiesSubscriptionForm { get; set; }
    }
}
