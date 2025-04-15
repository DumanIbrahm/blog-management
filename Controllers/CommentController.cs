using BlogManagementProject.Models;
using BlogManagementProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogManagementProject.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
            }

            comment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            comment.CreatedAt = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }
    }
}
