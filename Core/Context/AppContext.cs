using Common.Config;
using Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Core.Context
{
    public class AppContext : IdentityDbContext<ApplicationUser>, IAppContext
    {
        #region Entity registration

        public DbSet<Company> Companies { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Log> Logs { get; set; }

        #endregion

        #region IAppContext members

        public IDbSet<Company> GetCompanies()
        {
            return Companies;
        }

        public IDbSet<ApplicationUser> GetUsers()
        {
            return this.Users;
        }

        public IDbSet<Internship> GetInternships()
        {
            return Internships;
        }

        public IDbSet<Category> GetCategories()
        {
            return Categories;
        }

        public IDbSet<Log> GetLogs()
        {
            return Logs;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor is required by EF for migrations
        /// DO NOT delete it or use in code directly
        /// </summary>
        public AppContext()
        {
            new AppContext(null);
        }

        /// <summary>
        /// Use this constructor
        /// </summary>
        /// <param name="config"></param>
        public AppContext(AppContextConfig config = null) : base(SystemConfig.DefaultConnectionStringName, false) // set IsIdentityV1Schema to false to prevent executing additional SQL queries
        {
            // set configuration
            if (config != null)
            {
                this.Configuration.AutoDetectChangesEnabled = config.AutoDetectChanges;
            }
            else
            {
                var defaultConfig = new AppContextConfig();
                this.Configuration.AutoDetectChangesEnabled = defaultConfig.AutoDetectChanges;
            }

            //if (HttpContext.Current != null)
            //{
            //    if (HttpContext.Current.User != null)
            //    {
            //        if (HttpContext.Current.User.Identity.IsAuthenticated != false)
            //        {
            //            currentIdentity = HttpContext.Current.User.Identity;
            //        }
            //    }
            //}
        }

        #endregion
    }
}
