using System;

namespace Application.BookExtraOptions
{
    public class BookExtraOptionDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
