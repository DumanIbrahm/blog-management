using BlogManagementProject.Models;

namespace BlogManagementProject.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByIdAsync(string id);

    }
}
