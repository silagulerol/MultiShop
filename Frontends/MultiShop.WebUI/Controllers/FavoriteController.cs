using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.FavoriteServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "LogIn");
            }

            var values = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(string productId, string? returnUrl)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index", "LogIn");
            }

            if (!string.IsNullOrWhiteSpace(productId))
            {
                await _favoriteService.ToggleFavoriteAsync(userId, productId);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirstValue("sub")
                   ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
