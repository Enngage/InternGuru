using Core.Config;
using Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Service.Context
{
    public class AppContext : IdentityDbContext<ApplicationUser>, IAppContext
    {
        #region Entity registration + IAppContext members

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<Internship> Internships { get; set; }
        public IDbSet<InternshipCategory> InternshipCategories { get; set; }
        public IDbSet<CompanyCategory> CompanyCategories { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<Currency> Currencies { get; set; }
        public IDbSet<InternshipAmountType> InternshipAmountTypes { get; set; }
        public IDbSet<InternshipDurationType> InternshipDurationTypes { get; set; }
        public IDbSet<CompanySize> CompanySizes { get; set; }
        public IDbSet<Country> Countries { get; set; }
        public IDbSet<ThesisType> ThesisTypes { get; set; }
        public IDbSet<Thesis> Theses { get; set; }
        public IDbSet<Language> Languages { get; set; }
        public IDbSet<HomeOfficeOption> HomeOfficeOptions { get; set; }
        public IDbSet<StudentStatusOption> StudentStatusOptions { get; set; }
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<Email> Emails { get; set; }
        public IDbSet<Questionnaire> Questionnaires { get; set; }
        public IDbSet<EducationType> EducationTypes { get; set; }
        public IDbSet<QuestionnaireSubmission> QuestionnaireSubmissions { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor is required by EF for migrations
        /// DO NOT delete it or use in code directly
        /// </summary>
        public AppContext()
        {
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
                Configuration.AutoDetectChangesEnabled = config.AutoDetectChanges;
            }
            else
            {
                var defaultConfig = new AppContextConfig();
                Configuration.AutoDetectChangesEnabled = defaultConfig.AutoDetectChanges;
            }
        }

        #endregion

        #region IAppContext methods

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // disable cascade on following entities:
            // see https://social.msdn.microsoft.com/Forums/en-US/58a4e272-ee28-4245-ba95-ca7edc818e7a/sql-exception-foreign-key-may-cause-multiple-cascade-path-specify-on-delete-no-action?forum=adodotnetentityframework
            modelBuilder.Entity<Internship>()
                .HasRequired(p => p.CreatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Internship>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Internship>()
                .HasRequired(p => p.MinDurationType)
                .WithMany()
                .HasForeignKey(p => p.MinDurationTypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Internship>()
                .HasRequired(p => p.MaxDurationType)
                .WithMany()
                .HasForeignKey(p => p.MaxDurationTypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Internship>()
                .HasRequired(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasRequired(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasRequired(p => p.CreatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Thesis>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Thesis>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Email>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Email>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Questionnaire>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Questionnaire>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionnaireSubmission>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<QuestionnaireSubmission>()
                .HasRequired(p => p.UpdatedByApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByApplicationUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionnaireSubmission>()
               .HasRequired(p => p.Questionnaire)
               .WithMany()
               .HasForeignKey(p => p.QuestionnaireID)
               .WillCascadeOnDelete(false);
            // end cascade delete
        }
    }
}
