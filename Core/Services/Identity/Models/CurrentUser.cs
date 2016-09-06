
namespace Core.Services.Identity.Models
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
        public string Name { get; set; }

        /// <summary>
        /// ApplicationUserId
        /// </summary>
        public string Id { get; set; }
    }
}
