using BlogManagementProject.Models;
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

        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            var blogs = await _blogRepository.GetAllWithCategoryAsync();
            var categories = await _categoryRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                blogs = blogs.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                         b.Content.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                blogs = blogs.Where(b => b.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;

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
            Console.WriteLine("FORM SUBMITTED");

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Model error on {key}: {error.ErrorMessage}");
                    }
                }

                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                return View(blog);
            }

            Console.WriteLine($"ADDING BLOG: {blog.Title}, CategoryId: {blog.CategoryId}, UserId: {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blog.UserId = userId!;
            blog.PublishedDate = DateTime.Now;

            await _blogRepository.AddAsync(blog);
            await _blogRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blog.UserId != userId) return Forbid();

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(blog);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                return View(blog);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blog.UserId = userId!;
            blog.PublishedDate = DateTime.Now;

            _blogRepository.Update(blog);
            await _blogRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blog.UserId != userId) return Forbid();

            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound();

            _blogRepository.Remove(blog);
            await _blogRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchBlogs(string query)
        {
            var blogs = await _blogRepository.GetAllWithCategoryAsync();

            if (!string.IsNullOrEmpty(query))
            {
                blogs = blogs.Where(b =>
                    b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    b.Content.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            var result = blogs.Select(b => new
            {
                b.Id,
                b.Title,
                Content = b.Content.Length > 100 ? b.Content.Substring(0, 100) + "..." : b.Content,
                User = b.User?.UserName,
                Date = b.PublishedDate.ToShortDateString()
            });

            return Json(result);
        }

    }
}
