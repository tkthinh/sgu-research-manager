namespace Application.SCImagoFields
{
    public class UpdateSCImagoFieldRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
