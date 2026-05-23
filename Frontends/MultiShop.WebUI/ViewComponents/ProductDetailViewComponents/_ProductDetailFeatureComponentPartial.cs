using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.FavoriteServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Security.Claims;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailFeatureComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;

        public _ProductDetailFeatureComponentPartial(IProductService productService, IFavoriteService favoriteService)
        {
            _productService = productService;
            _favoriteService = favoriteService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var value = await _productService.GetByIdProductAsync(id);
            ViewBag.IsFavorited = await IsFavoritedAsync(id);
            return View(value);
        }

        private async Task<bool> IsFavoritedAsync(string productId)
        {
            var currentUser = ViewContext.HttpContext.User;

            if (currentUser.Identity?.IsAuthenticated != true)
            {
                return false;
            }

            var userId = currentUser.FindFirstValue("sub")
                         ?? currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return !string.IsNullOrWhiteSpace(userId) && await _favoriteService.IsProductFavoritedAsync(userId, productId);
        }
    }
}
