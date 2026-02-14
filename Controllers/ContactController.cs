using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Models;
using NouvoStudio.Services;

namespace NouvoStudio.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Page"] = "contact";
            return View(new ContactMessage());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactMessage message)
        {
            ViewData["Page"] = "contact";
            if (!ModelState.IsValid)
            {
                return View(message);
            }
            await _contactService.CreateAsync(message);
            TempData["ContactSuccess"] = "Your message has been sent successfully.";
            return RedirectToAction(nameof(Success), new { id = message.Id });
        }

        public async Task<IActionResult> Success(int id)
        {
            var message = await _contactService.GetByIdAsync(id);
            if (message == null) return RedirectToAction(nameof(Index));
            ViewData["Page"] = "contact";
            return View(message);
        }
    }
}


