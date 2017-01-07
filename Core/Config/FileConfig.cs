
using System.Collections.Generic;

namespace Core.Config
{
    public static class FileConfig
    {
        #region General

        /// <summary>
        /// Base folder for storing files
        /// </summary>
        public static string BaseFolderName
        {
            get
            {
                return "Content";
            }
        }

        /// <summary>
        /// Path to error icon
        /// </summary>
        public static string ErrorIconFilePath
        {
            get
            {
                return $"{BaseFolderName}/Images/Base/Caution.png";
            }
        }

        /// <summary>
        /// Path to transparent image
        /// </summary>
        public static string TransparentImagePath
        {
            get
            {
                return $"{BaseFolderName}/Images/Base/transparent.png";
            }
        }

        public static IEnumerable<string> AllowedImageExtensions
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

        public static IEnumerable<string> AllowedFileExtensions
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

        #endregion

        #region Company files

        /// <summary>
        /// Base path for Company files
        /// </summary>
        public static string CompanyBaseFolderPath
        {
            get
            {
                return $"{BaseFolderName}/Company";
            }
        }

        /// <summary>
        /// Path where company gallery images are stored
        /// </summary>
        public static string CompanyGalleryFolderName
        {
            get
            {
                return "Gallery";
            }
        }

        /// <summary>
        /// Company logo width in px
        /// </summary>
        public static int CompanyLogoWidth
        {
            get
            {
                return 200;
            }
        }

        /// <summary>
        /// Company logo height in px
        /// </summary>
        public static int CompanyLogoHeight
        {
            get
            {
                return 80;
            }
        }

        /// <summary>
        /// Company banner width in px
        /// </summary>
        public static int CompanyBannerWidth
        {
            get
            {
                return 1280;
            }
        }

        /// <summary>
        /// Company banner height in px
        /// </summary>
        public static int CompanyBannerHeight
        {
            get
            {
                return 280;
            }
        }

        /// <summary>
        /// Path where banners will be stored
        /// </summary>
        public static string BannerFolderName
        {
            get
            {
                return "Banner";
            }
        }

        /// <summary>
        /// Default file name for company banner
        /// </summary>
        public static string BannerFileName
        {
            get
            {
                return "Banner";
            }
        }

        /// <summary>
        /// Path where logos will be stored
        /// </summary>
        public static string LogoFolderName
        {
            get
            {
                return "Logo";
            }
        }

        /// <summary>
        /// Default file name for company logo
        /// </summary>
        public static string LogoFileName
        {
            get
            {
                return "Logo";
            }
        }

        /// <summary>
        /// Path to default company path
        /// </summary>
        public static string DefaultCompanyLogoPath
        {
            get
            {
                return $"{BaseFolderName}/Images/Default/defaultCompanyLogo.png";
            }
        }

        /// <summary>
        /// Path to default company banner path
        /// </summary>
        public static string DefaultCompanyLogoBanner
        {
            get
            {
                return $"{BaseFolderName}/Images/Default/defaultCompanyBanner.png";
            }
        }

        #endregion

        #region User files

        /// <summary>
        /// Path of base user folder
        /// </summary>
        public static string BaseUserFolderPath
        {
            get
            {
                return $"{BaseFolderName}/User";
            }
        }

        /// <summary>
        /// Path where avatars are stored
        /// </summary>
        public static string AvatarFolderName
        {
            get
            {
                return "Avatar";
            }
        }

        /// <summary>
        /// Default avatar file name
        /// </summary>
        public static string DefaultAvatarName
        {
            get
            {
                return "avatar";
            }
        }

        /// <summary>
        /// Side size of users avatar (avatar is squared) in pixels
        /// </summary>
        public static int AvatarSideSize
        {
            get
            {
                return 80;
            }
        }

        /// <summary>
        /// Path to default avatar
        /// </summary>
        public static string DefaultAvatarPath
        {
            get
            {
                return $"{BaseFolderName}/Images/Default/defaultAvatar.png";
            }
        }

        #endregion


    }
}
