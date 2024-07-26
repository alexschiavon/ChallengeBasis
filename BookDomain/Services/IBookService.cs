using Arch.Domain.Contracts.Service;
using BookDomain.Filters;
using BookDomain.Models;

namespace BookDomain.Services
{
    public interface IBookService : IBaseService<Book, Guid, BasicFilter>
    {
    }
}
