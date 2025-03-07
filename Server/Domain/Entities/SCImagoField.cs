namespace Domain.Entities
{
    public class SCImagoField : BaseEntity
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }

        public virtual WorkType? WorkType { get; set; }
    }
}