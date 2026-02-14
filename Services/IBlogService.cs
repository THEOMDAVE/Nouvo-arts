using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(bool onlyPublished = true);
        Task<BlogPost?> GetByIdAsync(int id);
        Task<BlogPost?> GetBySlugAsync(string slug);
        Task<BlogPost> CreateAsync(BlogPost post);
        Task<BlogPost> UpdateAsync(BlogPost post);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}


