

namespace UI.Builders.Shared
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public interface IUIHeader
    {
        UIHeaderType Type { get; }
        string Title { get; }
        void SetHeader(UIHeaderType type, string title);
    }
}
