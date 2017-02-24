
namespace Service.Models
{
    public class RequestContext : IRequestContext
    {
        public RequestContext(string currentUrl)
        {
            CurrentUrl = currentUrl;
        }

        public string CurrentUrl { get; }
    }
}
