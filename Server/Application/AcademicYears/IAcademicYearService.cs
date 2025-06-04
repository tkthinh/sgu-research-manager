using Domain.Interfaces;
using Application.Shared.Services;

namespace Application.AcademicYears
{
    public interface IAcademicYearService : IGenericService<AcademicYearDto>
    {
        Task<AcademicYearDto> GetCurrentAcademicYear(CancellationToken cancellationToken = default);
        Task<AcademicYearDto?> GetCurrentAcademicYearAsync();
    }
}
