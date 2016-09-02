using System;

namespace UI.Helpers
{
    public static class ImageHelper
    {
        private static string AbsolutePath
        {
            get
            {
                return "/";
            }
        }

        /// <summary>
        /// Gets url to transparent image
        /// </summary>
        /// <returns>Url to transparent (1px) image</returns>
        public static string GetTransparentImage()
        {
            return AbsolutePath + "Content/images/general/transparent.png";
        }
    }
}
