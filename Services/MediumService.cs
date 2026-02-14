using Microsoft.EntityFrameworkCore;
using NouvoStudio.Data;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class MediumService : IMediumService
    {
        private readonly ApplicationDbContext _context;

        public MediumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medium>> GetAllAsync()
        {
            return await _context.Mediums
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Medium?> GetByIdAsync(int id)
        {
            return await _context.Mediums.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Medium> CreateAsync(Medium medium)
        {
            _context.Mediums.Add(medium);
            await _context.SaveChangesAsync();
            return medium;
        }

        public async Task UpdateAsync(Medium medium)
        {
            _context.Mediums.Update(medium);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Mediums.FindAsync(id);
            if (existing != null)
            {
                _context.Mediums.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}




