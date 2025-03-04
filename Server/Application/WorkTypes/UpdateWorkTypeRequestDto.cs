namespace Application.WorkTypes
{
    public class UpdateWorkTypeRequestDto
    {
        public required string Name { get; set; }
        public bool HasExtraOption { get; set; }
    }
}
