using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductVariantDtos;
using MultiShop.Catalog.Services.ProductVariantService;

namespace MultiShop.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantsController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductVariants()
        {
            var values = await _productVariantService.GetAllProductVariantAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProductVariant(string id)
        {
            var value = await _productVariantService.GetByIdProductVariantAsync(id);
            return Ok(value);
        }

        [HttpGet("ProductVariantsByProductId/{productId}")]
        public async Task<IActionResult> GetProductVariantsByProductId(string productId)
        {
            var values = await _productVariantService.GetProductVariantsByProductIdAsync(productId);
            return Ok(values);
        }

        [HttpGet("AvailableByProductId/{productId}")]
        public async Task<IActionResult> GetAvailableProductVariantsByProductId(string productId)
        {
            var values = await _productVariantService.GetAvailableProductVariantsByProductIdAsync(productId);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductVariant(CreateProductVariantDto dto)
        {
            await _productVariantService.CreateProductVariantAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductVariant(UpdateProductVariantDto dto)
        {
            await _productVariantService.UpdateProductVariantAsync(dto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductVariant(string id)
        {
            await _productVariantService.DeleteProductVariantAsync(id);
            return Ok();
        }
    }
}
