namespace Arch.Domain.Contracts.Entity
{
    public interface IEntity<T> : IEquatable<IEntity<T>>
    {
        T Id { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        DateTime? DateTimeAdd { get; set; }
        DateTime? DateTimeUpd { get; set; }
    }
}
