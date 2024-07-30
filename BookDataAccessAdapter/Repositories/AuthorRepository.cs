using Arch.DataAccessAdapter.Repositories.Common;
using BookDataAccessAdapter.Context;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Repositories;

namespace BookDataAccessAdapter.Repositories
{
    public class AuthorRepository(BookContext dbContext) : BaseRepository<Author, Guid, BasicFilter>(dbContext), IAuthorRepository
    {
        public override IQueryable<Author> ApplyFilters(IQueryable<Author> query, BasicFilter filter)
        {
            // query para filtrar por algum campos específico da entidade aqui aqui
            return query;
        }

        protected override Guid GetEntityId(Author entity)
        {
            return entity.AuthorId;
        }

        public override async Task<Author> SaveOrUpdate(Author entity)
        {
            var existingEntity = _dbContext.Set<Author>().Find(entity.AuthorId);

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
