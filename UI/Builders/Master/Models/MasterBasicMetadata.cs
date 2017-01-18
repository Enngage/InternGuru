namespace UI.Builders.Master.Models
{
    public class MasterBasicMetadata
    {

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

                return title.Length<takeChars
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

            const int takeChars = 160;

            return description.Length < takeChars
                ? description
                : description.Substring(0, takeChars - 1);
        }

        #endregion

        /// <summary>
        /// Basic metadata title
        /// Title is not limited in size
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Basic metadata description
        /// Description is not limited in size. For shortened version use "DescriptionShortened" property
        /// </summary>
        public string Description { get; set; }
    }
}
