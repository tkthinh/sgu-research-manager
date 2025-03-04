namespace Application.BookExtraOptions
{
    public class UpdateBookExtraOptionRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
