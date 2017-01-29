using System.Collections.Generic;
using UI.Builders.Shared.Enums;
using UI.Builders.Shared.Models;

namespace UI.Modules.Header
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public class UiHeader : IUiHeader
    {
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string ImagePath { get; private set; }
        public IList<UIHeaderButton> Buttons { get; private set; } = new List<UIHeaderButton>();

        public UiHeaderTypeEnum Type { get; private set; } = UiHeaderTypeEnum.None;
        public UIHeaderSizeEnum Size { get; private set; } = UIHeaderSizeEnum.Medium;
        public UIHeaderColorEnum Color { get; private set; } = UIHeaderColorEnum.None;
        public UIHeaderTextAlignmentEnum TextAlignment { get; private set; } = UIHeaderTextAlignmentEnum.Left;

        public bool UseScrollButton { get; set; } = false;

        public void UseBackgroundImage(UIHeaderSizeEnum size, string title, string imagePath)
        {
            Type = UiHeaderTypeEnum.BackgroundImage;
            Size = size;
            Title = title;
            ImagePath = imagePath;
        }

        public void UseBackgroundImage(UIHeaderSizeEnum size, string title, string imagePath, string text)
        {
            Type = UiHeaderTypeEnum.BackgroundImage;
            Size = size;
            Title = title;
            ImagePath = imagePath;
            Text = text;
        }

        public void UseColoredHeader(UIHeaderSizeEnum size, UIHeaderColorEnum color, string title)
        {
            Type = UiHeaderTypeEnum.Colored; 
            Size = size;
            Color = color;
            Title = title;
        }

        public void UseColoredHeader(UIHeaderSizeEnum size, UIHeaderColorEnum color, string title, string text)
        {
            Type = UiHeaderTypeEnum.Colored;
            Size = size;
            Color = color;
            Title = title;
            Text = text;
        }

        public void SetTextAlignment(UIHeaderTextAlignmentEnum textAlignment)
        {
            TextAlignment = textAlignment;
        }

        public void AddButton(string text, string url, UIHeaderButtonTypeEnum type)
        {
            Buttons.Add(new UIHeaderButton()
            {
                Type = type,
                Text = text,
                ButtonUrl = url
            });
        }
    }
}
