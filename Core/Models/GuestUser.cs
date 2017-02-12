namespace Core.Models
{
    public class GuestUser : IUser
    {
        public bool IsAuthenticated => false;
        public string UserName => null;
        public string AuthenticationType => null;
        public string UserId => null;
    }
}
