using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public interface IArtworkService
    {
        Task<IEnumerable<Artwork>> GetAllAsync();
        Task<IEnumerable<Artwork>> GetFeaturedAsync();
        Task<IEnumerable<Artwork>> GetByCategoryAsync(int categoryId);
        Task<Artwork?> GetByIdAsync(int id);
        Task<Artwork?> GetByCodeAsync(string code);
        Task<Artwork> CreateAsync(Artwork artwork);
        Task<Artwork> UpdateAsync(Artwork artwork);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Artwork>> SearchAsync(string query, string? size = null, string? medium = null);
        Task<PagedResult<Models.Artwork>> SearchWithPagingAsync(
      int? categoryId,
      int? spaceId,
      string search,
      string size,
      string medium,
      int page,
      int pageSize);

    }
}
