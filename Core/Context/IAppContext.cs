using Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Core.Context
{
    public interface IAppContext : IDisposable
    {
        IDbSet<Company> GetCompanies();
        IDbSet<ApplicationUser> GetUsers();
        IDbSet<Internship> GetInternships();
        IDbSet<Category> GetCategories();
        IDbSet<Log> GetLogs();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
