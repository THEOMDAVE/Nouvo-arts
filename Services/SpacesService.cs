using Microsoft.EntityFrameworkCore;
using NouvoStudio.Data;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class SpacesService : ISpacesService
    {
        private readonly ApplicationDbContext _context;

        public SpacesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Spaces>> GetAllAsync()
        {
            return await _context.Spaces
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Spaces?> GetByIdAsync(int id)
        {
            return await _context.Spaces
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Spaces> CreateAsync(Spaces spaces)
        {
            spaces.CreatedAt = DateTime.UtcNow;
            spaces.UpdatedAt = DateTime.UtcNow;

            _context.Spaces.Add(spaces);
            await _context.SaveChangesAsync();
            return spaces;
        }

        public async Task<Spaces> UpdateAsync(Spaces spaces)
        {
            spaces.UpdatedAt = DateTime.UtcNow;

            _context.Spaces.Update(spaces);
            await _context.SaveChangesAsync();
            return spaces;
        }

        public async Task DeleteAsync(int id)
        {
            var spaces = await _context.Spaces.FindAsync(id);
            if (spaces != null)
            {
                _context.Spaces.Remove(spaces);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Spaces.AnyAsync(c => c.Id == id);
        }
    }
}
