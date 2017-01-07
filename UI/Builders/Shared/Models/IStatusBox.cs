
namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Represent status of current user
    /// </summary>
    public interface IStatusBox
    {
         int NewMessagesCount { get; }
         int NewEventLogCount { get; }
    }
}
