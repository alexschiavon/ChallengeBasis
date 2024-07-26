using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Models;

namespace BookDomain.Repositories
{
    public interface IBookRepository : IBaseRepository<Book, Guid, BasicFilter>
    {
        
    }

}
