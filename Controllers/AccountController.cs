using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NouvoStudio.Settings;
using NouvoStudio.Utilities;
using System.Security.Claims;

namespace NouvoStudio.Controllers
{
    public class AccountController : Controller
    {
        private readonly AdminOptions _adminOptions;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IOptions<AdminOptions> adminOptions, ILogger<AccountController> logger)
        {
            _adminOptions = adminOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Username and password are required.");
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            bool isAuthenticated = false;

            // Check username
            if (string.Equals(username, _adminOptions.Username, StringComparison.Ordinal))
            {
                // Check password hash first (new method)
                //if (!string.IsNullOrEmpty(_adminOptions.PasswordHash))
                //{
                //    isAuthenticated = PasswordHasher.VerifyPassword(password, _adminOptions.PasswordHash);
                //}
                //// Fallback to plain password for migration (backward compatibility)
                //else 
                if (!string.IsNullOrEmpty(_adminOptions.PasswordHash))
                {
                    isAuthenticated = string.Equals(password, _adminOptions.PasswordHash, StringComparison.Ordinal);
                    _logger.LogWarning("Using plain text password authentication. Please migrate to password hash.");
                }
            }

            if (isAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });

                _logger.LogInformation("Admin user {Username} logged in successfully.", username);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Admin");
            }

            _logger.LogWarning("Failed login attempt for username: {Username}", username);
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name ?? "Unknown";
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Admin user {Username} logged out.", username);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}


