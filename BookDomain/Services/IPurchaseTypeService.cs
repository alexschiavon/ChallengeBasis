using Arch.Domain.Contracts.Service;
using BookDomain.Filters;
using BookDomain.Models;

namespace BookDomain.Services
{
    public interface IPurchaseTypeService : IBaseService<PurchaseType, Guid, BasicFilter>
    {
    }
}
