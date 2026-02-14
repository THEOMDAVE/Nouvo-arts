using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Models;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class CustomizationController : Controller
    {
        private readonly ICustomizationService _customizationService;
        private readonly IMediumService _mediumService;

        public CustomizationController(ICustomizationService customizationService, IMediumService mediumService)
        {
            _customizationService = customizationService;
            _mediumService = mediumService;
        }

        public async Task<IActionResult> Index()
        {
            var mediums = await _mediumService.GetAllAsync();
            ViewBag.Mediums = mediums;
            ViewData["Page"] = "customization";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CustomizationRequest request)
        {
            if (ModelState.IsValid)
            {
                await _customizationService.CreateAsync(request);
                TempData["SuccessMessage"] = "Thank you! Your customization request has been submitted successfully. We'll get back to you soon.";
                return RedirectToAction(nameof(Index));
            }
            
            var mediums = await _mediumService.GetAllAsync();
            ViewBag.Mediums = mediums;
            ViewData["Page"] = "customization";
            return View(request);
        }
    }
}

