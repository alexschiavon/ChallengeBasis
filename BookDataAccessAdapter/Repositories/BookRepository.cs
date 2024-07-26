using Arch.DataAccessAdapter.Repositories.Common;
using BookDataAccessAdapter.Context;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Repositories;

namespace BookDataAccessAdapter.Repositories
{
    public class BookRepository(BookContext dbContext) : BaseRepository<Book, Guid, BasicFilter>(dbContext), IBookRepository
    {
        public override IQueryable<Book> ApplyFilters(IQueryable<Book> query, BasicFilter filter)
        {
            // query para filtrar por algum campos específico da entidade aqui aqui
            return query;
        }

        protected override Guid GetEntityId(Book entity)
        {
            return entity.BookId;
        }
    }
}
