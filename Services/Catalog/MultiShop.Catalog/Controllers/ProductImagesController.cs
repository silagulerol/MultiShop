using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Services.ProductImageService;

namespace MultiShop.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetAllProductImages()
        {
            var values = await _productImageService.GetAllProductImageAsync();
            return Ok(values);
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProductImage(string id)
        {
            var value = await _productImageService.GetByIdProductImageAsync(id);
            return Ok(value);
        }

        [HttpGet("ImagesByProductId/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(string productId)
        {
            var values = await _productImageService.GetImagesByProductIdAsync(productId);
            return Ok(values);
        }

        [HttpGet("ImagesByVariantId/{variantId}")]
        public async Task<IActionResult> GetImagesByVariantId(string variantId)
        {
            var values = await _productImageService.GetImagesByVariantIdAsync(variantId);
            return Ok(values);
        }

        [HttpGet("MainImageByProductId/{productId}")]
        public async Task<IActionResult> GetMainImageByProductId(string productId)
        {
            var value = await _productImageService.GetMainImageByProductIdAsync(productId);
            return Ok(value);
        }

        [HttpGet("MainImageByVariantId/{variantId}")]
        public async Task<IActionResult> GetMainImageByVariantId(string variantId)
        {
            var value = await _productImageService.GetMainImageByVariantIdAsync(variantId);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageDto dto)
        {
            await _productImageService.CreateProductImageAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto dto)
        {
            await _productImageService.UpdateProductImageAsync(dto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            await _productImageService.DeleteProductImageAsync(id);
            return Ok();
        }   

    }
}
