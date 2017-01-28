using UI.Builders.Shared.Enums;
using UI.Builders.Shared.Models;

namespace UI.Modules.Header
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public class UiHeader : IUiHeader
    {
        public UiHeaderType Type { get; private set; } = UiHeaderType.None;
        public string Title { get; private set; }
        public UIHeaderColor Color { get; private set; } = UIHeaderColor.None;
        public string Text { get; private set; }
        public string ImagePath { get; private set; }
        public bool UseScrollButton { get; set; } = false;

        public void SetHeader(UiHeaderType type, string title)
        {
            Type = type;
            Title = title;
        }

        public void SetHeader(UiHeaderType type, string title, string imagePath)
        {
            Type = type;
            Title = title;
            ImagePath = imagePath;
        }

        public void SetHeader(UiHeaderType type, string title, string imagePath, string text)
        {
            Type = type;
            Title = title;
            ImagePath = imagePath;
            Text = text;
        }

        public void SetHeader(UiHeaderType type, UIHeaderColor color, string title)
        {
            Type = type;
            Color = color;
            Title = title;
        }

        public void SetHeader(UiHeaderType type, UIHeaderColor color, string title, string text)
        {
            Type = type;
            Color = color;
            Title = title;
            Text = text;
        }
    }
}
