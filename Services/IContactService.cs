using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task<ContactMessage> CreateAsync(ContactMessage message);
        Task DeleteAsync(int id);
    }
}


