

using UI.Builders.Shared.Enums;

namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public interface IUiHeader
    {
        UiHeaderType Type { get; }
        string Title { get; }
        void SetHeader(UiHeaderType type, string title);
    }
}
