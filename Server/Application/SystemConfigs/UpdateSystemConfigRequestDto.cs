namespace Application.SystemConfigs
{
    public class UpdateSystemConfigRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
    }
}
