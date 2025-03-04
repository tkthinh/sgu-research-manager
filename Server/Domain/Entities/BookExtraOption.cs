using System;

namespace Domain.Entities
{
    public class BookExtraOption : BaseEntity
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; } // Option này thuộc về WorkType: Sách
        public virtual WorkType? WorkType { get; set; }
    }
}
