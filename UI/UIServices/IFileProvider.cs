using System.Collections.Generic;
using System.Drawing;
using System.Web;

namespace UI.UIServices
{
    public interface IFileProvider
    {
        /// <summary>
        /// Collection of allowed file extensions. Files with other extension will not be accepted
        /// </summary>
        IEnumerable<string> AllowedFileExtensions { get; }

        /// <summary>
        /// Collection of allowed image extensions. Files with other extension will not be accepted
        /// </summary>
        IEnumerable<string> AllowedImageExtensions { get; }

        /// <summary>
        /// Saves given image to specified location
        /// Image will be resized only if both height and width are provided
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="resizeToHeight">Resizes image to specified height</param>
        /// <param name="resizeToWidth">Resizes image to specified width</param>
        /// <param name="path">Target location (eg "\\Content\\Folder")</param>
        /// <param name="fileName">File name with extension</param>
        void SaveImage(HttpPostedFileBase image, string path, string fileName, int resizeToWidth = 0, int resizeToHeight = 0);

        /// <summary>
        /// Saves file to specified location
        /// </summary>
        /// <param name="file">File to save</param>
        /// <param name="path">Target location</param>
        /// <param name="fileName">File name with extension</param>
        void SaveFile(HttpPostedFileBase file, string path, string fileName);

        /// <summary>
        /// Resizes the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to. Must be greater then 0</param>
        /// <param name="height">The height to resize to. Must be greater then 0</param>
        /// <returns>The resized image</returns>
        Bitmap ResizeImage(Image image, int width, int height);
    }
}
