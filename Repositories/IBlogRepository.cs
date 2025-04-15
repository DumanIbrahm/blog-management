using BlogManagementProject.Models;

namespace BlogManagementProject.Repositories.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetAllWithCategoryAsync();
        Task<Blog?> GetByIdWithDetailsAsync(int id);
    }
}
