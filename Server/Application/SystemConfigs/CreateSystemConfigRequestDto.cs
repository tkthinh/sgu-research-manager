namespace Application.SystemConfigs
{
    public class CreateSystemConfigRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
    }
}
