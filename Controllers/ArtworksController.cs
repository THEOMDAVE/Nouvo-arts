using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NouvoStudio.Models;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class ArtworksController : Controller
    {
        private readonly IArtworkService _artworkService;
        private readonly ICategoryService _categoryService;
        private readonly IMediumService _mediumService;
        private readonly ISpacesService _spacesService;
        private readonly ILogger<ArtworksController> _logger;

        public ArtworksController(
            IArtworkService artworkService, 
            ICategoryService categoryService, 
            IMediumService mediumService, 
            ISpacesService spacesService,
            ILogger<ArtworksController> logger)
        {
            _artworkService = artworkService ?? throw new ArgumentNullException(nameof(artworkService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _mediumService = mediumService ?? throw new ArgumentNullException(nameof(mediumService));
            _spacesService = spacesService ?? throw new ArgumentNullException(nameof(spacesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        //public async Task<IActionResult> Index(int? categoryId, int? spaceId, string? search, string? size, string? medium, int page = 1, int pageSize = 12)
        //{
        //    try
        //    {
        //        var result = await _artworkService.SearchWithPagingAsync(categoryId, spaceId, search, size, medium, page, pageSize);

        //        ViewBag.Page = result.Page;
        //        ViewBag.PageSize = result.PageSize;
        //        ViewBag.TotalCount = result.TotalCount;
        //        ViewBag.Mediums = await _mediumService.GetAllAsync();
        //        ViewBag.SearchQuery = search;
        //        ViewBag.SelectedSize = size;
        //        ViewBag.SelectedMedium = medium;
        //        ViewBag.SelectedCategoryId = categoryId;
        //        ViewBag.SelectedSpaceId = spaceId;

        //        if (categoryId.HasValue)
        //        {
        //            var category = await _categoryService.GetByIdAsync(categoryId.Value);
        //            if (category != null)
        //            {
        //                ViewBag.Name = category.Name;
        //            }
        //        }

        //        if (spaceId.HasValue)
        //        {
        //            var space = await _spacesService.GetByIdAsync(spaceId.Value);
        //            if (space != null)
        //            {
        //                ViewBag.Name = space.Name;
        //            }
        //        }

        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {
        //            return PartialView("_ArtworkGrid", result.Items);
        //        }

        //        return View(result.Items);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error loading artworks page");
        //        return View(Enumerable.Empty<Artwork>());
        //    }
        //}
        public async Task<IActionResult> Index(
    int? categoryId,
    int? spaceId,
    string? search,
    string? size,
    string? medium,
    int page = 1,
    int pageSize = 30)
        {
            try
            {
                var result = await _artworkService.SearchWithPagingAsync(
                    categoryId, spaceId, search, size, medium, page, pageSize);

                string? name = null;

                if (categoryId.HasValue)
                {
                    var category = await _categoryService.GetByIdAsync(categoryId.Value);
                    name = category?.Name;
                }

                if (spaceId.HasValue)
                {
                    var space = await _spacesService.GetByIdAsync(spaceId.Value);
                    name = space?.Name;
                }

                var viewModel = new ArtworkListViewModel
                {
                    Artworks = result.Items,
                    Page = result.Page,
                    PageSize = result.PageSize,
                    TotalCount = result.TotalCount,
                    SearchQuery = search,
                    SelectedSize = size,
                    SelectedMedium = medium,
                    SelectedCategoryId = categoryId,
                    SelectedSpaceId = spaceId,
                    Name = name,
                    Mediums = await _mediumService.GetAllAsync()
                };

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("_ArtworkGrid", viewModel.Artworks);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading artworks page");
                return View(new ArtworkListViewModel());
            }
        }


        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var artwork = await _artworkService.GetByIdAsync(id);
                if (artwork == null)
                {
                    return NotFound();
                }

                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Spaces = await _spacesService.GetAllAsync();

                return View(artwork);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading artwork details for ID {ArtworkId}", id);
                return NotFound();
            }
        }

        public async Task<IActionResult> Favorites()
        {
            try
            {
                var artworks = await _artworkService.GetAllAsync();
                ViewData["Page"] = "favorites";
                return View(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading favorites");
                return View(Enumerable.Empty<Artwork>());
            }
        }
    }
}
