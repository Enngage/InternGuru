using Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Service.Context
{
    public interface IAppContext : IDisposable
    {
        #region Entities 

        IDbSet<Company> Companies { get; }
        IDbSet<ApplicationUser> Users { get; }
        IDbSet<Internship> Internships { get; }
        IDbSet<InternshipCategory> InternshipCategories { get; }
        IDbSet<CompanyCategory> CompanyCategories { get; }
        IDbSet<Log> Logs { get; }
        IDbSet<Message> Messages { get; }
        IDbSet<Currency> Currencies { get; }
        IDbSet<InternshipAmountType> InternshipAmountTypes { get; }
        IDbSet<InternshipDurationType> InternshipDurationTypes { get; }
        IDbSet<CompanySize> CompanySizes { get; }
        IDbSet<Country> Countries { get; }
        IDbSet<ThesisType> ThesisTypes { get; }
        IDbSet<Thesis> Theses { get; }
        IDbSet<Language> Languages { get; }
        IDbSet<HomeOfficeOption> HomeOfficeOptions { get; }
        IDbSet<StudentStatusOption> StudentStatusOptions { get; }
        IDbSet<Activity> Activities { get; }
        IDbSet<Email> Emails { get; }
        IDbSet<Questionare> Questionares { get; }

        #endregion

        #region Save methods

        int SaveChanges();
        Task<int> SaveChangesAsync();

        #endregion

        #region DB methods

        DbContextTransaction BeginTransaction();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        #endregion
    }
}
