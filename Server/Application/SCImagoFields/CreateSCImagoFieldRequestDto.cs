namespace Application.SCImagoFields
{
    public class CreateSCImagoFieldRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
