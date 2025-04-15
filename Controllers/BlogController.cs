using BlogManagementProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogManagementProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public BlogController(
            IBlogRepository blogRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogRepository.GetAllWithCategoryAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogRepository.GetByIdWithDetailsAsync(id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                return View(blog);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blog.UserId = userId!;
            blog.PublishedDate = DateTime.Now;

            await _blogRepository.AddAsync(blog);
            await _blogRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
