using BlogManagementProject.Models;
using BlogManagementProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace BlogManagementProject.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Comment model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
                return RedirectToAction("Details", "Blog", new { id = model.BlogId });

            model.CreatedAt = DateTime.Now;
            model.UserId = _userManager.GetUserId(User);

            _context.Comments.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Blog", new { id = model.BlogId });
        }
    }
}
