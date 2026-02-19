using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NouvoStudio.Services;
using NouvoStudio.Models;
using NouvoStudio.Utilities;
using NouvoStudio.Exceptions;

namespace NouvoStudio.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISpacesService _spacesService;
        private readonly IArtworkService _artworkService;
        private readonly IWebHostEnvironment _environment;
        private readonly IBlogService _blogService;
        private readonly IContactService _contactService;
        private readonly ICustomizationService _customizationService;
        private readonly IMediumService _mediumService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            ICategoryService categoryService,
            ISpacesService spacesService, 
            IArtworkService artworkService, 
            IWebHostEnvironment environment, 
            IBlogService blogService, 
            IContactService contactService, 
            ICustomizationService customizationService, 
            IMediumService mediumService,
            ILogger<AdminController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _spacesService = spacesService ?? throw new ArgumentNullException(nameof(spacesService));
            _artworkService = artworkService ?? throw new ArgumentNullException(nameof(artworkService));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
            _customizationService = customizationService ?? throw new ArgumentNullException(nameof(customizationService));
            _mediumService = mediumService ?? throw new ArgumentNullException(nameof(mediumService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var artworks = await _artworkService.GetAllAsync();
            var spaces = await _spacesService.GetAllAsync();
            
            ViewBag.CategoryCount = categories.Count();
            ViewBag.ArtworkCount = artworks.Count();
            ViewBag.SpacesCount = spaces.Count();
            
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Spaces()
        {
            var spaces = await _spacesService.GetAllAsync();
            return View(spaces);
        }

        public async Task<IActionResult> Artworks()
        {
            var artworks = await _artworkService.GetAllAsync();
            return View(artworks);
        }

        // Category CRUD
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category, IFormFile? imageFile)
        {
            ModelState.Remove("Image");

            if (imageFile != null && imageFile.Length > 0)
            {
                var (isValid, errorMessage) = FileUploadValidator.ValidateImageFile(imageFile);
                if (!isValid)
                {
                    ModelState.AddModelError("Image", errorMessage);
                    return View(category);
                }

                try
                {
                    var savedPath = await SaveImageAsync(imageFile);
                    category.Image = savedPath;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving category image");
                    ModelState.AddModelError("Image", "An error occurred while saving the image. Please try again.");
                    return View(category);
                }
            }

            if (string.IsNullOrWhiteSpace(category.Image))
            {
                ModelState.AddModelError("Image", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.CreateAsync(category);
                    TempData["SuccessMessage"] = "Category created successfully.";
                    return RedirectToAction(nameof(Categories));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating category");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the category. Please try again.");
                }
            }
            return View(category);
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category category, IFormFile? imageFile)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if ((imageFile == null || imageFile.Length == 0) && string.IsNullOrWhiteSpace(category.Image))
            {
                var existing = await _categoryService.GetByIdAsync(id);
                if (existing != null)
                {
                    category.Image = existing.Image;
                }
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                category.Image = savedPath;
            }

            if (ModelState.IsValid)
            {
                await _categoryService.UpdateAsync(category);
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Categories));
        }


        // Spaces CRUD
        public IActionResult CreateSpaces()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSpaces(Spaces spaces, IFormFile? imageFile)
        {
            ModelState.Remove("Image");

            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                spaces.Image = savedPath;
            }

            if (string.IsNullOrWhiteSpace(spaces.Image))
            {
                ModelState.AddModelError("Image", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                await _spacesService.CreateAsync(spaces);
                return RedirectToAction(nameof(Spaces));
            }
            return View(spaces);
        }

        public async Task<IActionResult> EditSpaces(int id)
        {
            var spaces = await _spacesService.GetByIdAsync(id);
            if (spaces == null)
            {
                return NotFound();
            }
            return View(spaces);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSpaces(int id, Spaces spaces, IFormFile? imageFile)
        {
            if (id != spaces.Id)
            {
                return NotFound();
            }

            if ((imageFile == null || imageFile.Length == 0) && string.IsNullOrWhiteSpace(spaces.Image))
            {
                var existing = await _spacesService.GetByIdAsync(id);
                if (existing != null)
                {
                    spaces.Image = existing.Image;
                }
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                spaces.Image = savedPath;
            }

            if (ModelState.IsValid)
            {
                await _spacesService.UpdateAsync(spaces);
                return RedirectToAction(nameof(Spaces));
            }
            return View(spaces);
        }

        public async Task<IActionResult> DeleteSpaces(int id)
        {
            var spaces = await _spacesService.GetByIdAsync(id);
            if (spaces == null)
            {
                return NotFound();
            }
            return View(spaces);
        }

        [HttpPost, ActionName("DeleteSpaces")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSpacesConfirmed(int id)
        {
            await _spacesService.DeleteAsync(id);
            return RedirectToAction(nameof(Spaces));
        }




        // Artwork CRUD
        public async Task<IActionResult> CreateArtwork()
        {
            var categories = await _categoryService.GetAllAsync();
            var spaces = await _spacesService.GetAllAsync(); // Get spaces list
            var mediums = await _mediumService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Spaces = spaces;
            ViewBag.Mediums = mediums;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArtwork(Artwork artwork, IFormFile? imageFile, int[]? selectedCategoryIds, int[]? selectedSpaceIds)
        {
            ModelState.Remove("Image");
            ModelState.Remove("CategoryIds");
            ModelState.Remove("SpaceId");

            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                artwork.Image = savedPath;
            }

            if (string.IsNullOrWhiteSpace(artwork.Image))
            {
                ModelState.AddModelError("Image", "Image is required.");
            }

            if (selectedCategoryIds == null || selectedCategoryIds.Length == 0)
            {
                ModelState.AddModelError("CategoryIds", "At least one category must be selected.");
                artwork.CategoryIds = string.Empty;
            }
            else
            {
                artwork.CategoryIds = string.Join(",", selectedCategoryIds);
            }

            // Set SpaceId if provided
            if (selectedSpaceIds == null || selectedSpaceIds.Length == 0)
            {
                ModelState.AddModelError("SpaceIds", "At least one space must be selected.");
                artwork.SpaceIds = string.Empty;
            }
            else { 
                artwork.SpaceIds = string.Join(",", selectedSpaceIds); 
            }

            // Auto-generate Size from HeightFeet and WidthFeet
            artwork.Size = $"{artwork.HeightFeet} X {artwork.WidthFeet}";

            if (ModelState.IsValid)
            {
                await _artworkService.CreateAsync(artwork);
                return RedirectToAction(nameof(Artworks));
            }
            
            var categories = await _categoryService.GetAllAsync();
            var spaces = await _spacesService.GetAllAsync();
            var mediums = await _mediumService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Spaces = spaces;
            ViewBag.Mediums = mediums;
            return View(artwork);
        }

        public async Task<IActionResult> EditArtwork(int id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            
            var categories = await _categoryService.GetAllAsync();
            var spaces = await _spacesService.GetAllAsync();
            var mediums = await _mediumService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Spaces = spaces;
            ViewBag.Mediums = mediums;
            return View(artwork);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArtwork(int id, Artwork artwork, IFormFile? imageFile, int[]? selectedCategoryIds, int[]? selectedSpaceIds)
        {
            if (id != artwork.Id)
            {
                return NotFound();
            }

            ModelState.Remove("CategoryIds");
            ModelState.Remove("SpaceIds");

            if ((imageFile == null || imageFile.Length == 0) && string.IsNullOrWhiteSpace(artwork.Image))
            {
                var existing = await _artworkService.GetByIdAsync(id);
                if (existing != null)
                {
                    artwork.Image = existing.Image;
                }
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                artwork.Image = savedPath;
            }

            if (selectedCategoryIds == null || selectedCategoryIds.Length == 0)
            {
                ModelState.AddModelError("CategoryIds", "At least one category must be selected.");
                artwork.CategoryIds = string.Empty;
            }
            else
            {
                artwork.CategoryIds = string.Join(",", selectedCategoryIds);
            }

            if (selectedSpaceIds == null || selectedSpaceIds.Length == 0)
            {
                ModelState.AddModelError("SpaceIds", "At least one space must be selected.");
                artwork.SpaceIds = string.Empty;
            }
            else
            {
                artwork.SpaceIds = string.Join(",", selectedSpaceIds);
            }

            // Auto-generate Size from HeightFeet and WidthFeet
            artwork.Size = $"{artwork.HeightFeet} X {artwork.WidthFeet}";
            
            if (ModelState.IsValid)
            {
                await _artworkService.UpdateAsync(artwork);
                return RedirectToAction(nameof(Artworks));
            }
            
            var categories = await _categoryService.GetAllAsync();
            var spaces = await _spacesService.GetAllAsync();
            var mediums = await _mediumService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Spaces = spaces;
            ViewBag.Mediums = mediums;
            return View(artwork);
        }

        // Medium CRUD
        public async Task<IActionResult> Mediums()
        {
            var mediums = await _mediumService.GetAllAsync();
            return View(mediums);
        }

        public IActionResult CreateMedium()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMedium(Medium medium)
        {
            if (ModelState.IsValid)
            {
                await _mediumService.CreateAsync(medium);
                return RedirectToAction(nameof(Mediums));
            }
            return View(medium);
        }

        public async Task<IActionResult> EditMedium(int id)
        {
            var medium = await _mediumService.GetByIdAsync(id);
            if (medium == null) return NotFound();
            return View(medium);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedium(int id, Medium medium)
        {
            if (id != medium.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _mediumService.UpdateAsync(medium);
                return RedirectToAction(nameof(Mediums));
            }
            return View(medium);
        }

        public async Task<IActionResult> DeleteMedium(int id)
        {
            var medium = await _mediumService.GetByIdAsync(id);
            if (medium == null) return NotFound();
            return View(medium);
        }

        [HttpPost, ActionName("DeleteMedium")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMediumConfirmed(int id)
        {
            await _mediumService.DeleteAsync(id);
            return RedirectToAction(nameof(Mediums));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is required.", nameof(imageFile));

            var imagesDir = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            var fileName = FileUploadValidator.GetSafeFileName(imageFile.FileName);
            var physicalPath = Path.Combine(imagesDir, fileName);

            // Ensure the file doesn't already exist (unlikely with GUID, but safe)
            if (System.IO.File.Exists(physicalPath))
            {
                fileName = FileUploadValidator.GetSafeFileName(imageFile.FileName);
                physicalPath = Path.Combine(imagesDir, fileName);
            }

            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var relativePath = $"/images/{fileName}";
            _logger.LogInformation("Image saved: {RelativePath}", relativePath);
            return relativePath;
        }

        public async Task<IActionResult> DeleteArtwork(int id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return View(artwork);
        }

        [HttpPost, ActionName("DeleteArtwork")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArtworkConfirmed(int id)
        {
            await _artworkService.DeleteAsync(id);
            return RedirectToAction(nameof(Artworks));
        }

        // Blog CRUD
        public async Task<IActionResult> Blog()
        {
            var posts = await _blogService.GetAllAsync(false);
            return View(posts);
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(BlogPost post, IFormFile? imageFile)
        {
            ModelState.Remove("Image");
            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                post.Image = savedPath;
            }
            if (string.IsNullOrWhiteSpace(post.Image))
            {
                ModelState.AddModelError("Image", "Image is required.");
            }
            if (string.IsNullOrWhiteSpace(post.Slug))
            {
                post.Slug = post.Title?.Trim().ToLower().Replace(' ', '-').Replace("--", "-") ?? Guid.NewGuid().ToString("N");
            }
            if (ModelState.IsValid)
            {
                await _blogService.CreateAsync(post);
                return RedirectToAction(nameof(Blog));
            }
            return View(post);
        }

        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _blogService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, BlogPost post, IFormFile? imageFile)
        {
            if (id != post.Id) return NotFound();

            if ((imageFile == null || imageFile.Length == 0) && string.IsNullOrWhiteSpace(post.Image))
            {
                var existing = await _blogService.GetByIdAsync(id);
                if (existing != null)
                {
                    post.Image = existing.Image;
                }
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                var savedPath = await SaveImageAsync(imageFile);
                post.Image = savedPath;
            }
            if (ModelState.IsValid)
            {
                await _blogService.UpdateAsync(post);
                return RedirectToAction(nameof(Blog));
            }
            return View(post);
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _blogService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostConfirmed(int id)
        {
            await _blogService.DeleteAsync(id);
            return RedirectToAction(nameof(Blog));
        }

        // Contact messages
        public async Task<IActionResult> ContactMessages()
        {
            var messages = await _contactService.GetAllAsync();
            return View(messages);
        }

        public async Task<IActionResult> ContactMessageDetails(int id)
        {
            var msg = await _contactService.GetByIdAsync(id);
            if (msg == null) return NotFound();
            return View(msg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            await _contactService.DeleteAsync(id);
            return RedirectToAction(nameof(ContactMessages));
        }

        // Customization requests
        public async Task<IActionResult> Customizations()
        {
            var requests = await _customizationService.GetAllAsync();
            return View(requests);
        }

        public async Task<IActionResult> CustomizationDetails(int id)
        {
            var request = await _customizationService.GetByIdAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomization(int id)
        {
            await _customizationService.DeleteAsync(id);
            return RedirectToAction(nameof(Customizations));
        }
    }
}
