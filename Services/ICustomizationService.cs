using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public interface ICustomizationService
    {
        Task<IEnumerable<CustomizationRequest>> GetAllAsync();
        Task<CustomizationRequest?> GetByIdAsync(int id);
        Task<CustomizationRequest> CreateAsync(CustomizationRequest request);
        Task DeleteAsync(int id);
    }
}

