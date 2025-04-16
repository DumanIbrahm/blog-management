using BlogManagementProject.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogManagementProject
{
    public class User : IdentityUser
    {
        public ICollection<Blog>? Blogs { get; set; }
        public string? DisplayName { get; set; }
        public string? ProfileImagePath { get; set; }

    }
}
