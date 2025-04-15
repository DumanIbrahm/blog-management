using BlogManagementProject.Models;
using BlogManagementProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementProject.Repositories.Implementations
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetAllWithCategoryAsync()
        {
            return await _context.Blogs
                                 .Include(b => b.Category)
                                 .Include(b => b.User)
                                 .ToListAsync();
        }

        public async Task<Blog?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Blogs
                                 .Include(b => b.Category)
                                 .Include(b => b.User)
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Blog?> GetByIdWithCommentsAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.User)
                .Include(b => b.Category)
                .Include(b => b.Comments!)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}
