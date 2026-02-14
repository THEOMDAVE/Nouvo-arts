using Microsoft.EntityFrameworkCore;
using NouvoStudio.Data;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;

        public ArtworkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artwork>> GetAllAsync()
        {
            return await _context.Artworks
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Artwork>> GetFeaturedAsync()
        {
            return await _context.Artworks
                .Where(a => a.IsFeatured)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Artwork>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Artworks
                .Where(a => a.CategoryIds.Contains(categoryId.ToString()))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Artwork?> GetByIdAsync(int id)
        {
            return await _context.Artworks
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Artwork?> GetByCodeAsync(string code)
        {
            return await _context.Artworks
                .FirstOrDefaultAsync(a => a.Code == code);
        }

        public async Task<Artwork> CreateAsync(Artwork artwork)
        {
            artwork.CreatedAt = DateTime.UtcNow;
            artwork.UpdatedAt = DateTime.UtcNow;
            
            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();
            return artwork;
        }

        public async Task<Artwork> UpdateAsync(Artwork artwork)
        {
            artwork.UpdatedAt = DateTime.UtcNow;
            
            _context.Artworks.Update(artwork);
            await _context.SaveChangesAsync();
            return artwork;
        }   

        public async Task DeleteAsync(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork != null)
            {
                _context.Artworks.Remove(artwork);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Artworks.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Artwork>> SearchAsync(string query, string? size = null, string? medium = null)
        {
            var artworks = _context.Artworks.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                artworks = artworks.Where(a => 
                    a.Name.Contains(query) || 
                    a.Code.Contains(query) ||
                    a.Description!.Contains(query));
            } 

            if (!string.IsNullOrEmpty(size))
            {
                artworks = artworks.Where(a => a.Size.ToLower() == size.ToLower());
            }

            if (!string.IsNullOrEmpty(medium))
            {
                artworks = artworks.Where(a => a.Medium == medium);
            }

            return await artworks.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<IEnumerable<Models.Artwork>> SearchWithPagingAsync(
  string search,
  string size,
  string medium,
  int page,
  int pageSize)
        {
            var query = _context.Artworks.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Contains(search));

            if (!string.IsNullOrEmpty(size))
                query = query.Where(x => x.Size == size);

            if (!string.IsNullOrEmpty(medium))
                query = query.Where(x => x.Medium == medium);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
