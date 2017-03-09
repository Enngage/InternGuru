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
        public string Title { get; set; }
        public string SubText { get; set; }
        public string ImagePath { get; set; }
        public IList<UIHeaderButton> Buttons { get; set; } = new List<UIHeaderButton>();

        public UiHeaderTypeEnum Type { get; set; } = UiHeaderTypeEnum.None;
        public UIHeaderSizeEnum Size { get; set; } = UIHeaderSizeEnum.Medium;
        public UIHeaderColorEnum Color { get; set; } = UIHeaderColorEnum.None;
        public UIHeaderTextAlignmentEnum TextAlignment { get; set; } = UIHeaderTextAlignmentEnum.Left;
        public bool ShowScrollButton { get; set; } = false;
        public bool IsMainHeader { get; set; } = true;

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
