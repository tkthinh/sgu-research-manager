using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository
    {
        private readonly DbContext _dbContext;

        public WorkRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Work>> GetWorksWithAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Work>()
                .Include(w => w.WorkType)
                .Include(w => w.WorkLevel)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.AuthorRole)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Purpose)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.SCImagoField)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Field)
                .ToListAsync(cancellationToken);
        }

        public async Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Work>()
                .Include(w => w.WorkType)
                .Include(w => w.WorkLevel)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.AuthorRole)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Purpose)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.SCImagoField)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Field)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Work>> FindWorksWithAuthorsAsync(string title, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Work>()
                .Where(w => w.Title.Contains(title))
                .Include(w => w.WorkType)
                .Include(w => w.WorkLevel)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.AuthorRole)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Purpose)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.SCImagoField)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Field)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Work>()
                .Include(w => w.WorkType)
                .Include(w => w.WorkLevel)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.AuthorRole)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Purpose)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.SCImagoField)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Field)
                .Where(w => w.Authors.Any(a => a.User.DepartmentId == departmentId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Work>()
                .Where(w => ids.Contains(w.Id))
                .Include(w => w.WorkType)
                .Include(w => w.WorkLevel)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.AuthorRole)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Purpose)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.SCImagoField)
                .Include(w => w.Authors)
                    .ThenInclude(a => a.Field)
                .ToListAsync(cancellationToken);
        }
    }
}