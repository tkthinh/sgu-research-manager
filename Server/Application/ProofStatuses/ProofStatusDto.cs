namespace Application.ProofStatuses
{
    public class ProofStatusDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
