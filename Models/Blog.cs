using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogManagementProject
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string? ImagePath { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign key 
        public User? User { get; set; }

        [Required]
        public int CategoryId { get; set; } // Foreign key
        public Category? Category { get; set; }
    }
}
