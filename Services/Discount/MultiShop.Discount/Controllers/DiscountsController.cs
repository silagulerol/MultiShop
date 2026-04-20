using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Discount.Dtos;
using MultiShop.Discount.Services;

namespace MultiShop.Discount.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscountAsync()
        {
            var values = await _discountService.GetAllDiscountAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdDiscountCouponAsync(int id)
        {
            var value = await _discountService.GetByIdDiscountCouponAsync(id);
            return Ok(value);
        }

        [HttpGet("GetCodeDetailByCode/{code}")]
        public async Task<IActionResult> GetCodeDetailByCodeAsync(string code)
        {
            var value = await _discountService.GetDiscountCode(code);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscountCouponAsync(CreateDiscountCouponDto createCouponDto)
        {
            await _discountService.CreateDiscountCouponAsync(createCouponDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateCouponDto)
        {
            await _discountService.UpdateDiscountCouponAsync(updateCouponDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDiscountCouponAsync(int id)
        {
            await _discountService.DeleteDiscountCouponAsync(id);
            return Ok();
        }
    }
}
