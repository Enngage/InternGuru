
using System.Collections.Generic;

namespace Core.Config
{
    public static class FileConfig
    {
        #region General

        /// <summary>
        /// Base folder for storing files
        /// </summary>
        public static string BaseFolderName => "Content";

        /// <summary>
        /// Path to error icon
        /// </summary>
        public static string ErrorIconFilePath => $"{BaseFolderName}/Images/Base/Caution.png";

        /// <summary>
        /// Path to transparent image
        /// </summary>
        public static string TransparentImagePath => $"{BaseFolderName}/Images/Base/transparent.png";

        public static IEnumerable<string> AllowedImageExtensions => new List<string>()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
        };

        public static IEnumerable<string> AllowedFileExtensions => new List<string>()
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

        #endregion

        #region Company files

        /// <summary>
        /// Base path for Company files
        /// </summary>
        public static string CompanyBaseFolderPath => $"{BaseFolderName}/Company";

        /// <summary>
        /// Path where company gallery images are stored
        /// </summary>
        public static string CompanyGalleryFolderName => "Gallery";

        /// <summary>
        /// Company logo width in px
        /// </summary>
        public static int CompanyLogoWidth => 80;

        /// <summary>
        /// Company logo height in px
        /// </summary>
        public static int CompanyLogoHeight => 80;

        /// <summary>
        /// Company banner width in px
        /// </summary>
        public static int CompanyBannerWidth => 1280;

        /// <summary>
        /// Company banner height in px
        /// </summary>
        public static int CompanyBannerHeight => 280;

        /// <summary>
        /// Path where banners will be stored
        /// </summary>
        public static string BannerFolderName => "Banner";

        /// <summary>
        /// Default file name for company banner
        /// </summary>
        public static string BannerFileName => "Banner";

        /// <summary>
        /// Path where logos will be stored
        /// </summary>
        public static string LogoFolderName => "Logo";

        /// <summary>
        /// Default file name for company logo
        /// </summary>
        public static string LogoFileName => "Logo";

        /// <summary>
        /// Path to default company path
        /// </summary>
        public static string DefaultCompanyLogoPath => $"{BaseFolderName}/Images/Default/defaultCompanyLogo.png";

        /// <summary>
        /// Path to default company banner path
        /// </summary>
        public static string DefaultCompanyLogoBanner => $"{BaseFolderName}/Images/Default/defaultCompanyBanner.png";

        #endregion

        #region User files

        /// <summary>
        /// Path of base user folder
        /// </summary>
        public static string BaseUserFolderPath => $"{BaseFolderName}/User";

        /// <summary>
        /// Path where avatars are stored
        /// </summary>
        public static string AvatarFolderName => "Avatar";

        /// <summary>
        /// Default avatar file name
        /// </summary>
        public static string DefaultAvatarName => "avatar";

        /// <summary>
        /// Side size of users avatar (avatar is squared) in pixels
        /// </summary>
        public static int AvatarSideSize => 80;

        /// <summary>
        /// Path to default avatar
        /// </summary>
        public static string DefaultAvatarPath => $"{BaseFolderName}/Images/Default/defaultAvatar.png";

        #endregion

    }
}
