using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.ProductVariantService;
using MultiShop.WebUI.Services.DiscountServices;
using System.Globalization;

namespace MultiShop.WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
       
        // product'ın ID'sine erişmek için ProductService'e ihtiyacım var. O yüzden onu da inject ediyorum.
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IDiscountService _discountService;
        private readonly IProductVariantService _productVariantService;

        public ShoppingCartController(IBasketService basketService, IProductService productService, IDiscountService discountService, IProductVariantService productVariantService)
        {
            _basketService = basketService;
            _productService = productService;
            _discountService = discountService;
            _productVariantService = productVariantService;
        }


        public async Task<IActionResult> Index(string? code)
        {
            ShoppingCartViewBag();
            var basket = await _basketService.GetBasketAsync();
            ViewBag.total = basket.TotalPrice;

            if (!string.IsNullOrEmpty(code))
            {
                var discount = await _discountService.GetDiscountCode(code);
                var totalPriceWithDiscount = basket.TotalPrice  - (basket.TotalPrice * discount.rate / 100);
                ViewBag.totalPriceWithDiscount = totalPriceWithDiscount;
                ViewBag.code = code;
                ViewBag.discountRate = discount.rate;
            }

            return View();
        }

        public async Task<IActionResult> AddBasketItem(
            string id,
            string? productVariantId,
            string? size,
            string? color,
            string? price,
            string? productImageUrl,
            int quantity = 1)
        {
            var product = await _productService.GetByIdProductAsync(id);
            if (product != null)
            {
                var unitPrice = product.StartingDiscountedPrice ?? product.StartingPrice;
                var selectedProductVariantId = string.IsNullOrWhiteSpace(productVariantId) ? null : productVariantId;
                var selectedSize = size;
                var selectedColor = color;
                var selectedProductImageUrl = productImageUrl;

                if (!string.IsNullOrWhiteSpace(selectedProductVariantId))
                {
                    var variant = await _productVariantService.GetByIdProductVariantAsync(selectedProductVariantId);
                    if (!string.IsNullOrWhiteSpace(variant.ProductVariantId) && variant.ProductId == product.ProductId)
                    {
                        selectedProductVariantId = variant.ProductVariantId;
                        selectedSize = variant.Size;
                        selectedColor = variant.Color;
                        unitPrice = variant.DiscountedPrice ?? variant.Price;
                        selectedProductImageUrl = string.IsNullOrWhiteSpace(variant.MainImageUrl)
                            ? product.MainImageUrl
                            : variant.MainImageUrl;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(price) &&
                         decimal.TryParse(price, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedPrice))
                {
                    unitPrice = parsedPrice;
                }

                var items = new BasketItemDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = unitPrice,
                    Quantity = quantity < 1 ? 1 : quantity,
                    ProductVariantId = selectedProductVariantId,
                    Size = selectedSize,
                    Color = selectedColor,
                    ProductImageUrl = string.IsNullOrWhiteSpace(selectedProductImageUrl) ? product.MainImageUrl : selectedProductImageUrl,
                    VendorId = product.VendorId
                };
                await _basketService.AddBasketItem(items);
            }
            return Redirect("/ShoppingCart/Index");
        }

        // {controller=Home}/{action=Index}/{id?}
        public async Task<IActionResult> RemoveBasketItem(string id)
        {
            await _basketService.RemoveBasketItem(id);
            return RedirectToAction("Index");
        }

        public void ShoppingCartViewBag()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Pages";
            ViewBag.directory3 = "Products";
        }
    }
}
