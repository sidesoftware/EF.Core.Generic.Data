using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Interface;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("EF.Core.Generic.Data.Tests")]

namespace EF.Core.Generic.Data
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        private Dictionary<(Type type, string name), object> _repositories;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Context to initialize with</param>
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Initializes an instance of the repository
        /// </summary>
        /// <typeparam name="TEntity">The entity type to initialize with</typeparam>
        /// <returns>An initialized repository</returns>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return (IRepository<TEntity>) GetOrAddRepository(typeof(TEntity), new Repository<TEntity>(Context));
        }

        public TContext Context { get; }

        /// <inheritdoc/>
        public int Commit(bool autoHistory = false)
        {
            if (autoHistory) Context.EnsureAutoHistory();
            return Context.SaveChanges();
        }

        /// <inheritdoc/>
        public async Task<int> CommitAsync(bool autoHistory = false, CancellationToken cancellationToken = default)
        {
            if (autoHistory) Context.EnsureAutoHistory();

            return await Context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public string GetDbConnection()
        {
            return Context.Database.GetDbConnection().ConnectionString;
        }

        /// <inheritdoc/>
        public bool CanConnect()
        {
            return Context.Database.CanConnect();
        }

        /// <inheritdoc/>
        public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Database.CanConnectAsync(cancellationToken);
        }

        /// <summary>
        /// Releases the allocated resources for this context
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }

        internal object GetOrAddRepository(Type type, object repo)
        {
            // Initialize dictionary if it is null
            _repositories ??= new Dictionary<(Type type, string Name), object>();

            // Pull out the repository if it exists
            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository)) return repository;

            // Add the repository to the dictionary
            _repositories.Add((type, repo.GetType().FullName), repo);
            return repo;
        }
    }
}