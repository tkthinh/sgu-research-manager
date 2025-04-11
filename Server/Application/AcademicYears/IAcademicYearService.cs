using Domain.Interfaces;

namespace Application.AcademicYears
{
    public interface IAcademicYearService : IGenericService<AcademicYearDto>
    {
        Task<AcademicYearDto> GetCurrentAcademicYear(CancellationToken cancellationToken = default);
    }
}
