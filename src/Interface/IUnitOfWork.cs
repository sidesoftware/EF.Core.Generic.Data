using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Generic.Data.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        /// <summary>
        ///     Saves all changes made in this context to the database.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
        ///         to discover any changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
        ///         includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        ///         Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        ///         in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information
        ///         and examples.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <param name="autoHistory">Ensures the automatic history</param>
        /// <returns>
        ///     The number of state entries written to the database.
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        int Commit(bool autoHistory = false);

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
        ///         to discover any changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
        ///         includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        ///         Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        ///         in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more
        ///         information and examples.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <param name="autoHistory">Ensures the automatic history</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> CommitAsync(bool autoHistory = false, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the underlying ADO.NET <see cref="T:System.Data.Common.DbConnection.ConnectionString" /> for this <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-connections">Connections and connection strings</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <returns>The <see cref="T:System.Data.Common.DbConnection.ConnectionString" /></returns>
        string GetDbConnection();

        /// <summary>
        ///     Determines whether or not the database is available and can be connected to.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Any exceptions thrown when attempting to connect are caught and not propagated to the application.
        ///     </para>
        ///     <para>
        ///         The configured connection string is used to create the connection in the normal way, so all
        ///         configured options such as timeouts are honored.
        ///     </para>
        ///     <para>
        ///         Note that being able to connect to the database does not mean that it is
        ///         up-to-date with regard to schema creation, etc.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-connections">Database connections in EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <returns><see langword="true" /> if the database is available; <see langword="false" /> otherwise.</returns>
        bool CanConnect();

        /// <summary>
        ///     Determines whether or not the database is available and can be connected to.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Any exceptions thrown when attempting to connect are caught and not propagated to the application.
        ///     </para>
        ///     <para>
        ///         The configured connection string is used to create the connection in the normal way, so all
        ///         configured options such as timeouts are honored.
        ///     </para>
        ///     <para>
        ///         Note that being able to connect to the database does not mean that it is
        ///         up-to-date with regard to schema creation, etc.
        ///     </para>
        ///     <para>
        ///         Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
        ///         includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        ///         Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        ///         in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see>
        ///         for more information and examples.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-connections">Database connections in EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><see langword="true" /> if the database is available; <see langword="false" /> otherwise.</returns>
        /// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
        Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
    }

    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}