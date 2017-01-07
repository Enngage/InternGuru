using Core.Config;
using Core.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace UI.UIServices
{
    public class FileProvider : IFileProvider
    {
        /// <summary>
        /// Path to application where files will be saved
        /// </summary>
        private readonly string _rootPath = SystemConfig.ServerRootPath;
        private readonly int _maximumFileSize = AppConfig.MaximumFileSize;

        public IEnumerable<string> AllowedImageExtensions
        {
            get
            {
                return FileConfig.AllowedImageExtensions;
            }
        }

        public IEnumerable<string> AllowedFileExtensions
        {
            get
            {
                return FileConfig.AllowedFileExtensions;
            }
        }

        public void SaveFile(HttpPostedFileBase file, string path, string fileName)
        {
            if (file.ContentLength > 0)
            {
                if (file.ContentLength > _maximumFileSize)
                {
                    throw new FileUploadException(
                        $"Maximální velikost souboru je {ConvertHelper.FormatBytes(_maximumFileSize)}. Požadovaný soubor má {ConvertHelper.FormatBytes(file.ContentLength)}");
                }

                var extension = Path.GetExtension(file.FileName);

                if (extension != null)
                {
                    var fileExtension = extension.ToLower();

                    if (AllowedFileExtensions.Contains(fileExtension))
                    {
                        // file extension is allowed

                        // get absolute path where file will be saved
                        var absolutePath = _rootPath + path + fileName + fileExtension;

                        // save file
                        file.SaveAs(absolutePath);
                    }
                    else
                    {
                        // file extension is not allowed
                        throw new FileUploadException($"Soubory typu {fileExtension} nejsou povoleny");

                    }
                }
            }
            else
            {
                throw new FileUploadException("Vyberte soubor");
            }
        }

        public void SaveImage(HttpPostedFileBase file, string path, string fileName, int resizeToWidth = 0, int resizeToHeight = 0)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (file.ContentLength > _maximumFileSize)
                {
                    throw new FileUploadException(
                        $"Maximální velikost souboru je {ConvertHelper.FormatBytes(_maximumFileSize)}. Požadovaný soubor má {ConvertHelper.FormatBytes(file.ContentLength)}");
                }

                // add "/" to path if it is not present
                if (!path.EndsWith("/"))
                {
                    path += "/";
                }

                var extension = Path.GetExtension(file.FileName);
                if (extension != null)
                {
                    var fileExtension = extension.ToLower();

                    if (AllowedImageExtensions.Contains(fileExtension))
                    {
                        // get image from stream
                        var image = Image.FromStream(file.InputStream);

                        // resize image if necessary
                        if (resizeToWidth > 0 && resizeToHeight > 0)
                        {
                            // save resized image
                            var resizedImage = ResizeImage(image, resizeToWidth, resizeToHeight);

                            // delete existing images if they exist
                            var existingImage = Directory.GetFiles(_rootPath + path, fileName + ".*");
                            if (existingImage.Length > 0)
                            {
                                // file exists -- delete it
                                File.Delete(existingImage[0]);
                            }

                            // save resized image
                            var fullFileName = _rootPath + path + fileName + fileExtension;

                            resizedImage.Save(fullFileName);
                        }
                        else
                        {
                            // Save image as regular file
                            SaveFile(file, path, fileName);
                        }
                    }
                    else
                    {
                        throw new FileUploadException($"Soubory typu {fileExtension} nejsou povoleny");
                    }
                }
            }
            else
            {
                throw new FileUploadException("Vyberte soubor");
            }
        }

        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
