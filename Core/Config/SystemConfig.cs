
namespace Core.Config
{
    public static class SystemConfig
    {
        #region Connection string

        /// <summary>
        /// Represents default connection string
        /// </summary>
        public static string DefaultConnectionStringName => "DefaultConnection";

        #endregion

        #region Domain

        public static string DomainName { get; set; }

        #endregion    

        #region System paths

        /// <summary>
        /// Represents root path of server 
        /// - Server.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath)
        /// </summary>
        public static string ServerRootPath { get; private set; }

        /// <summary>
        /// Represents virtual root path of server 
        /// - Server.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.MapPath("~")
        /// or
        /// - System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath)
        /// </summary>
        public static string ServerVirtualPath { get; private set; }

        /// <summary>
        /// Represents root path of server
        /// </summary>
        /// <param name="rootPath">rootPath</param>
        public static void SetServerRootPath(string rootPath)
        {
            ServerRootPath = rootPath;
        }

        /// <summary>
        /// Represents viritual root path of server 
        /// </summary>
        public static void SetServerVirtualRootPath(string virtualRootPath)
        {
            ServerVirtualPath = virtualRootPath;
        }

        #endregion

    }
}
