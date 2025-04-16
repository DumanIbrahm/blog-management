using System.ComponentModel.DataAnnotations;

namespace BlogManagementProject.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Blog>? Blogs { get; set; }
    }
}
