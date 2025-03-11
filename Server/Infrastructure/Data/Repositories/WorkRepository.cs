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
    }
}
