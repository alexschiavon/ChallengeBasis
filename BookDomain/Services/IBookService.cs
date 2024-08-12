using Arch.Domain.Contracts.Service;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Models.Report;

namespace BookDomain.Services
{
    public interface IBookService : IBaseService<Book, Guid, BookFilter>
    {
        Task<ICollection<BookDetails>> Report();
    }
}
