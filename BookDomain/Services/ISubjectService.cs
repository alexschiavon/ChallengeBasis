using Arch.Domain.Contracts.Service;
using BookDomain.Filters;
using BookDomain.Models;

namespace BookDomain.Services
{
    public interface ISubjectService : IBaseService<Subject, Guid, BasicFilter>
    {
    }
}
