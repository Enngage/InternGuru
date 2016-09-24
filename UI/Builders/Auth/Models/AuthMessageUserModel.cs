
namespace UI.Builders.Auth.Models
{
    public class AuthMessageUserModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DisplayName
        {
            get
            {
                return !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) ? $"{FirstName} {LastName}" : UserName;
            }
        }
    }
}
