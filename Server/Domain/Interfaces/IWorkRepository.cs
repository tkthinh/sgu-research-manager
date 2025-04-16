using Domain.Entities;
using Domain.Enums;
using System;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        // Phương thức cơ bản sử dụng bởi hệ thống filter 
        Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Work>> GetWorksWithAuthorsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        
        // Phương thức tổng quát để lấy công trình với predicate tùy chỉnh
        Task<IEnumerable<Work>> GetWorksByFilterAsync(Expression<Func<Work, bool>> predicate, CancellationToken cancellationToken = default);
    }
}