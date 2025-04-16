using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogManagementProject.Models;

namespace BlogManagementProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.BlogCount = await _context.Blogs.CountAsync();
            ViewBag.UserCount = await _userManager.Users.CountAsync();
            ViewBag.CategoryCount = await _context.Categories.CountAsync();
            ViewBag.CommentCount = await _context.Comments.CountAsync();

            return View();
        }

        public async Task<IActionResult> Blogs()
        {
            var blogs = await _context.Blogs.Include(b => b.User).Include(b => b.Category).ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Comments()
        {
            var comments = await _context.Comments.Include(c => c.Blog).Include(c => c.User).ToListAsync();
            return View(comments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Blogs));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        public IActionResult GetWeeklyBlogStats()
        {
            var oneWeekAgo = DateTime.Today.AddDays(-6);
            var data = _context.Blogs
                .Where(b => b.PublishedDate >= oneWeekAgo)
                .GroupBy(b => b.PublishedDate.Date)
                .Select(g => new
                {
                    date = g.Key.ToString("yyyy-MM-dd"),
                    count = g.Count()
                })
                .ToList();

            var result = Enumerable.Range(0, 7)
                .Select(i => oneWeekAgo.AddDays(i))
                .Select(date => new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    count = data.FirstOrDefault(d => d.date == date.ToString("yyyy-MM-dd"))?.count ?? 0
                });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogCountByCategory()
        {
            var data = await _context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    count = c.Blogs.Count()
                })
                .ToListAsync();

            return Json(data);
        }

    }
}
