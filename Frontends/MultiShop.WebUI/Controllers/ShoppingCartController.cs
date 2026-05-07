using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;
using NuGet.ContentModel;

namespace MultiShop.WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
       
        // product'ın ID'sine erişmek için ProductService'e ihtiyacım var. O yüzden onu da inject ediyorum.
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IDiscountService _discountService;

        public ShoppingCartController(IBasketService basketService, IProductService productService, IDiscountService discountService)
        {
            _basketService = basketService;
            _productService = productService;
            _discountService = discountService;
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

        public async Task<IActionResult> AddBasketItem(string id)
        {
            var product = await _productService.GetByIdProductAsync(id);
            if (product != null)
            {
                var items = new BasketItemDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.ProductPrice,
                    Quantity = 1,
                    ProductImageUrl= product.ProductImageUrl,
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
