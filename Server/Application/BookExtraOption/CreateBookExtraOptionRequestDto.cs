namespace Application.BookExtraOptions
{
    public class CreateBookExtraOptionRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
