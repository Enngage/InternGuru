
namespace Common.Config
{
    public static class FileConfig
    {
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
                return 1100;
            }
        }

        /// <summary>
        /// Company banner height in px
        /// </summary>
        public static int CompanyBannerHeight
        {
            get
            {
                return 350;
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
    }
}
