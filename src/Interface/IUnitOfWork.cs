using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Generic.Data.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int Commit(bool autoHistory = false);
        Task<int> CommitAsync(bool autoHistory = false);
        string GetDbConnection();
    }

    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}