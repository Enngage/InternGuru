using System.Security.Claims;

namespace Core.Models
{
    public class AuthenticatedUser : IUser
    {
        public bool IsAuthenticated => true;
        public string UserName { get; }
        public string UserId { get; }
        public string AuthenticationType { get; }

        public AuthenticatedUser(ClaimsIdentity identity, string userId)
        {
            UserName = identity.Name;
            AuthenticationType = identity.AuthenticationType;
            UserId = userId;
        }
    }
}
