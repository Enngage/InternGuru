using Common.Config;
using System;
using System.IO;

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

        private static string ServerRootPath
        {
            get
            {
                return SystemConfig.ServerRootPath;
            }
        }

        /// <summary>
        /// Gets url to transparent image
        /// </summary>
        /// <returns>Url to transparent (1px) image</returns>
        public static string GetTransparentImage()
        {
            return AbsolutePath + "Content/images/transparent.png";
        }

        /// <summary>
        /// Gets url to company banner
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns>Url to company banner</returns>
        public static string GetCompanyBanner(string companyName)
        {
            var imagePath = AbsolutePath + FileConfig.BannerFolderPath;

            return GetFilePathWithExtension(imagePath, Entity.Company.GetLogoFileName(companyName));
        }

        /// <summary>
        /// Gets url to company logo
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns>Url to company logo</returns>
        public static string GetCompanyLogo(string companyName)
        {
            var imagePath = AbsolutePath + FileConfig.LogoFolderPath;

            return GetFilePathWithExtension(imagePath, Entity.Company.GetLogoFileName(companyName));
        }


        #region Helper methods


        /// <summary>
        /// Gets file path with extension
        /// </summary>
        /// <param name="path">Absolute path to file without extension</param>
        /// <param name="fileName">File name to look for</param>
        /// <returns>Extension of found file, null otherwise</returns>
        private static string GetFilePathWithExtension(string path, string fileName)
        {
            if (!String.IsNullOrEmpty(path)) {
                // remove first slash if its present
                var pathWithoutFirstSlash = path;
                if (path[0] == '/')
                {
                    pathWithoutFirstSlash = pathWithoutFirstSlash.Substring(1, pathWithoutFirstSlash.Length - 1);
                }

                var systemAbsolutePath = (ServerRootPath + pathWithoutFirstSlash).Replace('/', '\\');
                var existingFiles = Directory.GetFiles(systemAbsolutePath, fileName + ".*");
                if (existingFiles.Length > 0)
                {
                    // file exists - get extension
                    var extension = Path.GetExtension(existingFiles[0]);

                    return path + fileName + extension;
                }
            }
            return null;
        }

        #endregion
    }
}
