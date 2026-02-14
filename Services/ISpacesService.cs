using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public interface ISpacesService
    {
        Task<IEnumerable<Spaces>> GetAllAsync();
        Task<Spaces?> GetByIdAsync(int id);
        Task<Spaces> CreateAsync(Spaces spaces);
        Task<Spaces> UpdateAsync(Spaces spaces);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
