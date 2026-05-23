using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Services.ProductDetailService;

namespace MultiShop.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailsController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductDetails()
        {
            var values = await _productDetailService.GetAllProductDetailAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProductDetail(string id)
        {
            var value = await _productDetailService.GetByIdProductDetailAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductDetail(CreateProductDetailDto dto)
        {
            await _productDetailService.CreateProductDetailAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto dto)
        {
            await _productDetailService.UpdateProductDetailAsync(dto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductDetail(string id)
        {
            await _productDetailService.DeleteProductDetailAsync(id);
            return Ok();
        }

        [HttpGet("ProductDetailByProductId/{productId}")]
        public async Task<IActionResult> GetProductDetailByProductId(string productId)
        {
            var value = await _productDetailService.GetByProductIdProductDetailAsync(productId);
            return Ok(value);
        }

        [HttpGet("ProductDetailByVariantId/{variantId}")]
        public async Task<IActionResult> GetProductDetailByVariantId(string variantId)
        {
            var value = await _productDetailService.GetProductDetailByVariantIdAsync(variantId);
            return Ok(value);
        }
    }
}
