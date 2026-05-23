using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductVariantDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.ProductVariantService;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class ProductVariantController : Controller
    {
        private readonly IProductVariantService _productVariantService;
        private readonly IProductService _productService;

        public ProductVariantController(
            IProductVariantService productVariantService,
            IProductService productService)
        {
            _productVariantService = productVariantService;
            _productService = productService;
        }

        private string GetCurrentVendorId()
        {
            return User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private async Task<bool> IsProductOwner(string productId)
        {
            var product = await _productService.GetByIdProductAsync(productId);

            if (product == null)
            {
                return false;
            }

            return product.VendorId == GetCurrentVendorId();
        }

        [HttpGet]
        public async Task<IActionResult> ProductVariantList(string id)
        {
            if (!await IsProductOwner(id))
            {
                return Forbid();
            }

            ProductVariantViewBag();
            ViewBag.ProductId = id;

            var product = await _productService.GetByIdProductAsync(id);
            ViewBag.ProductName = product?.ProductName;

            var values = await _productVariantService.GetProductVariantsByProductIdAsync(id);
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductVariant(string id)
        {
            if (!await IsProductOwner(id))
            {
                return Forbid();
            }

            ProductVariantViewBag();
            return View(new CreateProductVariantDto
            {
                ProductId = id,
                Stock = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductVariant(CreateProductVariantDto createProductVariantDto)
        {
            if (string.IsNullOrEmpty(createProductVariantDto.ProductId))
            {
                return BadRequest("ProductId boş geliyor.");
            }

            if (!await IsProductOwner(createProductVariantDto.ProductId))
            {
                return Forbid();
            }

            await _productVariantService.CreateProductVariantAsync(createProductVariantDto);
            return Redirect($"/Vendor/ProductVariant/ProductVariantList/{createProductVariantDto.ProductId}");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProductVariant(string id)
        {
            var value = await _productVariantService.GetByIdProductVariantAsync(id);

            if (string.IsNullOrEmpty(value.ProductVariantId))
            {
                return NotFound();
            }

            if (!await IsProductOwner(value.ProductId))
            {
                return Forbid();
            }

            ProductVariantViewBag();
            var updateDto = new UpdateProductVariantDto
            {
                ProductVariantId = value.ProductVariantId,
                ProductId = value.ProductId,
                VariantName = value.VariantName,
                Sku = value.Sku,
                Size = value.Size,
                Color = value.Color,
                ColorHexCode = value.ColorHexCode,
                MaterialOption = value.MaterialOption,
                CapacityOption = value.CapacityOption,
                StyleOption = value.StyleOption,
                Price = value.Price,
                DiscountedPrice = value.DiscountedPrice,
                DiscountRate = value.DiscountRate,
                Stock = value.Stock,
                MaxOrderQuantity = value.MaxOrderQuantity,
                MainImageUrl = value.MainImageUrl,
                IsDefault = value.IsDefault,
                IsAvailable = value.IsAvailable,
                CreatedDate = value.CreatedDate
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductVariant(UpdateProductVariantDto updateProductVariantDto)
        {
            if (string.IsNullOrEmpty(updateProductVariantDto.ProductId))
            {
                return BadRequest("ProductId boş geliyor.");
            }

            if (!await IsProductOwner(updateProductVariantDto.ProductId))
            {
                return Forbid();
            }

            await _productVariantService.UpdateProductVariantAsync(updateProductVariantDto);
            return Redirect($"/Vendor/ProductVariant/ProductVariantList/{updateProductVariantDto.ProductId}");
        }

        public async Task<IActionResult> DeleteProductVariant(string id)
        {
            var value = await _productVariantService.GetByIdProductVariantAsync(id);

            if (string.IsNullOrEmpty(value.ProductVariantId))
            {
                return NotFound();
            }

            if (!await IsProductOwner(value.ProductId))
            {
                return Forbid();
            }

            await _productVariantService.DeleteProductVariantAsync(id);
            return Redirect($"/Vendor/ProductVariant/ProductVariantList/{value.ProductId}");
        }

        void ProductVariantViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product Variants";
            ViewBag.v3 = "Variant Operations";
            ViewBag.v0 = "Product Variant Operation";
        }
    }
}
