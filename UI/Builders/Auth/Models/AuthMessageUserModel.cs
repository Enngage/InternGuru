
using UI.Helpers;

namespace UI.Builders.Auth.Models
{
    public class AuthMessageUserModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        public string DisplayName => UserHelper.GetDisplayNameStatic(FirstName, LastName, Nickname, UserName);
    }
}
