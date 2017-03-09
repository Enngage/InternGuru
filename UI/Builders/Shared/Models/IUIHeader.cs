

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
        UiHeaderTypeEnum Type { get; set; }

        /// <summary>
        /// Size of the header
        /// </summary>
        UIHeaderSizeEnum Size { get; set; }

        /// <summary>
        /// Color of the header (if applicable)
        /// Name of the enum represents class name of the color in _header.css
        /// </summary>
        UIHeaderColorEnum Color { get; set; }

        /// <summary>
        /// Text alignment
        /// </summary>
        UIHeaderTextAlignmentEnum TextAlignment { get; set; }

        /// <summary>
        /// Title of the header
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Text of the header (if applicable)
        /// </summary>
        string SubText { get; set; }

        /// <summary>
        /// Path to image when Headers with Type = BackGroundImage
        /// Example: /content/images//teaser.jpg
        /// </summary>
        string ImagePath { get; set; }

        /// <summary>
        /// Buttons
        /// </summary>
        IList<UIHeaderButton> Buttons { get; set; }

        /// <summary>
        /// If true, a button with scroll functionality will be included
        /// </summary>
        bool ShowScrollButton { get; set; }
    }
}
