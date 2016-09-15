using Common.Config;
using Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Core.Context
{
    public class AppContext : IdentityDbContext<ApplicationUser>, IAppContext
    {
        #region Entity registration + IAppContext members

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<Internship> Internships { get; set; }
        public IDbSet<InternshipCategory> InternshipCategories { get; set; }
        public IDbSet<CompanyCategory> CompanyCategories { get; set; }
        public IDbSet<Log> Logs { get; set; }

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
        }

        #endregion

        #region IAppContext

        public DbContextTransaction BeginTransaction()
        {
            return this.Database.BeginTransaction();
        }

        #endregion
    }
}
