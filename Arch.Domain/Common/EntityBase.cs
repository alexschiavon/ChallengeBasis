
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Arch.Domain.Contracts.Entity;

namespace Arch.Domain.Common
{
    public class EntityBase<T> : IEntity<T>
    {
        [Key]
        [Column("id")]
        public T Id { get; set; }
        [Column("isactive")]
        [Required]
        public bool IsActive { get; set; }
        [Column("isdeleted")]
        [Required]
        public bool IsDeleted { get; set; }
        [Column("datetimeadd")]
        public DateTime? DateTimeAdd { get; set; }
        [Column("datetimeupd")]
        public DateTime? DateTimeUpd { get; set; }

        /// <summary>
        /// DefaultEquals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IEntity<T>? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            var otherB = other as IEntity<T>;
            return otherB != null && Id.Equals(otherB.Id);
        }
    }
}
