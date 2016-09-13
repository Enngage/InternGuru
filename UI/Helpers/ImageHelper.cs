using Common.Config;
using Common.Helpers;
using Common.Project;
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
        /// <param name="companyName">Company name</param>
        /// <returns>Url to company logo</returns>
        public static string GetCompanyLogo(string companyName)
        {
            var imagePath = AbsolutePath + FileConfig.LogoFolderPath;

            return GetFilePathWithExtension(imagePath, Entity.Company.GetLogoFileName(companyName));
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
