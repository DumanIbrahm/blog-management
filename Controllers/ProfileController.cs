using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogManagementProject.Repositories.Interfaces;

namespace BlogManagementProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlogRepository _blogRepository;

        public ProfileController(IUserRepository userRepository, IBlogRepository blogRepository)
        {
            _userRepository = userRepository;
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetByIdAsync(userId!);
            var blogs = await _blogRepository.GetAllByUserIdAsync(userId!);

            ViewBag.User = user;
            return View(blogs);
        }
    }
}
