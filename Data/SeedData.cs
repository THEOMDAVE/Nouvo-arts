using Microsoft.EntityFrameworkCore;
using NouvoStudio.Models;

namespace NouvoStudio.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.Categories.AnyAsync())
            {
                return; // Data already seeded
            }

            // Seed Categories
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Abstract",
                    Description = "Non-representational art that explores color, form, and composition",
                    Image = "https://images.unsplash.com/photo-1545239351-1141bd82e8a6?q=80&w=1600&auto=format&fit=crop",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Landscape",
                    Description = "Artworks depicting natural scenery and outdoor environments",
                    Image = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?q=80&w=1600&auto=format&fit=crop",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Figurative",
                    Description = "Artworks that represent the human figure and form",
                    Image = "https://images.unsplash.com/photo-1549880338-65ddcdfd017b?q=80&w=1600&auto=format&fit=crop",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Minimal",
                    Description = "Clean, simple compositions with minimal elements",
                    Image = "https://images.unsplash.com/photo-1526318472351-c75fcf070305?q=80&w=1600&auto=format&fit=crop",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Modern",
                    Description = "Contemporary art reflecting current artistic trends",
                    Image = "https://images.unsplash.com/photo-1545235617-9465d2a55698?q=80&w=1600&auto=format&fit=crop",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Botanical",
                    Description = "Artworks featuring plants, flowers, and natural elements",
                    Image = "https://images.unsplash.com/photo-1501004318641-b39e6451bec6?q=80&w=1600&auto=format&fit=crop",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Get the created categories for foreign key relationships
            var abstractCategory = await context.Categories.FirstAsync(c => c.Name == "Abstract");
            var landscapeCategory = await context.Categories.FirstAsync(c => c.Name == "Landscape");
            var figurativeCategory = await context.Categories.FirstAsync(c => c.Name == "Figurative");
            var minimalCategory = await context.Categories.FirstAsync(c => c.Name == "Minimal");
            var modernCategory = await context.Categories.FirstAsync(c => c.Name == "Modern");
            var botanicalCategory = await context.Categories.FirstAsync(c => c.Name == "Botanical");

            // Seed Artworks
            var artworks = new List<Artwork>
            {
                new Artwork
                {
                    Name = "Azure Bloom",
                    Code = "ART-001",
                    CategoryIds = abstractCategory.Id.ToString(),
                    Size = "Small",
                    Medium = "Acrylic",
                    Price = 220,
                    Description = "Calming abstract fields with soft gradients.",
                    Image = "https://images.unsplash.com/photo-1526318472351-c75fcf070305?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "Horizon Lines",
                    Code = "ART-002",
                    CategoryIds = landscapeCategory.Id.ToString(),
                    Size = "Large",
                    Medium = "Oil",
                    Price = 980,
                    Description = "Expansive seascape with layered textures.",
                    Image = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "Silent Figure",
                    Code = "ART-003",
                    CategoryIds = figurativeCategory.Id.ToString(),
                    Size = "Medium",
                    Medium = "Mixed Media",
                    Price = 640,
                    Description = "Minimal portrait evoking quiet introspection.",
                    Image = "https://images.unsplash.com/photo-1452587925148-ce544e77e70d?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "Mono Study",
                    Code = "ART-004",
                    CategoryIds = minimalCategory.Id.ToString(),
                    Size = "Small",
                    Medium = "Acrylic",
                    Price = 190,
                    Description = "Balanced composition with subdued palette.",
                    Image = "https://images.unsplash.com/photo-1549880338-65ddcdfd017b?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "City Modern",
                    Code = "ART-005",
                    CategoryIds = modernCategory.Id.ToString(),
                    Size = "Large",
                    Medium = "Oil",
                    Price = 1150,
                    Description = "Energetic geometric structures and light.",
                    Image = "https://images.unsplash.com/photo-1545235617-9465d2a55698?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "Verdant",
                    Code = "ART-006",
                    CategoryIds = botanicalCategory.Id.ToString(),
                    Size = "Medium",
                    Medium = "Watercolor",
                    Price = 320,
                    Description = "Botanical study with airy transparency.",
                    Image = "https://images.unsplash.com/photo-1501004318641-b39e6451bec6?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "Ocean Dreams",
                    Code = "ART-007",
                    CategoryIds = abstractCategory.Id.ToString(),
                    Size = "Medium",
                    Medium = "Acrylic",
                    Price = 450,
                    Description = "Fluid abstract composition inspired by ocean waves.",
                    Image = "https://images.unsplash.com/photo-1545239351-1141bd82e8a6?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Artwork
                {
                    Name = "Mountain Vista",
                    Code = "ART-008",
                    CategoryIds = landscapeCategory.Id.ToString(),
                    Size = "Large",
                    Medium = "Oil",
                    Price = 750,
                    Description = "Majestic mountain landscape with dramatic lighting.",
                    Image = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?q=80&w=1400&auto=format&fit=crop",
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Artworks.AddRange(artworks);
            await context.SaveChangesAsync();

            // Seed Blog Posts
            var blogPosts = new List<BlogPost>
            {
                new BlogPost
                {
                    Title = "Collecting Art 101",
                    Excerpt = "A beginner-friendly guide to starting an art collection.",
                    Content = "<p>Starting small is perfectly fine. Focus on what resonates with you.</p><p>Visit galleries, read, and keep notes. Over time, your taste will refine.</p>",
                    Image = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?q=80&w=1600&auto=format&fit=crop",
                    Slug = "collecting-art-101",
                    PublishedAt = DateTime.UtcNow.AddDays(-30),
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-30),
                    IsPublished = true
                },
                new BlogPost
                {
                    Title = "Meet the Artist: L. Rivera",
                    Excerpt = "Studio visit and process notes from contemporary painter L. Rivera.",
                    Content = "<p>Rivera blends layers of acrylic with graphite marks to build atmosphere.</p>",
                    Image = "https://images.unsplash.com/photo-1519681393784-d120267933ba?q=80&w=1600&auto=format&fit=crop",
                    Slug = "meet-the-artist-l-rivera",
                    PublishedAt = DateTime.UtcNow.AddDays(-15),
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15),
                    IsPublished = true
                },
                new BlogPost
                {
                    Title = "Current Trends in Contemporary Art",
                    Excerpt = "What curators are watching this season.",
                    Content = "<p>Material experimentation and cross-disciplinary collaboration are thriving.</p>",
                    Image = "https://images.unsplash.com/photo-1515263487990-61b07816b324?q=80&w=1600&auto=format&fit=crop",
                    Slug = "current-trends-contemporary-art",
                    PublishedAt = DateTime.UtcNow.AddDays(-5),
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5),
                    IsPublished = true
                }
            };

            context.BlogPosts.AddRange(blogPosts);
            await context.SaveChangesAsync();
        }
    }
}
