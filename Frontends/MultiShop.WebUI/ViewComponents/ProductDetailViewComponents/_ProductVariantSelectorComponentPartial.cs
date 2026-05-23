using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.ProductVariantService;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductVariantSelectorComponentPartial : ViewComponent
    {
        private readonly IProductVariantService _productVariantService;

        public _ProductVariantSelectorComponentPartial(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string id,
            decimal startingPrice,
            string? mainImageUrl)
        {
            var values = await _productVariantService.GetAvailableProductVariantsByProductIdAsync(id);

            ViewBag.ProductId = id;
            ViewBag.StartingPrice = startingPrice;
            ViewBag.MainImageUrl = mainImageUrl;

            return View(values);
        }
    }
}
