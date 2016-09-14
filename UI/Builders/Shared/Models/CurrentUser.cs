
namespace UI.Builders.Shared
{
    public class CurrentUser : ICurrentUser
    {
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
    }
}
