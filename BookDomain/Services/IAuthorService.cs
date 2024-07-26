using Arch.Domain.Contracts.Service;
using BookDomain.Filters;
using BookDomain.Models;

namespace BookDomain.Services
{
    public interface IAuthorService : IBaseService<Author, Guid, BasicFilter>
    {
    }
}
