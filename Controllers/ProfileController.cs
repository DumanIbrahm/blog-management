using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogManagementProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using BlogManagementProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using BlogManagementProject.Models;

namespace BlogManagementProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(IUserRepository userRepository, IBlogRepository blogRepository, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _blogRepository = blogRepository;
            _userManager = userManager;
            _environment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.Include(u => u.Blogs).FirstOrDefaultAsync(u => u.Id == userId);

            ViewBag.User = user;

            List<Blog> blogs = new List<Blog>();

            if (user?.Blogs != null)
            {
                blogs = user.Blogs.OrderByDescending(b => b.PublishedDate).ToList();
            }

            return View(blogs);
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            var model = new UpdateProfileViewModel
            {
                UserName = user.UserName,
                DisplayName = user.UserName, // veya başka bir `DisplayName` alanı varsa onu al
                ExistingImagePath = user.ProfileImagePath
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return NotFound();

            user.DisplayName = model.DisplayName;
            user.UserName = model.UserName;

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImagePath = "/images/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }


    }
}
