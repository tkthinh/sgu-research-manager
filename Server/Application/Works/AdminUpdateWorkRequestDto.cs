using Domain.Enums;

namespace Application.Works
{
    public class AdminUpdateWorkRequestDto
    {
        public int FinalWorkHour { get; set; }
        public ProofStatus ProofStatus { get; set; }
    }
}