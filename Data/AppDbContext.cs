using BlogManagementProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementProject
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Technology", Description = "All about tech" },
            new Category { Id = 2, Name = "Lifestyle", Description = "Health & daily life" },
            new Category { Id = 3, Name = "Education", Description = "Learning and knowledge" }
            );

            builder.Entity<Blog>()
                   .HasOne(b => b.User)
                   .WithMany(u => u.Blogs)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Blog>()
                   .HasOne(b => b.Category)
                   .WithMany(c => c.Blogs)
                   .HasForeignKey(b => b.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                    .HasOne(c => c.Blog)
                    .WithMany(b => b.Comments)
                    .HasForeignKey(c => c.BlogId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                    .HasOne(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
