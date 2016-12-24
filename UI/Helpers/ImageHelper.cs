using Core.Config;
using Core.Helpers;
using System;
using System.Collections.Generic;
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
        /// Gets url to image from "Content" folder and appends hash version (?v=kji424Nfs02dmsik24...)
        /// Example usage: GetImage("Images/logo.png")
        /// </summary>
        /// <param name="imagePath">Path to image</param>
        /// <returns>Url to image/returns>
        public static string GetImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            // remove slash if its present
            if (imagePath[0] == '/')
            {
                imagePath = imagePath.Substring(1, imagePath.Length);
            }

            return AbsolutePath + "Content/" + imagePath + "?v" + HashHelper.GetVersionHash();
        }


        /// <summary>
        /// Gets url to transparent image
        /// </summary>
        /// <returns>Url to transparent (1px) image</returns>
        public static string GetTransparentImage()
        {
            return AbsolutePath + FileConfig.TransparentImagePath;
        }

        /// <summary>
        /// Gets url to company banner
        /// </summary>
        /// <param name="companyGUID">companyGUID</param>
        /// <returns>Url to company banner</returns>
        public static string GetCompanyBanner(Guid companyGUID)
        {
            var imagePath = AbsolutePath + FileConfig.BannerFolderPath;

            var banner = GetFilePathWithExtension(imagePath, Entity.Company.GetLogoFileName(companyGUID));

            if (string.IsNullOrEmpty(banner))
            {
                // use default logo
                return AbsolutePath + FileConfig.DefaultCompanyLogoBanner;
            }
            else
            {
                return banner;
            }
        }

        /// <summary>
        /// Gets url to company logo
        /// </summary>
        /// <param name="companyGUID">companyGUID</param>
        /// <returns>Url to company logo</returns>
        public static string GetCompanyLogo(Guid companyGUID)
        {
            var imagePath = AbsolutePath + FileConfig.LogoFolderPath;

            var logo = GetFilePathWithExtension(imagePath, Entity.Company.GetLogoFileName(companyGUID));

            if (string.IsNullOrEmpty(logo))
            {
                // use default logo
                return AbsolutePath + FileConfig.DefaultCompanyLogoPath;
            }
            else
            {
                return logo;
            }
        }

        /// <summary>
        /// Gets company gallery images
        /// </summary>
        /// <param name="companyGUID">companyGUID</param>
        /// <returns>Dictionary containing file name and url to gallery files, empty dictionary is returned if there are no files in gallery</returns>
        public static Dictionary<string, string> GetCompanyGalleryImages(Guid companyGUID)
        {
            // get all files in given folder
            var galleryFiles = GetFilesFromFolder(Entity.Company.GetCompanyGalleryFolderPath(companyGUID));
            return galleryFiles == null ? new Dictionary<string, string>() : galleryFiles;
        }

        /// <summary>
        /// Gets url to user's avatar. Returns default avatar if none is found.
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <returns>Url to avatar of user</returns>
        public static string GetUserAvatar(string userName)
        {
            var imagePath = AbsolutePath + FileConfig.AvatarFolderPath;

            var avatar = GetFilePathWithExtension(imagePath, Entity.ApplicationUser.GetAvatarFileName(userName));

            if (string.IsNullOrEmpty(avatar))
            {
                // use default avatar if user's avatar is not found
                return AbsolutePath + FileConfig.DefaultAvatarPath;
            }
            else
            {
                return avatar;
            }
        }

        /// <summary>
        /// Gets system path to given file
        /// </summary>
        /// <param name="relativePath">Relative path</param>
        /// <returns>System path</returns>
        public static string GetSystemPathToFile(string relativePath)
        {
            if (!String.IsNullOrEmpty(relativePath))
            {
                // remove first slash if its present
                var pathWithoutFirstSlash = relativePath;
                if (relativePath[0] == '/')
                {
                    pathWithoutFirstSlash = pathWithoutFirstSlash.Substring(1, pathWithoutFirstSlash.Length - 1);
                }

                var systemAbsolutePath = (ServerRootPath + pathWithoutFirstSlash).Replace('/', '\\');

                return systemAbsolutePath;
            }
            return null;
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

        /// <summary>
        /// Gets all files from given folder
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <returns>File name and path to files, null if folder does not exist or path is invalid</returns>
        private static Dictionary<string, string> GetFilesFromFolder(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                var files = new Dictionary<string, string>();

                // remove first slash if its present
                var pathWithoutFirstSlash = path;
                if (path[0] == '/')
                {
                    pathWithoutFirstSlash = pathWithoutFirstSlash.Substring(1, pathWithoutFirstSlash.Length - 1);
                }

                var systemAbsolutePath = (ServerRootPath + pathWithoutFirstSlash).Replace('/', '\\');

                try
                {
                    var existingFiles = Directory.GetFiles(systemAbsolutePath);

                    foreach (var file in existingFiles)
                    {
                        var fileNameWithExtension = Path.GetFileName(file);
                        var urlPath = AbsolutePath + path + "/" + fileNameWithExtension;
                        files.Add(fileNameWithExtension, urlPath);
                    }

                    return files;
                }
                catch (DirectoryNotFoundException)
                {
                    return null;
                }
            }
            return null;
        }

        #endregion
    }
}
