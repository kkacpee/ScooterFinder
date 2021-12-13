namespace ServerApi.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Query;
    using ServerApi.Interfaces.Repositories;
    using ServerApi.Persistance;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class GenericRepository<TEntity> : BaseRepository, IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(AppDbContext context) : base(context) { }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<TEntity, dynamic> distinctBy = null)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            var result = await query.ToListAsync(cancellationToken);

            return distinctBy != null ? result.DistinctBy(distinctBy) : result;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? take = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;
            query = query.Where(predicate);
            query = take.HasValue ? query.Take(take.Value) : query;

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null,
            int? take = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            var selectedQuery = query.Where(predicate).Select(select);

            selectedQuery = orderBy?.Invoke(selectedQuery) ?? selectedQuery;
            selectedQuery = take.HasValue ? selectedQuery.Take(take.Value) : selectedQuery;

            return await selectedQuery.ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync<TId>(
            TId id,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (id == null || (id is int idInt && idInt == 0)) throw new ArgumentNullException($"Parameter {nameof(id)} cannot be null or zero.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            var lambda = CreateFindByPrimaryKeyLambda(id);

            var result = await query.SingleOrDefaultAsync(lambda, cancellationToken);
            if (result == null) throw new NullReferenceException($"Object of type {typeof(TEntity).Name} not found.");

            return result;
        }

        public virtual async Task<TResult> GetFirstAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            var result = query.Where(predicate);

            if (!await result.AnyAsync(cancellationToken)) throw new NullReferenceException($"Object of type {typeof(TEntity).Name} not found.");

            return await result.Select(select).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            return await query.Where(predicate).Select(select).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult> GetSingleAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            var result = query.Where(predicate);

            if (!await result.AnyAsync(cancellationToken)) throw new NullReferenceException($"Object of type {typeof(TEntity).Name} not found.");

            return await result.Select(select).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult> GetSingleOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            return await query.Where(predicate).Select(select).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult> GetLastAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            return await query.Where(predicate).Select(select).LastAsync(cancellationToken);
        }

        public virtual async Task<TResult> GetLastOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new ArgumentNullException($"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            return await query.Where(predicate).Select(select).LastOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().AnyAsync(cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");

            return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().CountAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null) throw new ArgumentNullException($"Parameter {nameof(predicate)} cannot be null.");

            return await _context.Set<TEntity>().CountAsync(predicate, cancellationToken);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entity == null) throw new ArgumentNullException($"Parameter {nameof(entity)} cannot be null.");

            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entities == null) throw new ArgumentNullException($"Parameter {nameof(entities)} cannot be null.");

            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entity == null) throw new ArgumentNullException($"Parameter {nameof(entity)} cannot be null.");

            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entities == null) throw new ArgumentNullException($"Parameter {nameof(entities)} cannot be null.");

            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entity == null) throw new ArgumentNullException($"Parameter {nameof(entity)} cannot be null.");

            if (entity.GetType().GetProperties().All(x => x.Name != "IsDeleted"))
                throw new InvalidOperationException($"Entity {typeof(TEntity).Name} doesn't contain IsDeleted property. If you want to delete entities permanently use DeletePermanentlyAsync method.");

            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
            await UpdateAsync(entity, cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entities == null) throw new ArgumentNullException($"Parameter {nameof(entities)} cannot be null.");

            if (typeof(TEntity).GetProperties().All(x => x.Name != "IsDeleted"))
                throw new InvalidOperationException($"Entity {typeof(TEntity).Name} doesn't contain IsDeleted property. If you want to delete entities permanently use DeletePermanentlyAsync method.");

            foreach (var entity in entities)
            {
                entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
            }
            await UpdateManyAsync(entities, cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeleteByIdAsync<TId>(TId id, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (id == null || (id is int idInt && idInt == 0)) throw new ArgumentNullException($"Parameter {nameof(id)} cannot be null or zero.");

            var entity = await GetByIdAsync(id, cancellationToken);

            await DeleteAsync(entity, cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeletePermanentlyAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entity == null) throw new ArgumentNullException($"Parameter {nameof(entity)} cannot be null.");

            _context.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeletePermanentlyByIdAsync<TId>(TId id, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (id == null || (id is int idInt && idInt == 0)) throw new ArgumentNullException($"Parameter {nameof(id)} cannot be null or zero.");

            var entity = await GetByIdAsync(id, cancellationToken);

            await DeletePermanentlyAsync(entity, cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeleteManyPermanentlyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entities == null) throw new ArgumentNullException($"Parameter {nameof(entities)} cannot be null.");

            _context.RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        protected Expression<Func<TEntity, bool>> CreateFindByPrimaryKeyLambda<TId>(TId id)
        {
            var pkPropertyName = GetPrimaryKeyPropertyName();
            var propertyInfo = typeof(TEntity).GetProperty(pkPropertyName);

            if (propertyInfo?.PropertyType != typeof(TId)) throw new InvalidOperationException($"Invalid type of {nameof(id)} argument.");

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.MakeMemberAccess(parameter, propertyInfo);
            var constant = Expression.Constant(id, id.GetType());
            var equation = Expression.Equal(member, constant);

            return Expression.Lambda<Func<TEntity, bool>>(equation, parameter);
        }

        protected virtual string GetPrimaryKeyPropertyName() => _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

        private void DetachAll(bool detachAll)
        {
            if (detachAll)
            {
                var entityEntries = _context.ChangeTracker.Entries().ToArray();

                foreach (EntityEntry entityEntry in entityEntries)
                {
                    entityEntry.State = EntityState.Detached;
                }
            }
        }
    }
}
