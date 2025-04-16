using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogManagementProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using BlogManagementProject.ViewModels;

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
            var user = await _userRepository.GetByIdAsync(userId!);
            var blogs = await _blogRepository.GetAllByUserIdAsync(userId!);

            ViewBag.User = user;
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.NormalizedUserName = model.UserName.ToUpper(); // UserName değiştiyse normalize et
            user.Email = user.Email; // Email değiştirilmediyse koru

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/profiles");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImagePath = "/images/profiles/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


    }
}
