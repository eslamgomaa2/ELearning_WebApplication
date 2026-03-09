using E_Learning.Repository.Data;
using E_Learning.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Repository.Repositories.GenericesRepositories
{
    public class GenericRepository <TEntity,TKey> : IGenericRepository <TEntity,TKey> where TEntity : class
    {
        protected readonly ELearningDbContext   _context;
        protected readonly DbSet<TEntity> _set;

        public GenericRepository(ELearningDbContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
            => await _set.FindAsync([id], ct);

        public virtual async Task<TEntity?> GetByIdAsync(
            TKey id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include,
            CancellationToken ct = default)
        {
            var keyName = _context.Model.FindEntityType(typeof(TEntity))!
                                   .FindPrimaryKey()!.Properties[0].Name;

            var param = Expression.Parameter(typeof(TEntity), "e");
            var body = Expression.Equal(
                Expression.Property(param, keyName),
                Expression.Constant(id));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);

            return await include(_set.AsNoTracking())
                            .FirstOrDefaultAsync(lambda, ct);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
            => await _set.AsNoTracking().ToListAsync(ct);

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include,
            CancellationToken ct = default)
            => await include(_set.AsNoTracking()).ToListAsync(ct);

        public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _set.AsNoTracking().Where(predicate).ToListAsync(ct);

        public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken ct = default)
        {
            IQueryable<TEntity> query = _set.AsNoTracking().Where(predicate);
            if (include != null) query = include(query);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync(ct);
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _set.AsNoTracking().FirstOrDefaultAsync(predicate, ct);

        public virtual async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include,
            CancellationToken ct = default)
            => await include(_set.AsNoTracking()).FirstOrDefaultAsync(predicate, ct);

        public virtual async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _set.AnyAsync(predicate, ct);

        public virtual async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default)
            => predicate is null
                ? await _set.CountAsync(ct)
                : await _set.CountAsync(predicate, ct);

   

        public virtual async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken ct = default)
        {
            IQueryable<TEntity> query = _set.AsNoTracking();

            if (predicate != null) query = query.Where(predicate);
            if (include != null) query = include(query);

            int totalCount = await query.CountAsync(ct);

            if (orderBy != null) query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }


        public virtual async Task<IReadOnlyList<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            IQueryable<TEntity> query = _set.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            return await query.Select(selector).ToListAsync(ct);
        }


        public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
            => await _set.AddAsync(entity, ct);

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
            => await _set.AddRangeAsync(entities, ct);

        public virtual void Update(TEntity entity)
            => _set.Update(entity);

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
            => _set.UpdateRange(entities);

        public virtual void Remove(TEntity entity)
            => _set.Remove(entity);

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
            => _set.RemoveRange(entities);

     
        public IQueryable<TEntity> Query() => _set.AsQueryable();
        public IQueryable<TEntity> QueryNoTracking() => _set.AsNoTracking();
    }
}
