using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Interface;
using EF.Core.Generic.Data.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace EF.Core.Generic.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            DbContext = context ?? throw new ArgumentException(nameof(context));
            DbSet = DbContext.Set<T>();
        }

        #region Get Functions

        public T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = DbSet;

            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            return orderBy != null ? orderBy(query).FirstOrDefault() : query.FirstOrDefault();
        }

        public T Get(int id)
        {
            return DbContext.Find<T>(id);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
                IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>,
                IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true)
        {
            IQueryable<T> query = DbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query) : query;
        }

        public IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
            int size = 20, bool enableTracking = true)
        {
            IQueryable<T> query = DbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

        public IPaginate<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0, int size = 20, bool enableTracking = true) where TResult : class
        {
            IQueryable<T> query = DbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).Select(selector).ToPaginate(index, size)
                : query.Select(selector).ToPaginate(index, size);
        }

        #endregion

        #region Remove Functions

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Remove(params T[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Remove(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        #endregion

        #region Add Functions

        public virtual T Add(T entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public void Add(params T[] entities)
        {
            DbSet.AddRange(entities);
        }

        public void Add(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }

        #endregion

        #region Update Functions

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Update(params T[] entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
        }

        #endregion

        #region Async SingleOrDefault

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = DbSet;

            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            if (orderBy != null) return await orderBy(query).FirstOrDefaultAsync();

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        #endregion

        #region Async GetListAsync

        public Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = DbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken)
                : query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public Task<IPaginate<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true,
            CancellationToken cancellationToken = default,
            bool ignoreQueryFilters = false)
            where TResult : class
        {
            IQueryable<T> query = DbSet;

            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            return orderBy != null
                ? orderBy(query).Select(selector).ToPaginateAsync(index, size, 0, cancellationToken)
                : query.Select(selector).ToPaginateAsync(index, size, 0, cancellationToken);
        }

        #endregion

        #region Async Add Functions

        public virtual ValueTask<EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual Task AddAsync(params T[] entities)
        {
            return DbSet.AddRangeAsync(entities);
        }

        public virtual Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return DbSet.AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}