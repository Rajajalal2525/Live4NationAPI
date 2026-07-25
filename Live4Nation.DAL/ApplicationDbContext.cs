using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<WebsiteSetting> WebsiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.News)
                .WithOne(n => n.Category)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<News>()
                .HasMany(n => n.NewsImages)
                .WithOne(ni => ni.News)
                .HasForeignKey(ni => ni.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}