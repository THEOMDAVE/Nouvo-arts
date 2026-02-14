using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class SpacesController : Controller
    {
        private readonly ISpacesService _spacesService;

        public SpacesController(ISpacesService spacesService)
        {
            _spacesService = spacesService;
        }

        public async Task<IActionResult> Index()
        {
            var spaces = await _spacesService.GetAllAsync();
            return View(spaces);
        }

        public async Task<IActionResult> Details(int id)
        {
            var space = await _spacesService.GetByIdAsync(id);
            if (space == null)
            {
                return NotFound();
            }

            return View(space);
        }
    }
}


