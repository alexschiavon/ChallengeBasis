using Microsoft.EntityFrameworkCore;
using Arch.DataAccessAdapter.Extensions;
using Arch.Domain.Adapters.Helper;

namespace Arch.DataAccessAdapter.Repositories.Common
{
    public abstract class BaseRepository<TEntity, TId, TFilter> where TEntity : class
    {
        protected readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, TFilter filter);

        public async Task<Metadata<TEntity, TFilter>> FindByFilterAsync(Metadata<TEntity, TFilter> metadata)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (metadata?.Pagination == null)
            {
                metadata.Pagination = new Pagination();
            }
            if (metadata?.SortedBy == null)
            {
                metadata.SortedBy = new SortedBy();
            }

            var limit = 10;
            if (metadata?.Pagination?.Limit > 0)
            {
                limit = metadata.Pagination.Limit;
            }
            query = ApplyFilters(query, metadata.Custom);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / limit);

            // Update pagination metadata (common logic)
            metadata.Pagination.TotalCount = totalCount;
            metadata.Pagination.PageCount = totalPages;

            metadata.Content = await query
                .OrderBy(metadata.SortedBy.Field, metadata.SortedBy.Order.ToUpper() == "ASC" ? true : false)
                .Skip(metadata.Pagination.CurrentPage * limit)
                .Take(limit)
                .ToListAsync();

            return metadata;
        }

        public async Task<TEntity> FindById(TId id)
        {
            return await _dbContext.FindAsync<TEntity>(id);
        }

        /// <summary>
        /// Busca o ID da entidade independente da propriedade utilizada na classe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract TId GetEntityId(TEntity entity);


        public virtual async Task<TEntity> SaveOrUpdate(TEntity obj)
        {
            TEntity? existingEntity = null;

            if (GetEntityId(obj) != null) // Check for non-empty ID
            {
                existingEntity = await FindById(GetEntityId(obj));
            }

            if (existingEntity != null)
            {
                _dbContext.Update(obj);
            }
            else
            {   
                await _dbContext.AddAsync(obj);
            }

            await _dbContext.SaveChangesAsync();

            return obj;
        }

        public virtual async Task Delete(TEntity obj)
        {
            if (obj == null)
            {
                throw new Exception("Objeto não encontrado.");
            }

            var existingEntity = await FindById(GetEntityId(obj));
            if (existingEntity == null)
            {
                throw new Exception("Objeto não encontrado.");
            }

            _dbContext.Remove(existingEntity);
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
