
namespace Common.Config
{
    public static class SystemConfig
    {
        #region Connection string

        /// <summary>
        /// Represents default connection string
        /// </summary>
        public static string DefaultConnectionStringName
        {
            get
            {
                return "DefaultConnection";
            }
        }

        #endregion

        #region Domain

        public static string DomainName { get; set; }

        #endregion    

        #region System paths

        private static string serverRootPath = null;
        private static string serverVirtualPath = null;

        /// <summary>
        /// Represents root path of server 
        /// - Server.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath)
        /// </summary>
        public static string ServerRootPath
        {
            get
            {
                return serverRootPath;
            }
        }

        /// <summary>
        /// Represents virtual root path of server 
        /// - Server.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath)
        /// </summary>
        public static string ServerVirtualPath
        {
            get
            {
                return serverVirtualPath;
            }
        }

        /// <summary>
        /// Represents root path of server
        /// </summary>
        /// <param name="rootPath">rootPath</param>
        public static void SetServerRootPath(string rootPath)
        {
            serverRootPath = rootPath;
        }

        /// <summary>
        /// Represents viritual root path of server 
        /// </summary>
        /// <param name="rootPath">rootPath</param>
        public static void SetServerVirtualRootPath(string virtualRootPath)
        {
            serverVirtualPath = virtualRootPath;
        }

        #endregion

    }
}
