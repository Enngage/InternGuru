

namespace UI.Builders.Master.Models
{
    public class MasterGooglePlusMetadata
    {

        #region Methods

        /// <summary>
        /// Shortens title
        /// </summary>
        /// <returns>Shortened title</returns>
        public string ShortenName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            const int takeChars = 64;

            return name.Length < takeChars
                ? name
                : name.Substring(0, takeChars - 1);
        }

        /// <summary>
        /// Shortens description
        /// </summary>
        /// <returns>Shortened title</returns>
        public string ShortenDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return null;
            }

            const int takeChars = 160;

            return description.Length < takeChars
                ? description
                : description.Substring(0, takeChars - 1);
        }

        #endregion


        /// <summary>
        /// Name of the page (title)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of page
        /// Maximum 155 characters
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Path to image
        /// Needs to be converted to Absolute URL
        /// </summary>
        public string Image { get; set; }
    }
}
