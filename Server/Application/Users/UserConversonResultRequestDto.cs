namespace Application.Users
{
    public class UserConversionResultRequestDto
    {
        public Guid UserId { get; set; }
        public required string UserName { get; set; }
        public ConversionDetailsRequestDto? ConversionResults { get; set; }
    }
}
