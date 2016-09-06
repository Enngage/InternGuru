
namespace Core.Services.Identity.Models
{
    public interface ICurrentUser
    {
        /// <summary>
        /// Authentication type
        /// </summary>
        string AuthenticationType { get; }
        /// <summary>
        /// Indicates if user is authenticated
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// Name of user
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ApplicationUserId
        /// </summary>
        string Id { get;  }
    }
}
