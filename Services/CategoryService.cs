using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NouvoStudio.Data;
using NouvoStudio.Exceptions;
using NouvoStudio.Models;

namespace NouvoStudio.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // Check if name already exists
            if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
            {
                throw new ValidationException($"A category with name '{category.Name}' already exists.");
            }

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category {CategoryId} ({Name}) created successfully.", category.Id, category.Name);
                return category;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating category {Name}", category.Name);
                throw new ValidationException("An error occurred while creating the category. Please check the data and try again.");
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var existing = await GetByIdAsync(category.Id);
            if (existing == null)
            {
                throw new NotFoundException("Category", category.Id);
            }

            // Check if name is being changed and if new name already exists
            if (existing.Name != category.Name && await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != category.Id))
            {
                throw new ValidationException($"A category with name '{category.Name}' already exists.");
            }

            category.UpdatedAt = DateTime.UtcNow;
            
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category {CategoryId} ({Name}) updated successfully.", category.Id, category.Name);
                return category;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating category {CategoryId}", category.Id);
                throw new ValidationException("An error occurred while updating the category. Please check the data and try again.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Category", id);
            }

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category {CategoryId} ({Name}) deleted successfully.", category.Id, category.Name);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting category {CategoryId}", id);
                throw new ValidationException("An error occurred while deleting the category.");
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
