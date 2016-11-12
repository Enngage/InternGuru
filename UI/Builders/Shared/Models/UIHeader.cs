

namespace UI.Builders.Shared
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public class UIHeader : IUIHeader
    {
        public UIHeaderType Type { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Sets header
        /// </summary>
        public void SetHeader(UIHeaderType type, string title)
        {
            this.Type = type;
            this.Title = title;
        }
    }
}
