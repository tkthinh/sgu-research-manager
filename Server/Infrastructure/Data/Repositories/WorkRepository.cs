using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository
    {
        public WorkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Work>> GetWorksWithAuthorsAsync()
        {
            return await context.Set<Work>()
                .Include(w => w.Authors)
                .ToListAsync();
        }

        public async Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id)
        {
            return await context.Set<Work>()
                .Include(w => w.Authors)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Work>> FindWorksWithAuthorsAsync(string title)
        {
            return await context.Set<Work>()
                .Include(w => w.Authors)
                .Where(w => w.Title.Contains(title))
                .ToListAsync();
        }

        public async Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await context.Works
                .Include(w => w.Authors) // Bao gồm thông tin Authors để trả về đầy đủ WorkDto
                .Where(w => w.Authors.Any(a =>
                    context.Users
                        .Where(u => u.DepartmentId == departmentId)
                        .Select(u => u.Id)
                        .Contains(a.UserId)))
                .Distinct()
                .ToListAsync(cancellationToken);
        }

    }
}
