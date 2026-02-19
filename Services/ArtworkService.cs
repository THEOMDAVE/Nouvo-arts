using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NouvoStudio.Data;
using NouvoStudio.Exceptions;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArtworkService> _logger;

        public ArtworkService(ApplicationDbContext context, ILogger<ArtworkService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            if (artwork == null)
                throw new ArgumentNullException(nameof(artwork));

            // Check if code already exists
            if (await _context.Artworks.AnyAsync(a => a.Code == artwork.Code))
            {
                throw new ValidationException($"An artwork with code '{artwork.Code}' already exists.");
            }

            artwork.CreatedAt = DateTime.UtcNow;
            artwork.UpdatedAt = DateTime.UtcNow;
            
            try
            {
                _context.Artworks.Add(artwork);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Artwork {ArtworkId} ({Code}) created successfully.", artwork.Id, artwork.Code);
                return artwork;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating artwork with code {Code}", artwork.Code);
                throw new ValidationException("An error occurred while creating the artwork. Please check the data and try again.");
            }
        }

        public async Task<Artwork> UpdateAsync(Artwork artwork)
        {
            if (artwork == null)
                throw new ArgumentNullException(nameof(artwork));

            var existing = await GetByIdAsync(artwork.Id);
            if (existing == null)
            {
                throw new NotFoundException("Artwork", artwork.Id);
            }

            // Check if code is being changed and if new code already exists
            if (existing.Code != artwork.Code && await _context.Artworks.AnyAsync(a => a.Code == artwork.Code && a.Id != artwork.Id))
            {
                throw new ValidationException($"An artwork with code '{artwork.Code}' already exists.");
            }

            artwork.UpdatedAt = DateTime.UtcNow;
            
            try
            {
                _context.Artworks.Update(artwork);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Artwork {ArtworkId} ({Code}) updated successfully.", artwork.Id, artwork.Code);
                return artwork;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating artwork {ArtworkId}", artwork.Id);
                throw new ValidationException("An error occurred while updating the artwork. Please check the data and try again.");
            }
        }   

        public async Task DeleteAsync(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                throw new NotFoundException("Artwork", id);
            }

            try
            {
                _context.Artworks.Remove(artwork);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Artwork {ArtworkId} ({Code}) deleted successfully.", artwork.Id, artwork.Code);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting artwork {ArtworkId}", id);
                throw new ValidationException("An error occurred while deleting the artwork.");
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Artworks.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Artwork>> SearchAsync(string query, string? size = null, string? medium = null)
        {
            try
            {
                var artworks = _context.Artworks.AsQueryable();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    artworks = artworks.Where(a => 
                        a.Name.Contains(query) || 
                        a.Code.Contains(query) ||
                        (!string.IsNullOrEmpty(a.Description) && a.Description.Contains(query)));
                } 

                if (!string.IsNullOrWhiteSpace(size))
                {
                    artworks = artworks.Where(a => a.Size.ToLower() == size.ToLower());
                }

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    artworks = artworks.Where(a => a.Medium == medium);
                }

                return await artworks.OrderBy(a => a.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching artworks with query: {Query}", query);
                throw;
            }
        }

        public async Task<PagedResult<Artwork>> SearchWithPagingAsync(
            int? categoryId, 
            int? spaceId, 
            string? search, 
            string? size, 
            string? medium, 
            int page, 
            int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 12;
            if (pageSize > 100) pageSize = 100; // Prevent excessive page sizes

            try
            {
                var query = _context.Artworks.AsQueryable();

                // Category filter
                if (categoryId.HasValue)
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.CategoryIds) && 
                        x.CategoryIds.Contains(categoryId.Value.ToString()));
                }

                // Space filter
                if (spaceId.HasValue)
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.SpaceIds) && 
                        x.SpaceIds.Contains(spaceId.Value.ToString()));
                }

                // Search filter
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x => x.Name.Contains(search) || 
                        x.Code.Contains(search) ||
                        (!string.IsNullOrEmpty(x.Description) && x.Description.Contains(search)));
                }

                // Size filter
                if (!string.IsNullOrWhiteSpace(size))
                {
                    query = query.Where(x => x.Size == size);
                }

                // Medium filter
                if (!string.IsNullOrWhiteSpace(medium))
                {
                    query = query.Where(x => x.Medium == medium);
                }

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(x => x.IsFeatured)
                    .ThenByDescending(x => x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<Artwork>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching artworks with paging");
                throw;
            }
        }

    }
}
