using Microsoft.EntityFrameworkCore;
using NouvoStudio.Data;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactMessage>> GetAllAsync()
        {
            return await _context.ContactMessages
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<ContactMessage?> GetByIdAsync(int id)
        {
            return await _context.ContactMessages.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ContactMessage> CreateAsync(ContactMessage message)
        {
            message.CreatedAt = DateTime.UtcNow;
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.ContactMessages.FindAsync(id);
            if (existing != null)
            {
                _context.ContactMessages.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}


