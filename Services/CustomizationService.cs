using Microsoft.EntityFrameworkCore;
using NouvoStudio.Data;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class CustomizationService : ICustomizationService
    {
        private readonly ApplicationDbContext _context;

        public CustomizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomizationRequest>> GetAllAsync()
        {
            return await _context.CustomizationRequests
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<CustomizationRequest?> GetByIdAsync(int id)
        {
            return await _context.CustomizationRequests.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CustomizationRequest> CreateAsync(CustomizationRequest request)
        {
            request.CreatedAt = DateTime.UtcNow;
            _context.CustomizationRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.CustomizationRequests.FindAsync(id);
            if (existing != null)
            {
                _context.CustomizationRequests.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}

