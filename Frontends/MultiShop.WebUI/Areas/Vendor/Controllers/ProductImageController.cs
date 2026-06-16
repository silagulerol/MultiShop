using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductImageService;
using Newtonsoft.Json;
using System.Text;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;
        private readonly IProductService _productService;

        public ProductImageController(IProductImageService productImageService, IProductService productService)
        {
            _productImageService = productImageService;
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
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            if (!await IsProductOwner(id))
            {
                return Forbid();
            }

            ProductImageViewBag();
            var values = await _productImageService.GetByProductIdProductImageAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> ProductImageDetail(UpdateProductImageDto updateProductImageDto)
        {
            if (string.IsNullOrEmpty(updateProductImageDto.ProductId))
            {
                return BadRequest("ProductId boş geliyor.");
            }

            if (!await IsProductOwner(updateProductImageDto.ProductId))
            {
                return Forbid();
            }

            if (string.IsNullOrEmpty(updateProductImageDto.ProductImageId))
            {
                var createProductImageDto = new CreateProductImageDto
                {
                    ProductId = updateProductImageDto.ProductId,
                    Image1 = updateProductImageDto.Image1,
                    Image2 = updateProductImageDto.Image2,
                    Image3 = updateProductImageDto.Image3,
                    Image4 = updateProductImageDto.Image4
                };

                await _productImageService.CreateProductImageAsync(createProductImageDto);
            }
            else
            {
                await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            }

            return Redirect("/Vendor/Product/GetProductsWithCategory");
        }

        void ProductImageViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product Images";
            ViewBag.v3 = "Product Image Update";
            ViewBag.v0 = "Product Image Operation";
        }
    }
}
