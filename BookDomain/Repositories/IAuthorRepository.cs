using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Models;

namespace BookDomain.Repositories
{
    public interface IAuthorRepository : IBaseRepository<Author, Guid, BasicFilter>
    {
        
    }
}
