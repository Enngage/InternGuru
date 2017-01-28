
namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Represent status of current user
    /// </summary>
    public class StatusBox : IStatusBox
    {
         public int NewMessagesCount { get; set; }
         public int NewEventLogCount { get; set; }
    }
}
