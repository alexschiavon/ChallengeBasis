using Arch.DataAccessAdapter.Repositories.Common;
using BookDataAccessAdapter.Context;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Models.Report;
using BookDomain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookDataAccessAdapter.Repositories
{
    public class BookRepository(BookContext dbContext) : BaseRepository<Book, Guid, BasicFilter>(dbContext), IBookRepository
    {
        public override IQueryable<Book> ApplyFilters(IQueryable<Book> query, BasicFilter filter)
        {
            // query para filtrar por algum campos específico da entidade aqui aqui
            // Inclui as listas de BookAuthors e BookSubjects
            query = query
                .Include(b => b.BookPrices)
                .Include(b => b.BookAuthors)
                .Include(b => b.BookSubjects);
            return query;
        }

        public override async Task<Book> SaveOrUpdate(Book entity)
        {
            var existingEntity = _dbContext.Set<Book>()
                .Include(b => b.BookAuthors)
                .Include(b => b.BookSubjects)
                .Include(b => b.BookPrices)
                .FirstOrDefault(e => e.BookId == entity.BookId);

            if (existingEntity != null)
            {
                // Atualize as propriedades da entidade principal
                existingEntity.Title = entity.Title;
                existingEntity.Publisher = entity.Publisher;
                existingEntity.Edition = entity.Edition;
                existingEntity.PublicationYear = entity.PublicationYear;

                // Identificar entidades relacionadas que foram removidas
                var authorsToRemove = existingEntity.BookAuthors
                    .Where(re => !entity.BookAuthors.Any(ne => ne.AuthorCodAu == re.AuthorCodAu))
                    .ToList();

                var subjectsToRemove2 = existingEntity.BookSubjects
                    .Where(re => !entity.BookSubjects.Any(ne => ne.SubjectCodAs == re.SubjectCodAs))
                    .ToList();

                var pricesToRemove = existingEntity.BookPrices
                    .Where(re => !entity.BookPrices.Any(ne => ne.PurchaseTypeId == re.PurchaseTypeId))
                    .ToList();

                // Remover entidades relacionadas
                foreach (var relatedEntity in authorsToRemove)
                {
                    _dbContext.Remove(relatedEntity);
                }

                foreach (var relatedEntity in subjectsToRemove2)
                {
                    _dbContext.Remove(relatedEntity);
                }

                foreach (var relatedEntity in pricesToRemove)
                {
                    _dbContext.Remove(relatedEntity);
                }

                // Atualizar ou adicionar entidades relacionadas
                foreach (var relatedEntity in entity.BookAuthors)
                {
                    var existingRelatedEntity = existingEntity.BookAuthors
                        .FirstOrDefault(re => re.AuthorCodAu == relatedEntity.AuthorCodAu);

                    if (existingRelatedEntity != null)
                    {
                        // Atualize as propriedades da entidade relacionada existente
                        existingRelatedEntity.AuthorCodAu = relatedEntity.AuthorCodAu;
                        existingRelatedEntity.BookCodl = relatedEntity.BookCodl;

                        // Marque a entidade relacionada como modificada
                        _dbContext.Entry(existingRelatedEntity).State = EntityState.Modified;
                    }
                    else
                    {
                        // Adicione a nova entidade relacionada
                        existingEntity.BookAuthors.Add(relatedEntity);
                    }
                }

                foreach (var relatedEntity in entity.BookSubjects)
                {
                    var existingRelatedEntity = existingEntity.BookSubjects
                        .FirstOrDefault(re => re.SubjectCodAs == relatedEntity.SubjectCodAs);

                    if (existingRelatedEntity != null)
                    {
                        // Atualize as propriedades da entidade relacionada existente
                        existingRelatedEntity.SubjectCodAs = relatedEntity.SubjectCodAs;
                        existingRelatedEntity.BookCodl = relatedEntity.BookCodl;

                        // Marque a entidade relacionada como modificada
                        _dbContext.Entry(existingRelatedEntity).State = EntityState.Modified;
                    }
                    else
                    {
                        // Adicione a nova entidade relacionada
                        existingEntity.BookSubjects.Add(relatedEntity);
                    }
                }

                foreach (var relatedEntity in entity.BookPrices)
                {
                    var existingRelatedEntity = existingEntity.BookPrices
                        .FirstOrDefault(re => re.PurchaseTypeId == relatedEntity.PurchaseTypeId);

                    if (existingRelatedEntity != null)
                    {
                        // Atualize as propriedades da entidade relacionada existente
                        existingRelatedEntity.BookId = relatedEntity.BookId;
                        existingRelatedEntity.Price = relatedEntity.Price;
                        existingRelatedEntity.PurchaseTypeId = relatedEntity.PurchaseTypeId;

                        // Marque a entidade relacionada como modificada
                        _dbContext.Entry(existingRelatedEntity).State = EntityState.Modified;
                    }
                    else
                    {
                        // Adicione a nova entidade relacionada
                        existingEntity.BookPrices.Add(relatedEntity);
                    }
                }

                // Marque a entidade principal como modificada
                _dbContext.Entry(existingEntity).State = EntityState.Modified;
                _dbContext.Update(existingEntity);
            }
            else
            {
                // Adicione a nova entidade
                _dbContext.Add(entity);
            }

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public override async Task Delete(Book obj)
        {
            if (obj == null)
            {
                throw new Exception("Objeto não encontrado.");
            }
            var existingEntity = _dbContext.Set<Book>()
                            .Include(b => b.BookAuthors)
                            .Include(b => b.BookSubjects)
                            .Include(b => b.BookPrices)
                            .FirstOrDefault(e => e.BookId == obj.BookId); 
            if (existingEntity == null)
            {
                throw new Exception("Objeto não encontrado.");
            }


            foreach (var relatedEntity in existingEntity.BookSubjects)
            {
                _dbContext.Remove(relatedEntity);
            }

            foreach (var relatedEntity in existingEntity.BookAuthors)
            {
                _dbContext.Remove(relatedEntity);
            }

            _dbContext.Remove(existingEntity);
            await _dbContext.SaveChangesAsync();
        }

        protected override Guid GetEntityId(Book entity)
        {
            return entity.BookId;
        }

        public async Task<ICollection<BookDetails>> Report()
        {
            return await _dbContext.Set<BookDetails>().ToArrayAsync();
        }
    }
}
