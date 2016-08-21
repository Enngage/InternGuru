using Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Core.Context
{
    public interface IAppContext : IDisposable
    {
        IDbSet<Company> Companies { get; }
        IDbSet<ApplicationUser> Users { get; }
        IDbSet<Internship> Internships { get; }
        IDbSet<Category> Categories { get; }
        IDbSet<Log> Logs { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
