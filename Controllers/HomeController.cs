using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArtworkService _artworkService;
        private readonly ISpacesService _spacesService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ICategoryService categoryService, 
            IArtworkService artworkService, 
            ISpacesService spacesService,
            ILogger<HomeController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _artworkService = artworkService ?? throw new ArgumentNullException(nameof(artworkService));
            _spacesService = spacesService ?? throw new ArgumentNullException(nameof(spacesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var spaces = await _spacesService.GetAllAsync();
                var featuredArtworks = await _artworkService.GetFeaturedAsync();
                
                ViewBag.Spaces = spaces;
                ViewBag.FeaturedArtworks = featuredArtworks.Take(4);
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page");
                // Return view with empty data rather than crashing
                ViewBag.Spaces = Enumerable.Empty<Models.Spaces>();
                ViewBag.FeaturedArtworks = Enumerable.Empty<Models.Artwork>();
                return View();
            }
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
