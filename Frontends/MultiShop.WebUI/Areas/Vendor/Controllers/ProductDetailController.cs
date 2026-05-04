using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDetailDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductDetailService;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailService _productDetailService;
        private readonly IProductService _productService;

        public ProductDetailController(
            IProductDetailService productDetailService,
            IProductService productService)
        {
            _productDetailService = productDetailService;
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
                return false;

            return product.VendorId == GetCurrentVendorId();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            if (!await IsProductOwner(id))
                return Forbid();

            ProductDetailViewBag();

            var value = await _productDetailService.GetByProductIdProductDetailAsync(id);

            if (value == null)
            {
                value = new UpdateProductDetailDto
                {
                    ProductId = id
                };
            }

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto updateProductDetailDto)
        {
            if (string.IsNullOrEmpty(updateProductDetailDto.ProductId))
                return BadRequest("ProductId boş geliyor.");

            if (!await IsProductOwner(updateProductDetailDto.ProductId))
                return Forbid();

            if (string.IsNullOrEmpty(updateProductDetailDto.ProductDetailId))
            {
                var createDto = new CreateProductDetailDto
                {
                    ProductId = updateProductDetailDto.ProductId,
                    ProductLongDescription = updateProductDetailDto.ProductLongDescription,
                    ProductInformation = updateProductDetailDto.ProductInformation
                };

                await _productDetailService.CreateProductDetailAsync(createDto);
            }
            else
            {
                await _productDetailService.UpdateProductDetailAsync(updateProductDetailDto);
            }

            return Redirect("/Vendor/Product/GetProductsWithCategory");
        }

        void ProductDetailViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product Details";
            ViewBag.v3 = "Product Detail Update";
            ViewBag.v0 = "Product Detail Operation";
        }
    }
}