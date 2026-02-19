using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Data;
using NouvoStudio.Models;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class ArtworksController : Controller
    {
        private readonly IArtworkService _artworkService;
        private readonly ICategoryService _categoryService;
        private readonly IMediumService _mediumService;
        private readonly ISpacesService _spaces;
        private readonly ApplicationDbContext _context;


        public ArtworksController(IArtworkService artworkService, ICategoryService categoryService, IMediumService mediumService, ISpacesService spaces)
        {
            _artworkService = artworkService;
            _categoryService = categoryService;
            _mediumService = mediumService;
            _spaces = spaces;
        }

        //public async Task<IActionResult> Index(string? category, string? search, string? size, string? medium, int page = 1, int pageSize = 12)
        //{
        //    IEnumerable<Models.Artwork> artworks;

        //    artworks = await _artworkService.SearchWithPagingAsync(search, size, medium,  page, pageSize);

        //    ViewBag.Page = page;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.Mediums = await _mediumService.GetAllAsync();
        //    ViewBag.SearchQuery = search;
        //    ViewBag.SelectedSize = size;
        //    ViewBag.SelectedMedium = medium;

        //    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        return PartialView("_ArtworkGrid", artworks);
        //    }

        //    return View(artworks);
        //}

        public async Task<IActionResult> Index( int? categoryId, int? spaceId, string? search, string? size, string? medium, int page = 1, int pageSize = 12)
        {
            var result = await _artworkService.SearchWithPagingAsync( categoryId, spaceId, search, size, medium, page, pageSize);

            ViewBag.Page = result.Page;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalCount = result.TotalCount;
            ViewBag.Mediums = await _mediumService.GetAllAsync();
            ViewBag.SearchQuery = search;
            ViewBag.SelectedSize = size;
            ViewBag.SelectedMedium = medium;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedSpaceId = spaceId;

            if (categoryId.HasValue) 
            {
                var category = await _categoryService.GetByIdAsync(categoryId.Value);
                if (category != null)
                {
                    ViewBag.Name = category.Name;
                }
            }

            if (spaceId.HasValue) 
            {
                var space = await _spaces.GetByIdAsync(spaceId.Value);
                if (space != null)
                {
                    ViewBag.Name = space.Name;
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ArtworkGrid", result.Items);
            }

            return View(result.Items);
        }




        public async Task<IActionResult> Details(int id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Spaces = await _spaces.GetAllAsync();

            return View(artwork);
        }

        public async Task<IActionResult> Favorites()
        {
            var artworks = await _artworkService.GetAllAsync();
            ViewData["Page"] = "favorites";
            return View(artworks);
        }
    }
}
