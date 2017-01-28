using Core.Config;
using UI.Builders.Master.Enums;

namespace UI.Builders.Master.Models
{
    public class MasterOpenGraphMetadata
    {
        private string _section;

        #region Methods

        /// <summary>
        /// Shortens title 
        /// </summary>
        /// <returns>Shortened title</returns>
        public string ShortenTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return null;
            }

            const int takeChars = 64;

            return title.Length < takeChars
                ? title
                : title.Substring(0, takeChars - 1);
        }

        /// <summary>
        /// Shortens description 
        /// </summary>
        /// <returns>Shortened description</returns>
        public string ShortenDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return null;
            }

            const int takeChars = 300;

            return description.Length < takeChars
                ? description
                : description.Substring(0, takeChars - 1);
        }

        #endregion

        /// <summary>
        /// Title of open graph content
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type of open graph content
        /// </summary>
        public OpenGraphType Type { get; set; } = OpenGraphType.article;

        /// <summary>
        /// Path to image
        /// Needs to be converted to Absolute URL
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Section name
        /// Uses Title if not set
        /// </summary>
        public string Section
        {
            get
            {
                return string.IsNullOrEmpty(_section) ? Title : _section;
            }
            set
            {
                _section = value;
            }
        }

        /// <summary>
        /// Description of content
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Site name
        /// </summary>
        public string SiteName { get; } = AppConfig.SiteName;
    }
}
