using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductImageService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            ProductImageViewBag();
            var values= await _productImageService.GetByProductIdProductImageAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> ProductImageDetail(UpdateProductImageDto updateProductImageDto)
        {
            ProductImageViewBag();

            if (string.IsNullOrEmpty(updateProductImageDto.ProductImageId))
            {
                var createProductImageDto = new CreateProductImageDto
                {
                    ProductId = updateProductImageDto.ProductId,
                    ImageUrl = updateProductImageDto.ImageUrl,
                    DisplayOrder = updateProductImageDto.DisplayOrder,
                    IsMainImage = updateProductImageDto.IsMainImage,
                    ImageAltText = updateProductImageDto.ImageAltText,
                    ImageType = updateProductImageDto.ImageType,
                    CreatedDate = updateProductImageDto.CreatedDate == default
                        ? DateTime.UtcNow
                        : updateProductImageDto.CreatedDate
                };

                await _productImageService.CreateProductImageAsync(createProductImageDto);
            }
            else
            {
                await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            }

            return Redirect("/Admin/Product/GetProductsWithCategory"); 
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
