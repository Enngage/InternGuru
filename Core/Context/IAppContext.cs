﻿using Entity;
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
        IDbSet<InternshipCategory> InternshipCategories { get; }
        IDbSet<CompanyCategory> CompanyCategories { get; }
        IDbSet<Log> Logs { get; }
        IDbSet<Message> Messages { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbContextTransaction BeginTransaction();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
