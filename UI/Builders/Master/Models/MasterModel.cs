
namespace UI.Builders.Master.Models
{
    public class MasterModel
    {
        public string AuthenticatedUserId { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
        public string AuthenticatedUserName { get; set; }
    }
}
