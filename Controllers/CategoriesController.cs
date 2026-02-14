using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArtworkService _artworkService;

        public CategoriesController(ICategoryService categoryService, IArtworkService artworkService)
        {
            _categoryService = categoryService;
            _artworkService = artworkService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var artworks = await _artworkService.GetByCategoryAsync(id);
            ViewBag.Artworks = artworks;
            
            return View(category);
        }
    }
}
