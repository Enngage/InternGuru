
namespace Core.Models
{
    public interface IUser
    {
        bool IsAuthenticated { get; }
        string UserName { get; }
        string AuthenticationType { get; }
        string UserId { get; }
    }
}
