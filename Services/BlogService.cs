using Microsoft.EntityFrameworkCore;
using NouvoStudio.Data;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(bool onlyPublished = true)
        {
            var query = _context.BlogPosts.AsQueryable();
            if (onlyPublished)
            {
                query = query.Where(p => p.IsPublished);
            }
            return await query.OrderByDescending(p => p.PublishedAt).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(int id)
        {
            return await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<BlogPost?> GetBySlugAsync(string slug)
        {
            return await _context.BlogPosts.FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<BlogPost> CreateAsync(BlogPost post)
        {
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<BlogPost> UpdateAsync(BlogPost post)
        {
            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.BlogPosts.FindAsync(id);
            if (existing != null)
            {
                _context.BlogPosts.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.BlogPosts.AnyAsync(p => p.Id == id);
        }
    }
}


