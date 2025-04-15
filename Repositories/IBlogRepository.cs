using BlogManagementProject.Models;

namespace BlogManagementProject.Repositories.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetAllWithCategoryAsync();
        Task<Blog?> GetByIdWithDetailsAsync(int id);
        Task<Blog?> GetByIdWithCommentsAsync(int id);
        Task<IEnumerable<Blog>> GetAllByUserIdAsync(string userId);


    }
}
