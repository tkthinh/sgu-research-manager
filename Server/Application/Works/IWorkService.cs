﻿using Domain.Interfaces;

namespace Application.Works
{
    public interface IWorkService : IGenericService<WorkDto>
    {
        Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default);
        Task<WorkDto> UpdateWorkByAdminAsync(Guid workId, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken = default);
        Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkWithAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default);
        Task RegisterWorkByAuthorAsync(Guid authorId, bool registered, CancellationToken cancellationToken = default);


        //Task<WorkDto> UpdateWorkAsync(Guid workId, UpdateWorkWithAuthorRequestDto request, Guid userId, bool isAdmin, CancellationToken cancellationToken = default);
    }
}