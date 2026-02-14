using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetAllAsync();
            ViewData["Page"] = "blog";
            return View(posts);
        }

        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();
            var post = await _blogService.GetBySlugAsync(slug);
            if (post == null) return NotFound();
            ViewData["Page"] = "blog";
            return View(post);
        }
    }
}


