using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Models.Report;

namespace BookDomain.Repositories
{
    public interface IBookRepository : IBaseRepository<Book, Guid, BasicFilter>
    {
        public Task<ICollection<BookDetails>> Report();
    }

}
