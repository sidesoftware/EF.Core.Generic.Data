using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Paging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace EF.Core.Generic.Data.Interface
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);

        T FirstOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false);

        T Get(int id);

        Task<T> GetAsync(int id);

        int Count();
        int Count(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        TResult GetField<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector);

        Task<TResult> GetFieldAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector);

        IQueryable<T> GetSql(string sql, params object[] parameters);

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true, bool ignoreQueryFilters = false);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>,
                IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>,
                IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true, bool ignoreQueryFilters = false);

        IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true, bool ignoreQueryFilters = false);

        IPaginate<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true) where TResult : class;

        Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true, bool ignoreQueryFilters = false,
            CancellationToken cancellationToken = default);

        Task<IPaginate<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector = null,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true,
            bool ignoreQueryFilters = false,
            CancellationToken cancellationToken = default) where TResult : class;

        T Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);

        ValueTask<EntityEntry<T>> AddAsync(T entity,
            CancellationToken cancellationToken = default);

        Task AddAsync(params T[] entities);

        Task AddAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default);

        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);

        void Remove(T entity);
        void Remove(params T[] entities);
        void Remove(IEnumerable<T> entities);

        int ExecuteDelete(Expression<Func<T, bool>> predicate);
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);
    }
}