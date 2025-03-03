namespace Domain.Entities
{
   public class Purpose : BaseEntity
   {
        public required string Name { get; set; }
        public ICollection<Author>? Authors { get; set; }
        public ICollection<Factor>? Factors { get; set; }
    }
}
