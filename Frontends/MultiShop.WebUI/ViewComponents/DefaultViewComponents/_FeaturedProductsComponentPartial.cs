using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.FavoriteServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Security.Claims;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeaturedProductsComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;

        public _FeaturedProductsComponentPartial(IProductService productService, IFavoriteService favoriteService)
        {
            _productService = productService;
            _favoriteService = favoriteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = (await _productService.GetAllProductAsync()).Take(5).ToList();
            ViewBag.FavoriteProductIds = await GetFavoriteProductIdsAsync();
            return View(values);
        }

        private async Task<HashSet<string>> GetFavoriteProductIdsAsync()
        {
            var currentUser = ViewContext.HttpContext.User;

            if (currentUser.Identity?.IsAuthenticated != true)
            {
                return new HashSet<string>();
            }

            var userId = currentUser.FindFirstValue("sub")
                         ?? currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return new HashSet<string>();
            }

            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            return favorites.Select(x => x.ProductId).ToHashSet();
        }
    }
}
