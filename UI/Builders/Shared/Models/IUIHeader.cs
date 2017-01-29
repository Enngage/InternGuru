

using System.Collections.Generic;
using UI.Builders.Shared.Enums;

namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Represents header used in shared layouts
    /// </summary>
    public interface IUiHeader
    {
        /// <summary>
        /// Type of the header
        /// </summary>
        UiHeaderTypeEnum Type { get; }

        /// <summary>
        /// Size of the header
        /// </summary>
        UIHeaderSizeEnum Size { get; }

        /// <summary>
        /// Color of the header (if applicable)
        /// Name of the enum represents class name of the color in _header.css
        /// </summary>
        UIHeaderColorEnum Color { get; }

        /// <summary>
        /// Text alignment
        /// </summary>
        UIHeaderTextAlignmentEnum TextAlignment { get; }

        /// <summary>
        /// Title of the header
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Text of the header (if applicable)
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Path to image when Headers with Type = BackGroundImage
        /// Example: /content/images//teaser.jpg
        /// </summary>
        string ImagePath { get; }

        /// <summary>
        /// Buttons
        /// </summary>
        IList<UIHeaderButton> Buttons { get; }

        /// <summary>
        /// If true, a button with scroll functionality will be included
        /// </summary>
        bool UseScrollButton { get; set; }

        /// <summary>
        /// Sets text-alignment
        /// </summary>
        /// <param name="textAlignment">Text alignment</param>
        void SetTextAlignment(UIHeaderTextAlignmentEnum textAlignment);

        /// <summary>
        /// Adds button to header
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="url">Url of button</param>
        /// <param name="type">Type of button</param>
        void AddButton(string text, string url, UIHeaderButtonTypeEnum type);

        /// <summary>
        /// Sets header using background image
        /// </summary>
        /// <param name="size">Size of the header</param>
        /// <param name="title">Title</param>
        /// <param name="imagePath">Example: /content/images//teaser.jpg</param>
        void UseBackgroundImage(UIHeaderSizeEnum size, string title, string imagePath);

        /// <summary>
        /// Sets header using background image
        /// </summary>
        /// <param name="size">Size of the header</param>
        /// <param name="title">Title</param>
        /// <param name="imagePath">Example: /content/images//teaser.jpg</param>
        /// <param name="text">Text</param>
        void UseBackgroundImage(UIHeaderSizeEnum size, string title, string imagePath, string text);

        /// <summary>
        /// Sets header using colored background
        /// </summary>
        /// <param name="size">Size of the header</param>
        /// <param name="color">Color of header</param>
        /// <param name="title">Title</param>
        void UseColoredHeader(UIHeaderSizeEnum size, UIHeaderColorEnum color, string title);


        /// <summary>
        /// Sets header
        /// </summary>
        /// <param name="size">Size of the header</param>
        /// <param name="color">Color of header</param>
        /// <param name="title">Title</param>
        /// <param name="text">Text</param>
        void UseColoredHeader(UIHeaderSizeEnum size, UIHeaderColorEnum color, string title, string text);
    }
}
