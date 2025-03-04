
namespace Domain.Entities
{
    public class WorkLevel : BaseEntity
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
        public virtual WorkType? WorkType { get; set; }
        public ICollection<Factor>? Factors { get; set; }
    }
}