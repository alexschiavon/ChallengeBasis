using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;

namespace Arch.Domain.Contracts.Service
{
    public interface IBaseService<T, IdT, TFilter>
    {
        Task<bool> Delete(T o);
        void Dispose();
        Task<T> FindById(IdT id);
        Task<Metadata<T, TFilter>> FindByFilter(Metadata<T, TFilter> filter);
        IBaseRepository<T, IdT, TFilter> Instance();
        Task<T> SaveOrUpdate(T o);
        void Validate(T o);
    }
}
