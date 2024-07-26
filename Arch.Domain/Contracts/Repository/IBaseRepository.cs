using Arch.Domain.Adapters.Helper;

namespace Arch.Domain.Contracts.Repository
{
    public interface IBaseRepository<T, TId, TFilter> : IDisposable
    {
        Task<T> FindById(TId id);
        Task<T> SaveOrUpdate(T obj);
        Task Delete(T obj);
        Task<Metadata<T, TFilter>> FindByFilterAsync(Metadata<T, TFilter> metadata);
        IQueryable<T> ApplyFilters(IQueryable<T> query, TFilter filter);
    }
}
