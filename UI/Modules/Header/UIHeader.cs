using UI.Builders.Shared.Enums;
using UI.Builders.Shared.Models;

namespace UI.Modules.Header
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public class UiHeader : IUiHeader
    {
        public UiHeaderType Type { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Sets header
        /// </summary>
        public void SetHeader(UiHeaderType type, string title)
        {
            Type = type;
            Title = title;
        }
    }
}
