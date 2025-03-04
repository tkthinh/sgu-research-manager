namespace Application.WorkTypes
{
    public class CreateWorkTypeRequestDto
    {
        public required string Name { get; set; }
        public bool HasExtraOption { get; set; }
    }
}
