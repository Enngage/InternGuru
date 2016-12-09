using Core.Config;
using Core.Helpers;
using System;
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
        private string rootPath = SystemConfig.ServerRootPath;
        private int maximumFileSize = AppConfig.MaximumFileSize;

        public IEnumerable<string> AllowedImageExtensions
        {
            get
            {
                return new List<string>()
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".gif",
                };
            }
        }

        public IEnumerable<string> AllowedFileExtensions
        {
            get
            {
                return new List<string>()
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".gif",
                    ".xls",
                    ".xml",
                    ".doc",
                    ".docx"
                };
            }
        }

        public void SaveFile(HttpPostedFileBase file, string path, string fileName)
        {
            if (file.ContentLength > 0)
            {
                if (file.ContentLength > maximumFileSize)
                {
                    throw new FileUploadException(String.Format("Maximální velikost souboru je {0}. Požadovaný soubor má {1}", ConvertHelper.FormatBytes(maximumFileSize), ConvertHelper.FormatBytes(file.ContentLength)));
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (this.AllowedFileExtensions.Contains(fileExtension))
                {
                        // file extension is allowed

                        // get absolute path where file will be saved
                        string absolutePath = rootPath + path + fileName + fileExtension;

                        // save file
                        file.SaveAs(absolutePath);
                }
                else
                {
                    // file extension is not allowed
                    throw new FileUploadException(String.Format("Soubory typu {0} nejsou povoleny", fileExtension));

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
                if (file.ContentLength > maximumFileSize)
                {
                    throw new FileUploadException(String.Format("Maximální velikost souboru je {0}. Požadovaný soubor má {1}", ConvertHelper.FormatBytes(maximumFileSize), ConvertHelper.FormatBytes(file.ContentLength)));
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (this.AllowedImageExtensions.Contains(fileExtension))
                {
                    // get image from stream
                    var image = Image.FromStream(file.InputStream);

                    // resize image if necessary
                    if (resizeToWidth > 0 && resizeToHeight > 0)
                    {
                        // save resized image
                        var resizedImage = ResizeImage(image, resizeToWidth, resizeToHeight);

                        // delete existing images if they exist
                        var existingImage = Directory.GetFiles(rootPath + path, fileName + ".*");
                        if (existingImage.Length > 0)
                        {
                            // file exists -- delete it
                            System.IO.File.Delete(existingImage[0]);
                        }

                        // save resized image
                        string fullFileName = rootPath + path + fileName + fileExtension;

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
                    throw new FileUploadException(String.Format("Soubory typu {0} nejsou povoleny", fileExtension));
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
