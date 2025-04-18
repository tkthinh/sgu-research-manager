using Domain.Entities;
using Domain.Enums;
using Application.Authors;

namespace Application.Works
{
    public interface IWorkCalculateService
    {
        int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor);
        Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid? authorRoleId, CancellationToken cancellationToken = default);
        bool ShouldRecalculateAuthorHours(UpdateAuthorRequestDto authorRequest, UpdateWorkRequestDto? workRequest);
        Task<Factor> FindFactorAsync(Guid workTypeId, Guid? workLevelId, Guid purposeId, Guid? authorRoleId, ScoreLevel? scoreLevel, CancellationToken cancellationToken = default);
    }
}
