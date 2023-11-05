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

namespace EF.Core.Generic.Data;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<T> DbSet;

    public Repository(DbContext context)
    {
        DbContext = context ?? throw new ArgumentException("DbContext");
        DbSet = DbContext.Set<T>();
    }

    public void Dispose()
    {
        DbContext?.Dispose();
    }

    private IQueryable<T> Query(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = DbSet;

        if (!enableTracking) query = query.AsNoTracking();

        if (include != null) query = include(query);

        if (predicate != null) query = query.Where(predicate);

        if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

        return orderBy != null ? orderBy(query) : query;
    }

    #region Get Functions

    public T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true,
        bool ignoreQueryFilters = false)
    {
        return Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters).SingleOrDefault();
    }

    public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        bool enableTracking = true,
        bool ignoreQueryFilters = false)
    {
        return await Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters).SingleOrDefaultAsync();
    }

    public T FirstOrDefault(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true,
        bool ignoreQueryFilters = false)
    {
        return Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters).FirstOrDefault();
    }

    public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        bool enableTracking = true,
        bool ignoreQueryFilters = false)
    {
        return await Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters).FirstOrDefaultAsync();
    }

    public TResult GetField<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector)
    {
        IQueryable<T> query = DbSet;
        return query.Where(predicate).Select(selector).FirstOrDefault();
    }

    public async Task<TResult> GetFieldAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector)
    {
        IQueryable<T> query = DbSet;
        return await query.Where(predicate).Select(selector).FirstOrDefaultAsync();
    }

    public virtual async Task<T> GetAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public T Get(int id)
    {
        return DbContext.Find<T>(id);
    }

    public int Count()
    {
        return DbSet.Count();
    }

    public int Count(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = DbSet;
        query = query.Where(predicate);
        return query.Count();
    }

    public async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = DbSet;
        query = query.Where(predicate);
        return await query.CountAsync();
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null,
        bool enableTracking = true, bool ignoreQueryFilters = false)
    {
        return Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null,
        bool enableTracking = true, bool ignoreQueryFilters = false)
    {
        return await Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters).ToListAsync();
    }

    public IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
        int size = 20, bool enableTracking = true, bool ignoreQueryFilters = false)
    {
        return Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters).ToPaginate(index, size);
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

    public async Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        int index = 0,
        int size = 20,
        bool enableTracking = true, bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await Query(predicate, orderBy, include, enableTracking, ignoreQueryFilters)
            .ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public async Task<IPaginate<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null,
        int index = 0, int size = 20, bool enableTracking = true, bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default) where TResult : class
    {
        IQueryable<T> query = DbSet;

        if (!enableTracking) query = query.AsNoTracking();

        if (include != null) query = include(query);

        if (predicate != null) query = query.Where(predicate);

        if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

        return orderBy != null
            ? await orderBy(query).Select(selector).ToPaginateAsync(index, size, 0, cancellationToken)
            : await query.Select(selector).ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public IQueryable<T> GetSql(string sql, params object[] parameters)
    {
        return DbSet.FromSqlRaw(sql);
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

    #region ExecuteDelete

    public int ExecuteDelete(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = DbSet;

        // Retrieve the IQueryable representing the rows to delete
        query = query.Where(predicate);

        return query.ExecuteDelete();
    }

    public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = DbSet;

        // Retrieve the IQueryable representing the rows to delete
        query = query.Where(predicate);

        return await query.ExecuteDeleteAsync();
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
}