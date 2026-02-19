using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NouvoStudio.Data;
using NouvoStudio.Exceptions;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class SpacesService : ISpacesService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SpacesService> _logger;

        public SpacesService(ApplicationDbContext context, ILogger<SpacesService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Spaces>> GetAllAsync()
        {
            return await _context.Spaces
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Spaces?> GetByIdAsync(int id)
        {
            return await _context.Spaces
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Spaces> CreateAsync(Spaces spaces)
        {
            if (spaces == null)
                throw new ArgumentNullException(nameof(spaces));

            // Check if name already exists
            if (await _context.Spaces.AnyAsync(s => s.Name == spaces.Name))
            {
                throw new ValidationException($"A space with name '{spaces.Name}' already exists.");
            }

            spaces.CreatedAt = DateTime.UtcNow;
            spaces.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Spaces.Add(spaces);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Space {SpaceId} ({Name}) created successfully.", spaces.Id, spaces.Name);
                return spaces;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating space {Name}", spaces.Name);
                throw new ValidationException("An error occurred while creating the space. Please check the data and try again.");
            }
        }

        public async Task<Spaces> UpdateAsync(Spaces spaces)
        {
            if (spaces == null)
                throw new ArgumentNullException(nameof(spaces));

            var existing = await GetByIdAsync(spaces.Id);
            if (existing == null)
            {
                throw new NotFoundException("Space", spaces.Id);
            }

            // Check if name is being changed and if new name already exists
            if (existing.Name != spaces.Name && await _context.Spaces.AnyAsync(s => s.Name == spaces.Name && s.Id != spaces.Id))
            {
                throw new ValidationException($"A space with name '{spaces.Name}' already exists.");
            }

            spaces.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Spaces.Update(spaces);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Space {SpaceId} ({Name}) updated successfully.", spaces.Id, spaces.Name);
                return spaces;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating space {SpaceId}", spaces.Id);
                throw new ValidationException("An error occurred while updating the space. Please check the data and try again.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var space = await _context.Spaces.FindAsync(id);
            if (space == null)
            {
                throw new NotFoundException("Space", id);
            }

            try
            {
                _context.Spaces.Remove(space);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Space {SpaceId} ({Name}) deleted successfully.", space.Id, space.Name);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting space {SpaceId}", id);
                throw new ValidationException("An error occurred while deleting the space.");
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Spaces.AnyAsync(s => s.Id == id);
        }
    }
}
