using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public interface IMediumService
    {
        Task<IEnumerable<Medium>> GetAllAsync();
        Task<Medium?> GetByIdAsync(int id);
        Task<Medium> CreateAsync(Medium medium);
        Task UpdateAsync(Medium medium);
        Task DeleteAsync(int id);
    }
}




