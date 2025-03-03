
namespace Domain.Entities
{
    public class WorkLevel : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Factor>? Factors { get; set; }
    }
}