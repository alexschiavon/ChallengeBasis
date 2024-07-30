using Arch.DataAccessAdapter.Repositories.Common;
using BookDataAccessAdapter.Context;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Repositories;

namespace BookDataAccessAdapter.Repositories
{
    public class PurchaseTypeRepository(BookContext dbContext) : BaseRepository<PurchaseType, Guid, BasicFilter>(dbContext), IPurchaseTypeRepository
    {
        public override IQueryable<PurchaseType> ApplyFilters(IQueryable<PurchaseType> query, BasicFilter filter)
        {
            // query para filtrar por algum campos específico da entidade aqui aqui
            return query;
        }

        protected override Guid GetEntityId(PurchaseType entity)
        {
            return entity.PurchaseTypeId;
        }

        public override async Task<PurchaseType> SaveOrUpdate(PurchaseType entity)
        {
            var existingEntity = _dbContext.Set<PurchaseType>().Find(entity.PurchaseTypeId);

            if (existingEntity != null)
            {
                // Atualize as propriedades da entidade principal
                existingEntity.Name = entity.Name;

                _dbContext.Update(existingEntity);
            }
            else
            {
                await _dbContext.AddAsync(entity);
            }

            await _dbContext.SaveChangesAsync();

            return entity;
        }

    }
}
