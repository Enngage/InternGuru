
namespace UI.Modules.InfoMessage.Models
{
    public class InfoMessageProcessClosableMessageQuery
    {
        public string MessageID { get; set; }
        public int ClosedForDaysCount { get; set; }
        public bool RememberClosed { get; set; }
    }
}
