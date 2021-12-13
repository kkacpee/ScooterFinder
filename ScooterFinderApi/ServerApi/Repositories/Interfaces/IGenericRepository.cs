namespace ServerApi.Interfaces.Repositories
{
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<TEntity, dynamic> distinctBy = null);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? take = null);

        Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null,
            int? take = null);

        Task<TEntity> GetByIdAsync<TId>(
            TId id,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TResult> GetFirstAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TResult> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TResult> GetSingleAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TResult> GetSingleOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TResult> GetLastAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TResult> GetLastOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<bool> AnyAsync(CancellationToken cancellationToken);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<int> CountAsync(CancellationToken cancellationToken);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false);

        Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false);

        Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false);

        Task DeleteByIdAsync<TId>(TId id, CancellationToken cancellationToken, bool detachAll = false);

        Task DeletePermanentlyAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false);

        Task DeletePermanentlyByIdAsync<TId>(TId id, CancellationToken cancellationToken, bool detachAll = false);

        Task DeleteManyPermanentlyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false);

        Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false);
    }
}
