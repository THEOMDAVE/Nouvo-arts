using Microsoft.EntityFrameworkCore;
using NouvoStudio.Models;

namespace NouvoStudio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Spaces> Spaces { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<CustomizationRequest> CustomizationRequests { get; set; }
        public DbSet<Medium> Mediums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Image).IsRequired().HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Category
            modelBuilder.Entity<Spaces>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Image).IsRequired().HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });
            // Configure Artwork
            modelBuilder.Entity<Artwork>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Size).IsRequired().HasMaxLength(20);
                entity.Property(e => e.HeightFeet).IsRequired();
                entity.Property(e => e.WidthFeet).IsRequired();
                entity.Property(e => e.Medium).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Image).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CategoryIds).HasMaxLength(500);
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure BlogPost
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Excerpt).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Image).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Slug).IsUnique();
            });

            // Configure ContactMessage
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            });

            // Configure CustomizationRequest
            modelBuilder.Entity<CustomizationRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ArtworkType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Medium).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Size).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            });

            // Configure Medium
            modelBuilder.Entity<Medium>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Name).IsUnique();
            });
        }
    }
}
