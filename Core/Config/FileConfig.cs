
using System.Collections.Generic;

namespace Core.Config
{
    public static class FileConfig
    {
        #region General

        /// <summary>
        /// Path to transparent image
        /// </summary>
        public static string TransparentImagePath
        {
            get
            {
                return "Content/images/transparent.png";
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
        /// Path where company gallery images are stored
        /// </summary>
        public static string CompanyalleryImagesPath
        {
            get
            {
                return "Content/Company/Gallery";
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
        public static string BannerFolderPath
        {
            get
            {
                return "Content/Company/Banners/";
            }
        }

        /// <summary>
        /// Path where logos will be stored
        /// </summary>
        public static string LogoFolderPath
        {
            get
            {
                return "Content/Company/Logos/";
            }
        }

        /// <summary>
        /// Path to default company path
        /// </summary>
        public static string DefaultCompanyLogoPath
        {
            get
            {
                return "Content/images/icons/defaultCompanyLogo.png";
            }
        }

        /// <summary>
        /// Path to default company banner path
        /// </summary>
        public static string DefaultCompanyLogoBanner
        {
            get
            {
                return "Content/images/icons/defaultCompanyBanner.png";
            }
        }

        #endregion

        #region Avatar config

        /// <summary>
        /// Path where avatars are stored
        /// </summary>
        public static string AvatarFolderPath
        {
            get
            {
                return "Content/Avatars/";
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
                return "Content/images/icons/defaultAvatar.png";
            }
        }

        #endregion


    }
}
