

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
        UiHeaderType Type { get; }

        /// <summary>
        /// Color of the header (if applicable)
        /// Name of the enum represents class name of the color in _header.css
        /// </summary>
        UIHeaderColor Color { get; }

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
        /// If true, a button with scroll functionality will be included
        /// </summary>
        bool UseScrollButton { get; set; }

        /// <summary>
        /// Sets header
        /// </summary>
        /// <param name="type">Type of header</param>
        /// <param name="title">Title</param>
        void SetHeader(UiHeaderType type, string title);

        /// <summary>
        /// Sets header
        /// </summary>
        /// <param name="type">Type of header</param>
        /// <param name="title">Title</param>
        /// <param name="imagePath">Example: /content/images//teaser.jpg</param>
        void SetHeader(UiHeaderType type, string title, string imagePath);

        /// <summary>
        /// Sets header
        /// </summary>
        /// <param name="type">Type of Header</param>
        /// <param name="title">Title</param>
        /// <param name="imagePath">Example: /content/images//teaser.jpg</param>
        /// <param name="text">Text</param>
        void SetHeader(UiHeaderType type, string title, string imagePath, string text);

        /// <summary>
        /// Sets header
        /// </summary>
        /// <param name="type">Type of header</param>
        /// <param name="color">Color of header</param>
        /// <param name="title">Title</param>
        void SetHeader(UiHeaderType type, UIHeaderColor color, string title);


        /// <summary>
        /// Sets header
        /// </summary>
        /// <param name="type">Type of header</param>
        /// <param name="color">Color of header</param>
        /// <param name="title">Title</param>
        /// <param name="text">Text</param>
        void SetHeader(UiHeaderType type, UIHeaderColor color, string title, string text);
    }
}
