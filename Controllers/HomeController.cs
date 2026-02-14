using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArtworkService _artworkService;
        private readonly ISpacesService _Spaces;

        public HomeController(ICategoryService categoryService, IArtworkService artworkService, ISpacesService spaces)
        {
            _categoryService = categoryService;
            _artworkService = artworkService;
            _Spaces = spaces;
        }

        public async Task<IActionResult> Index()
        {
            var spaces = await _Spaces.GetAllAsync();
            var featuredArtworks = await _artworkService.GetFeaturedAsync();
            
            ViewBag.Spaces = spaces;
            ViewBag.FeaturedArtworks = featuredArtworks.Take(4);
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
