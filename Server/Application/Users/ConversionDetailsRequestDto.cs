namespace Application.Users
{
    public class ConversionDetailsRequestDto
    {
        public ConversionItemRequestDto? DutyHourConversion { get; set; }
        public ConversionItemRequestDto? OverLimitConversion { get; set; }
        public ConversionItemRequestDto? ResearchProductConversion { get; set; }
        public int TotalWorks { get; set; }
        public decimal TotalCalculatedHours { get; set; }
    }
}
